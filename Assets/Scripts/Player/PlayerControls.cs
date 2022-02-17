using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    private GameObject _player;

    private PlayerStats stats;

    private Weapon _weapon;

    private Vector3 _velocity;
    private Camera _camera;

    bool isGrounded;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _player = GameObject.FindWithTag("Player");
        stats = GetComponent<PlayerStats>();
        _camera = Camera.main;
        _weapon = GetComponent<Weapon>();
    }

    void Update()
    {
        checkGround();

        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            Movement.RotatePlayer(_player, _camera.gameObject);
        }

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            Movement.MoveXY(_player);
            if (isGrounded) { GetComponent<bob>().update_bob(); }
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
            _weapon.Attack(position, direction);
            stats.Ammo -= 1;
        }
        if (Input.GetButtonDown("Reload"))
        {
            _weapon.Reload();
            stats.Reloading = true;
        }


        
        applyGravity();
        resetYVelocity();

    }

    private void checkGround()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * .5f), transform.position.z);
        Vector3 direction = transform.TransformDirection(Vector3.down);
        float distance = 1.1f;

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
        _velocity.y += -45.81f * Time.deltaTime;
        char_controller.Move(_velocity * Time.deltaTime);
    }
}
