using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
		text = transform.GetChild(0);
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		gameObject.SetActive(false);
	}

	//On level complete
	public void LevelComplete(WaveManager Spawner)
    {
		gameObject.SetActive(true);
		//Levels[progression]
		currentLevel = transitionTo;
		Spawner.enabled = false;
    }

	//Randomize level order (except the last level is always last)
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
		//Levels[progression]
		SceneManager.LoadScene(1, LoadSceneMode.Single);
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

	void Update()
    {
		text.LookAt(player.position);
    }
}
