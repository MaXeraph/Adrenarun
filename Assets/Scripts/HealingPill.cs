using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPill : MonoBehaviour
{
	private static List<GameObject> currentPills = new List<GameObject>();
	private int healingPillCap = 2;

	public static GameObject SpawnPill(Vector3 position)
	{
		GameObject newHealingPill = ObjectPool.Create("HealingPill");
		Transform pillTransform = newHealingPill.GetComponent<Transform>();
		position.y = 1; // Do this so healing pills dont spawn in air for flying enemies
		pillTransform.position = position;
		currentPills.Add(newHealingPill);
		return newHealingPill;
	}

	public static void DespawnPills()
	{
		foreach (GameObject pill in currentPills)
		{
			ObjectPool.Destroy("HealingPill", pill);
		}
		currentPills.Clear();
	}

	void Update()
	{
		gameObject.transform.rotation *= Quaternion.AngleAxis(-3, Vector3.forward);
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			PlayerCentral playerCentral = other.gameObject.GetComponent<PlayerCentral>();
			if (playerCentral.healingPills < healingPillCap){
				AudioManager.PlayPickUpItemAudio();
				playerCentral.healingPills += 1;
				ObjectPool.Destroy("HealingPill", gameObject);
			}
		}
	}
}
