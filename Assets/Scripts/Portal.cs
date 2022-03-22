using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Portal : MonoBehaviour
{
	// Start is called before the first frame update
	public GameObject portalOpposite;
	Vector3 distanceToExit;
	Vector3 locationAfterTeleport;
	void Start()
	{
		distanceToExit = portalOpposite.transform.position - transform.position;
		locationAfterTeleport = distanceToExit * 1.05f;
	}

	void OnTriggerEnter(Collider c)
	{
		GameObject target = c.gameObject;
		if (target.tag == "Enemy"){
			c.transform.position += locationAfterTeleport;
		}
		if (target.tag == "Player")
		{
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
