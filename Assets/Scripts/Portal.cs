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
	Vector3 locationAfterTeleport;
	void Start()
	{
		if (reverseExitDirection) exitDirection = 0.95f;
		distanceToExit = portalOpposite.transform.position - transform.position;
		locationAfterTeleport = distanceToExit * exitDirection;
	}

	void OnTriggerEnter(Collider c)
	{
		GameObject target = c.gameObject;
		if (target.tag == "Enemy"){
		    NavMeshAgent navAgent = target.GetComponent<NavMeshAgent>();
			navAgent.Warp(c.transform.position + (locationAfterTeleport * 1.05f));
		}
		if (target.tag == "Player")
		{
			if(reverseExitDirection) Camera.main.transform.localRotation = Quaternion.Inverse(Camera.main.transform.rotation);
			Movement.RotatePlayer(c.gameObject, Camera.main);
			AudioManager.PlayPortalAudio();

			c.GetComponent<CharacterController>().enabled = false;
			c.transform.position += locationAfterTeleport;
			c.GetComponent<CharacterController>().enabled = true;
		}
		else if (target.tag == "Projectile")
		{
			if (target.name.Contains("Bullet"))
			{
				c.GetComponent<BulletMono>().enabled = false;
				c.transform.position += locationAfterTeleport;
				c.GetComponent<BulletMono>().enabled = true;
			}
		}
	}
}
