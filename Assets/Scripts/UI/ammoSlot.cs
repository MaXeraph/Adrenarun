using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ammoSlot : MonoBehaviour
{
	private bool _used = false;
	private bool transitionTo = true;
	public bool used
	{
		get { return _used; }
		set
		{
			update(value);
		}
	}

	void update(bool state)
    {
		transitionTo = state;
		float state_value = System.Convert.ToInt32(!state);
		DOTween.Kill(this);
		
		transform.DOScale(new Vector3(state_value/1f, state_value, state_value), 0.2f).OnComplete(finishState);
    }

	void finishState()
    {
		_used = transitionTo;
    }

}
