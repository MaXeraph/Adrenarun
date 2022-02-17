using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassUI : MonoBehaviour
{
    public static CompassUI Instance;
    //public RawImage CompassImage;
    public PlayerControls PlayerController;

    public Transform CameraTransform;
    public RectTransform CompassBar;

    public RectTransform NorthMarker;
    public RectTransform SouthMarker;
    public RectTransform WestMarker;
    public RectTransform EastMarker;
    public RectTransform NWMarker;
    public RectTransform NEMarker;
    public RectTransform SWMarker;
    public RectTransform SEMarker;

   // private void Awake()
   // {
   //     if (Instance == null)
   //     {
   //         Instance = this;
   //         DontDestroyOnLoad(gameObject);
   //     }
   //     else if (Instance != this)
   //     {
    //        Destroy(gameObject);
    //    }
    //}

    void Update()
    {
        SetMarkerPosition(NorthMarker, Vector3.forward * 1000);
        SetMarkerPosition(SouthMarker, Vector3.back * 1000);
        SetMarkerPosition(WestMarker, Vector3.left * 1000);
        SetMarkerPosition(EastMarker, Vector3.right * 1000);
        SetMarkerPosition(NWMarker, (Vector3.right + Vector3.forward) * 1000);
        SetMarkerPosition(SWMarker, (Vector3.right + Vector3.back) * 1000);
        SetMarkerPosition(NEMarker, (Vector3.left + Vector3.forward) * 1000);
        SetMarkerPosition(SEMarker, (Vector3.left + Vector3.back) * 1000);
    }

    private void SetMarkerPosition(RectTransform markerTransform, Vector3 worldPosition)
    {
        Vector3 directionToTarget = worldPosition - CameraTransform.position;
        float angle = Vector2.SignedAngle(new Vector2(directionToTarget.x, directionToTarget.z), new Vector2(CameraTransform.transform.forward.x, CameraTransform.transform.forward.z));
        float compassPosX = Mathf.Clamp(2 * angle / Camera.main.fieldOfView, -1, 1);
        markerTransform.anchoredPosition = new Vector3(CompassBar.rect.width / 1.9f * compassPosX, 0, 0);
    }

    //private void LateUpdate() => UpdateCompassHeading();

   // private void UpdateCompassHeading()
   // {
   //     if (PlayerController == null)
   //     { return; }
   //     Vector2 compassUvPosition = Vector2.right *
   //         (PlayerController.transform.rotation.eulerAngles.y / 360);
   //     CompassImage.uvRect = new Rect(compassUvPosition, Vector2.one);
   // }
}
