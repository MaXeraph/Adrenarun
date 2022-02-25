using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassUI : MonoBehaviour
{
    public static CompassUI Instance;
    private static Transform CameraTransform;
    private static RectTransform CompassBar;

    private static RectTransform[] directionTransforms = new RectTransform[8];

    private static string[] directionNames = new string[] { "North", "South", "East", "West", "NW", "NE", "SW", "SE" };

    private static Vector3[] directionVectors = new Vector3[] {Vector3.forward* 1000, Vector3.back* 1000, Vector3.right* 1000, Vector3.left* 1000,
                                                       (Vector3.right + Vector3.forward)* 1000, (Vector3.left + Vector3.forward)* 1000, (Vector3.right + Vector3.back)* 1000, (Vector3.left + Vector3.back)* 1000 };

    void Awake()
    {
        CameraTransform = Camera.main.transform;
        CompassBar = transform.GetChild(1).GetComponent<RectTransform>();
        for (int i = 0;i < directionNames.Length; i++)
        {
            string _name = directionNames[i];
            directionTransforms[i] = CompassBar.Find(_name).GetComponent<RectTransform>();
        }
    }

    public static void updateCompass()
    {
        for (int i = 0; i < directionNames.Length; i++)
        {
            SetMarkerPosition(directionTransforms[i], directionVectors[i]);
        }
    }

    private static void SetMarkerPosition(RectTransform markerTransform, Vector3 worldPosition)
    {
        Vector3 directionToTarget = worldPosition - CameraTransform.position;
        float angle = Vector2.SignedAngle(new Vector2(directionToTarget.x, directionToTarget.z), new Vector2(CameraTransform.transform.forward.x, CameraTransform.transform.forward.z));
        float compassPosX = Mathf.Clamp(2 * angle / Camera.main.fieldOfView, -1, 1);
        markerTransform.anchoredPosition = new Vector3(CompassBar.rect.width / 1.9f * compassPosX, 0, 0);
    }

}
