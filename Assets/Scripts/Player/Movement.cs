using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Movement variables
    public static float speed = 12f;
    public static float sprintSpeed = 0.05f;

    // Jump variables
    // NOTE: -45.81 is an experimental value for gravity
    public static float jumpHeight = 3f;
    public static float jumpVelocity = Mathf.Sqrt(jumpHeight * -2f * -45.81f);

    // MouseLook variables
    static float mouseSensitivity = 100f;
    static float xRotation = 0f;

    public static void RotatePlayer(GameObject player, Camera camera)
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        player.transform.Rotate(Vector3.up * mouseX);
        CompassUI.updateCompass();
    }
    public static void MoveXY(GameObject player)
    {
        CharacterController char_controller = player.GetComponent<CharacterController>();

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 move = player.transform.right * x + player.transform.forward * y;
        char_controller.Move(move * speed * Time.deltaTime * SpeedManager.playerMovementScaling);
    }

    public static void playerSprint(GameObject player)
    {
        CharacterController char_controller = player.GetComponent<CharacterController>();
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 move = player.transform.right * x + player.transform.forward * y;

        char_controller.Move(move * speed * Time.deltaTime * sprintSpeed * SpeedManager.playerMovementScaling);
    }
}
