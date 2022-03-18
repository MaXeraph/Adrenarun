using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Portal : MonoBehaviour
{
	public bool reverseExitDirection = false;
	private float exitDirection = 1.05f;
	public GameObject portalOpposite;
	Vector3 distanceToExit;
	void Start()
	{
		if (reverseExitDirection) exitDirection = 0.97f;
		distanceToExit = portalOpposite.transform.position - transform.position;
	}

	void OnTriggerEnter(Collider c)
	{
		Debug.Log(c.gameObject.tag);
		Debug.Log("distance: " + distanceToExit);
		if (c.gameObject.tag == "Player")
		{
			if(reverseExitDirection) Camera.main.transform.localRotation = Quaternion.Inverse(Camera.main.transform.rotation);
			Movement.RotatePlayer(c.gameObject, Camera.main);

			c.GetComponent<CharacterController>().enabled = false;
			c.transform.position = c.transform.position + distanceToExit * exitDirection;
			c.GetComponent<CharacterController>().enabled = true;
		}
		else if (c.gameObject.tag == "Enemy")
		{
			c.GetComponent<EnemyBehaviour>().enabled = false;
			c.GetComponent<NavMeshAgent>().SetDestination(portalOpposite.transform.position * 1.05f);
			c.GetComponent<EnemyBehaviour>().enabled = true;
		} else {
			c.transform.position = c.transform.position + distanceToExit * 1.05f;
		}

	}
}
