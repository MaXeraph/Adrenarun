using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCentral : MonoBehaviour
{
    private GameObject _player;
    private Camera _camera;
    private Weapon _weapon;
    public static bool paused = false;

    private Vector3 _velocity;

    bool isGrounded;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        
        _player = GameObject.FindWithTag("Player");

        _camera = Camera.main;

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
            _velocity.y += Movement.jumpVelocity;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            Movement.playerDash(_player);
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
