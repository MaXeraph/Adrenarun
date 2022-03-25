using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class CrosshairUI : MonoBehaviour
{

    public static Transform LookDirection;

    public static CrosshairUI instance;
	public static Image dashCircle;

    void Awake() => instance = this;

    void Start()
    {
        LookDirection = transform.GetChild(0);
		dashCircle = transform.GetChild(2).GetComponent<Image>();
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
  
        Vector3 directionToTarget = pos - Camera.main.transform.forward;

        float angle = Vector3.SignedAngle(directionToTarget, Camera.main.transform.forward, Vector3.up);

        IndTrans.localRotation = Quaternion.Euler(0, 0, angle);
    }

	public static void dash(float duration)
    {
		dashCircle.fillAmount = 0;
		dashCircle.DOFillAmount(100, duration).OnComplete(flash);
    }

	static void flash()
    {
		dashCircle.DOColor(Color.white, 0.1f).SetLoops(2, LoopType.Yoyo);
    }

}
