using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject _player;
    private GameObject _camera;
    private GameObject _gun;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _player = GameObject.FindWithTag("Player");
        _camera = GameObject.FindWithTag("MainCamera");
        // _gun = GameObject.FindWithTag("Gun");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            Movement.RotatePlayer(_player, _camera);
        }

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            Movement.MoveXY(_player);
        }

        if (Input.GetButtonDown("Jump"))
        {
            Movement.Jump(_player);
        }

        if (Input.GetMouseButton(0))
        {
            ShootControl.Shoot(_gun);
        }
    }
}
