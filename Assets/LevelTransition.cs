using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{

	private int transitionTo = 0;
	public static int[] Levels;
	private static int progression = 0;

	public static int currentLevel = 1;
	public static int maxLevel = 5;

	//On  game started
	public static void init()
    {
		//WaveManager.maxLevelNumber = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings-1;
		maxLevel = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings-1;
		WaveManager.maxLevelNumber = maxLevel;
		Levels = new int[maxLevel];
		for (int i = 0; i < maxLevel; i++)
        {
			Levels[i] = i+1;
        }
		progression = 0;
		reshuffle();
	}

	//One level enter
	void Awake()
    {
		if (transitionTo == 0) transitionTo = (Levels[progression+=1]);
		Debug.Log(progression);
		Debug.Log(transitionTo);
		gameObject.SetActive(false);
	}

	//On level complete
	public void LevelComplete(WaveManager Spawner)
    {
		gameObject.SetActive(true);
		currentLevel = Levels[progression];
		Spawner.enabled = false;
    }

	//Randomize level order (except level 6 is always last)
	static void reshuffle()
	{
		int[] _level = new int[(maxLevel - 2)];
		for (int i = 0; i < _level.Length; i++)
		{
			_level[i] = i+1;
		}
		for (int t = 0; t < _level.Length; t++)
		{
			int tmp = _level[t];
			int r = Random.Range(t, _level.Length);
			_level[t] = _level[r];
			_level[r] = tmp;
		}
		for (int t = 0; t < _level.Length; t++)
		{
			Levels[t] = _level[t];
		}
		for (int t = 0; t < Levels.Length; t++)
		{
			Debug.Log(Levels[t]);
		}
		currentLevel = Levels[0];
		SceneManager.LoadScene(Levels[progression], LoadSceneMode.Single);
	}

	//Exit level
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
				SceneManager.LoadScene(transitionTo, LoadSceneMode.Single);
			}
		}
	}
}
