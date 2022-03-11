using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ammoSlot : MonoBehaviour
{
    public bool used
    {
        set { _used = value; change_state(); }
        get { return _used; }
    }

    public bool _used = false;

    private void change_state()
    {
        transform.DOKill();
        if (_used) { transform.DOScale(new Vector3(0f, 0f, 0f), 0.2f); }
        else { transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f); }
    }


}
