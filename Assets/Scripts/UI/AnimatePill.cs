using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatePill : MonoBehaviour
{
 

	void Update()
	{
		gameObject.transform.rotation *= Quaternion.AngleAxis(-3, Vector3.forward);
	}
}
