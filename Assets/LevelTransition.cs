using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{

	public string transitionTo = "Level 2";


	void OnTriggerEnter(Collider c)
	{
		Debug.Log(c);
		Stats statsComponent = c.GetComponent<Stats>();
		if (statsComponent)
		{
			if (statsComponent.owner == EntityType.PLAYER) SceneManager.LoadScene(transitionTo, LoadSceneMode.Single);
		}
	}
}
