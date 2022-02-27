using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCentral : MonoBehaviour
{
    private static GameObject _player;
    private Camera _camera;
    private Weapon _weapon;
    private static CharacterController _controller;
    static PlayerCentral instance;

    public Transform arms;
    public Transform gun;

    static private Vector3 _velocity;
    static private Vector3 prev_dir;

    public static float dashLength = 0.2f;
    public static float dashCooldown = 0.5f;
    static float lastDash = 0f;

    static bool isGrounded;
    static bool canWallJump;
    float wallJumpSlope = 0.1f;


    private CharacterState state = new IdleState();
    private CharacterState prevState;

    void Awake() => instance = this;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _player = GameObject.FindWithTag("Player");

        _camera = Camera.main;

        _controller = GetComponent<CharacterController>();

        _weapon = _player.AddComponent<Weapon>();
        _weapon.Initialize(new BulletAttackBehaviour(EntityType.PLAYER), 0.2f, 16, 1f);
    }

    void Update()
    {
        // Seperates functionality between different movement states and handles transitions between them
        manageStates();

        //Look
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) Movement.RotatePlayer(_player, _camera);

        //Shoot
        if (Input.GetButton("Fire1"))
        {
            Vector3 position = _camera.transform.forward + _camera.transform.position + (0.22f* _camera.transform.right) + (-0.18f * _camera.transform.up);
            Vector3 direction = _camera.transform.forward + new Vector3(-0.0075f,0.003f,0);
            if (_weapon.Attack(position, direction)) shootEffects(position);
        }

        //Reload
        if (Input.GetButtonDown("Reload")) _weapon.Reload();

    }

    private void manageStates()
    {
        //State Managment
        prevState = state;
        //Update Current State
        state = state.handleInput();
        state = state.process();
        //Handle state transitions
        if (state != prevState) 
        { 
            prevState.exit(); 
            state.enter(); 
        }
    }

    private void shootEffects(Vector3 pos)
    {
        //Update UI
        UIManager.Ammo -= 1;

        //Muzzleflash
        GameObject flash = Instantiate(Resources.Load("Muzzleflash")) as GameObject;
        flash.transform.position = pos;
        flash.transform.right = Camera.main.transform.forward;
        flash.transform.Rotate(Random.Range(0, 360), 0, 0);

        //Recoil tween
        Sequence RecoilSequence = DOTween.Sequence();
        RecoilSequence.Insert(0, arms.DOPunchRotation(new Vector3(0, 0, -1f), _weapon._fireRate / 2, 0, 0.5f)); 
        RecoilSequence.Insert(0, gun.DOPunchRotation(new Vector3(-1f, 0, 0), _weapon._fireRate / 2, 0, 0.5f));
    }

    private static void checkGround()
    {
        Vector3 origin = new Vector3(instance.transform.position.x, instance.transform.position.y - (instance.transform.localScale.y * .5f), instance.transform.position.z);
        Vector3 direction = instance.transform.TransformDirection(Vector3.down);
        float distance = 0.6f;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
        {
            _velocity = new Vector3(0f, _velocity.y, 0f);
            isGrounded = true;
            canWallJump = true;
        }
        else isGrounded = false;
    }

    private static void checkDirection()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)  
            Movement.MoveXY(_player);

    }

    private static void applyGravity()
    {
        _velocity.y += -45.81f * Time.deltaTime * SpeedManager.playerMovementScaling;
        _controller.Move(_velocity * Time.deltaTime * SpeedManager.playerMovementScaling);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        bool wallJumpableSurface = hit.normal.y < wallJumpSlope;
        if (!isGrounded && wallJumpableSurface && Input.GetButtonDown("Jump") && canWallJump)
        {
            Debug.Log("JUMPED!!");
            canWallJump = false;
            //Wall jump now kicks off from the wall on X/Z vectors
            Vector3 target = hit.normal * Movement.speed * SpeedManager.playerMovementScaling;
            _velocity = target;
            _velocity.y = Movement.jumpVelocity;

        }
    }

    private static bool checkDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= (lastDash + dashCooldown)) return true;
        else return false;
    }


    // This should make more complex movement/animations easier and more stable
    //Base Character State Class
    class CharacterState
    {
        //On state Enter
        virtual public void enter() { }
        //Handle Input every frame
        virtual public CharacterState handleInput() { return this; }
        //Handle Other stuff every frame
        virtual public CharacterState process() { return this; }
        //On state Exit
        virtual public void exit() { }
    }

    //Idle state
    class IdleState : CharacterState
    {
        public override void enter()
        {
            _velocity.y = 0;
        }

        public override CharacterState handleInput()
        {
            //Can jump only from this state or during wall collision
            if (isGrounded && Input.GetButtonDown("Jump")) return new JumpState();

            if (checkDash()) return new DashState();

            return this;
        }

        public override CharacterState process()
        {
            checkGround();
            checkDirection();
            if (!isGrounded)
            {
                return new FallState();
            }
            return this;
        }
    }

    //Jump state
    class JumpState : CharacterState
    {
        public override void enter()
        {
            _velocity.y += Movement.jumpVelocity;
        }

        public override CharacterState handleInput()
        {
            if (checkDash()) return new DashState();
            return this;
        }

        public override CharacterState process()
        {
            applyGravity();
            checkDirection();
            if (_velocity.y < -3) return new FallState();
            return this;
        }
    }

    //Fall state
    class FallState : CharacterState
    {
        public override CharacterState handleInput()
        {
            if (checkDash()) return new DashState();
            return this;
        }

        public override CharacterState process()
        {
            checkGround();
            applyGravity();
            checkDirection();
            if (isGrounded) return new IdleState();
            return this;
        }
    }

    //Dash state
    class DashState : CharacterState
    {
        float dash_start;
        Vector3 dir;
        bool dashing = false;
        float mult = 1;
        float delay = 0.075f;
        

        public override void enter()
        {
            dir = Camera.main.transform.forward;
            _velocity.y = 0;
            dash_start = Time.time;
            dashing = true;
            DOTween.To(x => mult = x, 1f, 0f, dashLength-delay).SetDelay(delay);
        }

        public override CharacterState process()
        {
            Movement.playerDash(_player, dir*mult);
            if (dashing && Time.time >= (dash_start + dashLength)) return new FallState();
            return this;
        }

        public override void exit() { lastDash = Time.time; }
    }
}
