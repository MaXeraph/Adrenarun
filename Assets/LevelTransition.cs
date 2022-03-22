using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{

	public int transitionTo = 0;

	void Awake()
    {
		if(transitionTo == 0) transitionTo = (SceneManager.GetActiveScene().buildIndex + 1);
		gameObject.SetActive(false);
	}

	public void init(WaveManager Spawner)
    {
		gameObject.SetActive(true);
		Spawner.enabled = false;
    }

	void OnTriggerEnter(Collider c)
	{
		Stats statsComponent = c.GetComponent<Stats>();
		if (statsComponent)
		{
			if (statsComponent.owner == EntityType.PLAYER)
			{
				// update the hp before we load the scene 
				SceneManager.LoadScene(transitionTo, LoadSceneMode.Single);
				CompassUI.reset();
			}
		}
	}
}
