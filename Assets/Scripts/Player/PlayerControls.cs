using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    private GameObject _player;
    private GameObject _camera;
    private GameObject _gun;
    //private PlayerStats stats;

    private Vector3 _velocity;

    bool isGrounded;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _player = GameObject.FindWithTag("Player");
        UIManager.AmmoCapacity = 16;
        UIManager.Ammo = 16;
        UIManager.MaxHealth = 100;
        UIManager.Health = 100;

        _camera = Camera.main;
        _weapon = new Weapon(new BulletAttackBehaviour(EntityType.PLAYER));

    }

    void Update()
    {
        checkGround();

        if (Input.GetKeyDown(KeyCode.Equals)) { UIManager.Health += 10f; }
        if (Input.GetKeyDown(KeyCode.Minus)) { UIManager.Health -= 10f; }



        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            Movement.RotatePlayer(_player, _camera);
        }

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            Movement.MoveXY(_player);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            _velocity.y += Movement.jumpVelocity;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            Movement.playerDash(_player);
        }

        if (Input.GetMouseButtonDown(0))
        {
            ShootControl.Shoot(_gun);
           // PlayerStats.Ammo -= 1;
        }

        applyGravity();
        resetYVelocity();

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
        CharacterController char_controller = _player.GetComponent<CharacterController>();
        _velocity.y += -45.81f * Time.deltaTime * SpeedManager.playerMovementScaling;
        char_controller.Move(_velocity * Time.deltaTime * SpeedManager.playerMovementScaling);
    }
}
