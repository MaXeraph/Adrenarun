using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairUI : MonoBehaviour
{

    public static Transform LookDirection;

    public static CrosshairUI instance;

    void Awake() => instance = this;

    void Start()
    {
        LookDirection = transform.GetChild(0);
    }

    public static void addIndicator(Vector3 pos)
    {
        GameObject indicator = Instantiate(Resources.Load("BulletIndicator")) as GameObject;
       
        Transform IndTrans = indicator.GetComponent<Transform>();
        IndTrans.position = instance.transform.position;
        IndTrans.transform.SetParent(instance.transform);

        LookDirection.LookAt(pos);
        Vector3 IndicatorRot = LookDirection.forward;
        IndicatorRot = new Vector3(0, 0, IndicatorRot.y); ;
        //IndTrans.localRotation = Quaternion.(0, 0, IndicatorRot.y);
        Vector3 directionToTarget = pos - Camera.main.transform.forward;
        //float angle = Vector3.Angle(Camera.main.transform.forward,pos);
        float angle = Vector3.SignedAngle(directionToTarget, Camera.main.transform.forward, Vector3.up);

        //IndTrans.TransformPoint(new Vector3(0, 0, IndicatorRot.y));
        Debug.Log(angle);
        angle = Mathf.Clamp(2 * angle / Camera.main.fieldOfView, -360, 360);
        IndTrans.localRotation = Quaternion.Euler(0, 0, angle);
    }

}
