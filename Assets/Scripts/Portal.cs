using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Portal : MonoBehaviour
{
	// Start is called before the first frame update
	public GameObject portalOpposite;
	Vector3 distanceToExit;
	void Start()
	{
		distanceToExit = portalOpposite.transform.position - transform.position;
	}

	void OnTriggerEnter(Collider c)
	{
		Debug.Log(c.gameObject.tag);
		Debug.Log("distance: " + distanceToExit);
		if (c.gameObject.tag == "Player")
		{
			c.GetComponent<CharacterController>().enabled = false;
			c.transform.position = c.transform.position + distanceToExit * 1.05f;
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
