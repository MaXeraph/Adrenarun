using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    static float mouseSensitivity = 4500f;
    static float xRotation = 0f; 
    public static void RotatePlayer(GameObject player, GameObject camera)
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime * 2;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime * 2;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        player.transform.Rotate(Vector3.up * mouseX);
    }
    public static void MoveXY(GameObject player)
    {   
        //TODO: implement movement
        Debug.Log("Movement not implemented");
    }
    public static void Jump(GameObject player)
    {   
        //TODO: implement jump
        Debug.Log("Jump not implemented");
    }
}
