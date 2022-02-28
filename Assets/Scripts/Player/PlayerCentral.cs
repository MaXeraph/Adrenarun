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
    float wallJumpSlope = 0.1f;

    void Start()
    {
       // Cursor.lockState = CursorLockMode.Locked;
        
        _player = GameObject.FindWithTag("Player");

        _camera = Camera.main;

        _controller = GetComponent<CharacterController>();

        arms = transform.GetChild(0).GetChild(0).GetChild(0).Find("arms");
        gun = transform.GetChild(0).GetChild(0).GetChild(0).Find("gunF");
        
        _weapon = _player.AddComponent<Weapon>();
        _weapon.Initialize(new BulletAttackBehaviour(EntityType.PLAYER), 0.2f, 16, 1f);
    }


    void Update()
    {
        if (paused) return;

        checkGround();

        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            Movement.RotatePlayer(_player, _camera);
            CompassUI.updateCompass();
        }

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            Movement.MoveXY(_player);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            GetComponent<PowerUpManager>().presentPowerUps();
            GetComponent<Stats>().takeDamage(10f);
            _velocity.y += Movement.jumpVelocity;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            Movement.playerDash(_player);
        }

        //Shoot
        if (Input.GetButton("Fire1"))
        {
            Vector3 position = _camera.transform.forward + _camera.transform.position + (0.22f * _camera.transform.right) + (-0.18f * _camera.transform.up);
            Vector3 direction = _camera.transform.forward + new Vector3(-0.0075f, 0.003f, 0);
            if (_weapon.Attack(position, direction)) shootEffects(position);
        }
        if (Input.GetButtonDown("Reload"))
        {
            _weapon.Reload();
            UIManager.Reloading = true;
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
            _velocity.y = Movement.jumpVelocity;
        }
    }
}
