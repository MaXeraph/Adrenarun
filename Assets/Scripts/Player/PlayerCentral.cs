using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCentral : MonoBehaviour
{
    private static GameObject _player;
    private Camera _camera;
    private Weapon _weapon;
    private static CharacterController _controller;
    static PlayerCentral instance;

    static private Vector3 _velocity;
    static private Vector3 prev_dir;

    public static float dashLength = 0.12f;
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
        // Seperates functionality between different states and handles transitions between them
        manageStates();

        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            Movement.RotatePlayer(_player, _camera);
            CompassUI.updateCompass();
        }

        if (Input.GetButton("Fire1"))
        {
            Vector3 position = _camera.transform.forward + _camera.transform.position;
            Vector3 direction = _camera.transform.forward;
            if (_weapon.Attack(position, direction)) { UIManager.Ammo -= 1; }
        }

        if (Input.GetButtonDown("Reload"))
        {
            _weapon.Reload();
            UIManager.Reloading = true;
        }

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

    private static void checkGround()
    {
        Vector3 origin = new Vector3(instance.transform.position.x, instance.transform.position.y - (instance.transform.localScale.y * .5f), instance.transform.position.z);
        Vector3 direction = instance.transform.TransformDirection(Vector3.down);
        float distance = 0.6f;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
        {
            _velocity = new Vector3 (0f, _velocity.y, 0f);
            //Debug.DrawRay(origin, direction * distance, Color.red);
            isGrounded = true;
            canWallJump = true;

        }
        else
        {
            isGrounded = false;
        }
    }

    private static void checkDirection()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            Vector3 move = Movement.MoveXY(_player);
            if (move != Vector3.zero)
            {
                prev_dir = move;
            }
        }
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
            canWallJump = false;
            //Wall jump now kicks off from the wall on X/Z vectors
            Vector3 target = hit.normal * Movement.speed * SpeedManager.playerMovementScaling;
            _velocity = target;
            _velocity.y = Movement.jumpVelocity;

        }
        //Fall slightly slower against falls
        else if (!isGrounded && _velocity.y < 0) _velocity.y+=0.5f;
    }

    // This should make more complex movement/animations much easier and more stable
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

            if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= (lastDash + dashCooldown)) return new DashState();

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
            if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= (lastDash + dashCooldown)) return new DashState();
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
            if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= (lastDash + dashCooldown)) return new DashState();
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
        private float dash_start;
        private Vector3 dir;
        private bool dashing = false;

        public override void enter()
        {
            if (prev_dir == Vector3.zero) prev_dir = _player.transform.forward;
            _velocity.y = 0;
            dash_start = Time.time;
            dir = prev_dir;
            dashing = true;
        }

        public override CharacterState process()
        {
            Movement.playerDash(_player, dir);
            if (dashing && Time.time >= (dash_start + dashLength)) return new FallState();
            return this;
        }

        public override void exit() { lastDash = Time.time; }
    }
}
