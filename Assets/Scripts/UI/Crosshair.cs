using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Crosshair : MonoBehaviour
{
    public Image center;
    public Image border;

    void Update()
    {
        check_target();
    }

    public void set_color(bool hit, bool critical)
    {
        Vector4 col = new Vector4(0.1f, 1f, 0f, 0.7f);
        if (hit)
        {
            col = new Vector4(1f, 0f, 0.1f, 0.7f);
        }
        border.DOColor(col, 0.2f);
        if (!critical && hit) { col = new Vector4(1f, 1f, 0f, 1f); }
        center.DOColor(col, 0.2f);
    }

    void check_target()
    {
        RaycastHit hit;
        Vector3 origin = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
        Vector3 direction = Camera.main.transform.TransformDirection(Vector3.forward);
        float distance = 500f;
        if (Physics.Raycast(origin, direction, out hit, distance))
        {
            //If collider is in the target layer
            if (hit.collider.gameObject.layer == 3)
            {
                //If collider has the crit tag
                bool crit = hit.collider.CompareTag("Crit");
                Debug.DrawRay(origin, direction * distance, Color.red);
                set_color(true, crit);
                center.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.4f);
                border.transform.DOScale(new Vector3(1.25f, 1.25f, 1.25f), 1.25f);
                return;
            }
            else { revert(); Debug.DrawRay(origin, direction * distance, Color.green); }
        }
        else { revert(); Debug.DrawRay(origin, direction * distance, Color.green); }

        
    }

    void revert()
    {
        set_color(false, false);
        center.transform.DOScale(new Vector3(1, 1, 1), 0.4f);
        border.transform.DOScale(new Vector3(1, 1, 1), 0.4f);
       
    }
}
