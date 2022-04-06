using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class LevelTransition : MonoBehaviour
{

	public int transitionTo = 0;
	public static int[] Levels;
	private static int progression = 0;

	public static int currentLevel = 1;
	public static int maxLevel = 5;

	private Transform text;
	private Transform player;

	//On  game started (Pressing start on the title screen)
	public static void init()
	{
		// maxLevel = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings-1;
		WaveManager.maxLevelNumber = maxLevel;
		Levels = new int[3];
		// [2, 3, 4]
		//for (int i = 0; i < maxLevel; i++)
		//{
		//	Levels[i] = i + 2;
		//}
		progression = 0;
		reshuffle();
	}

	//One level enter
	void Awake()
	{
		if (progression != 2) transitionTo = (Levels[progression+=1]) ;
		text = transform.GetChild(0);
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		Movement.speed = 12f;
		gameObject.SetActive(false);
	}

	//On level complete
	public void LevelComplete(WaveManager Spawner)
	{
		gameObject.SetActive(true);
		Movement.speed *= 2f;
		//Levels[progression]
		currentLevel = transitionTo;
		Spawner.enabled = false;
        WaveManager.currentLevelNumber++;

	}

	//Randomize level order (except the last level is always last)
	// we are assigning values to levels here 
	static void reshuffle()
	{
		// the levels we want to shuffle are 2, 3, 4
		// the last level will always be 5 

		int index = 0;

		while (index < 2)
		{
			int num = Random.Range(2, 5);
			if (!Levels.Contains(num))
			{
				Levels[index] = num;
				index++;
				//Debug.Log(num);
			}
		}

		Levels[2] = 5;

		//for (int t = 0; t < Levels.Length; t++)
		//{
		//	Debug.Log(Levels[t]);
		//}
		currentLevel = Levels[0];
		//Levels[progression]
		SceneManager.LoadScene(currentLevel, LoadSceneMode.Single);
	}

	//Exit level
	void OnTriggerEnter(Collider c)
	{
		if (SceneManager.GetActiveScene().buildIndex == 1) {
			transitionTo = 0;
		}
		Stats statsComponent = c.GetComponent<Stats>();
		if (statsComponent)
		{
			if (statsComponent.owner == EntityType.PLAYER)
			{
				// update the hp before we load the scene 
				ObjectPool.Clear();
				//SceneManager.LoadScene(transitionTo, LoadSceneMode.Single);
				CompassUI.reset();
				SceneManager.LoadScene(transitionTo, LoadSceneMode.Single);
				//Debug.Log(progression);
			}
		}
	}

	void Update()
    {
		text.LookAt(player.position);
	}
}
