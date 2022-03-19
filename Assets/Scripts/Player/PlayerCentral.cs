using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCentral : MonoBehaviour
{
    private GameObject _player;
    private Camera _camera;
    private Weapon _weapon;
    public static bool paused = false;

    private Vector3 _velocity;
    private CharacterController _controller;

    Transform arms;
    Transform gun;

    bool isGrounded;
    bool canWallJump;
    bool _cooldown = false;
    float dashCD = 3f;
    double lastDashTime = -3;
    float wallJumpSlope = 0.1f;
    Vector3 wallJumpVector;
	private int _healingPills = 0;
	public int healingPills {
		get => _healingPills;
		set 
		{
			_healingPills = Mathf.Clamp(value, 0, 2);
			ConsumableUI.update_pill_amount(value);
		}
	}


    void Start()
    {
       Cursor.lockState = CursorLockMode.Locked;

        _player = GameObject.FindWithTag("Player");

        _camera = Camera.main;

        _controller = GetComponent<CharacterController>();

        arms = transform.GetChild(0).GetChild(0).GetChild(0).Find("arms");
        gun = transform.GetChild(0).GetChild(0).GetChild(0).Find("gunF");

        _weapon = _player.AddComponent<Weapon>();
        _weapon.Initialize(new BulletAttackBehaviour(EntityType.PLAYER, damage: 10f, bulletSpeed:30f), 0.2f, 16, 1f);

    }

	private bool test = false;
	private GameObject o = null;

    void Update()
    {
        if (paused) return;

        checkGround();

		if (Input.GetKey(KeyCode.O) && !test) {
			test = true;
			o = EnemyFactory.Instance.CreateEnemy(new Vector3(0, 2, 8), EnemyType.TANK, EnemyVariantType.SHIELD);
		}
		if (Input.GetKey(KeyCode.P) && test) {
			test = false;
			// Destroy(o);
		}

        //Look
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) Movement.RotatePlayer(_player, _camera);
        //Moved out of movement because now it shows enemy position and needs to updated every frame
        CompassUI.updateCompass();

        //Move
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
            Movement.MoveXY(_player);
            AudioManager.PlayWalkAudio();
        }
        else if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0) {
            AudioManager.StopWalkAudio();
        }

        //Jump
        if (Input.GetButtonDown("Jump") && isGrounded) 
        {
            _velocity.y += (Movement.jumpVelocity - 2f) + 1.5f * (SpeedManager.playerMovementScaling);
        }

        // if (Input.GetKey(KeyCode.LeftShift))
        // {
        //     Movement.playerSprint(_player);
        // }

        //Dash
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Vector3 XZInputVector = new Vector3(_controller.velocity.x, 0, _controller.velocity.z);
            if(SpeedManager.realTime - dashCD > lastDashTime && XZInputVector.magnitude > 0f){
                StartCoroutine(Dash());
                lastDashTime = SpeedManager.realTime;
                AudioManager.PlayDashAudio();
            }
        }

        //Shoot
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Vector3 position = _camera.transform.forward + _camera.transform.position;
            Vector3 direction = _camera.transform.forward;
            if (_weapon.Attack(position, direction, EntityType.PLAYER)) shootEffects(position + (0.22f * _camera.transform.right) + (-0.18f * _camera.transform.up));
        }

        //Reload
        if (Input.GetButtonDown("Reload")) _weapon.Reload();

		//Healing Pill
		if (Input.GetKeyDown(KeyCode.Q) && healingPills > 0) 
		{
			healingPills -= 1;
			_player.GetComponent<Stats>().currentHealth += 15;
		
		}


        applyGravity();
        resetYVelocity();

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

    private void checkGround()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * .5f), transform.position.z);
        Vector3 direction = transform.TransformDirection(Vector3.down);
        float distance = 0.6f;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
        {
            Debug.DrawRay(origin, direction * distance, Color.red);
            isGrounded = true;
            canWallJump = true;

        }
        else
        {
            isGrounded = false;
        }

    }

    private void resetYVelocity()
    {
        if(isGrounded && _velocity.y < 0){
            _velocity.y = 0f;
        }
    }

    private void descaleWallJumpVector()
    {
        wallJumpVector.x *= 0.99f;
        wallJumpVector.z *= 0.99f;
    }

    private void applyGravity()
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
            StartCoroutine(wallJump(hit.normal));
        }
    }

    IEnumerator Dash()
    {
        float startTime = Time.time;
        float ad_input = Input.GetAxis("Horizontal");
        float ws_input = Input.GetAxis("Vertical");

        float dashSpeed = 60f * SpeedManager.playerMovementScaling;
        float dashTime = 0.2f/SpeedManager.playerMovementScaling;
        Vector3 move = _player.transform.right * ad_input + _player.transform.forward * ws_input;
        while(Time.time < startTime + dashTime)
        {
            _controller.Move(move.normalized * dashSpeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator wallJump(Vector3 normal)
    {
        float wallJumpForce = 20f;
        _velocity.y = (Movement.jumpVelocity - 2) + 1.5f * SpeedManager.playerMovementScaling;
        wallJumpVector = normal * wallJumpForce * SpeedManager.playerMovementScaling;
        while(!isGrounded)
        {
            descaleWallJumpVector();
            _controller.Move(wallJumpVector * Time.deltaTime);
            yield return null;
        }

    }
}
