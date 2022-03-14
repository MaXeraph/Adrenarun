using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
	private static Dictionary<string, HashSet<GameObject>> available = new Dictionary<string, HashSet<GameObject>>();
	private static Dictionary<string, HashSet<GameObject>> occupied = new Dictionary<string, HashSet<GameObject>>();
	
	public static GameObject Create(string prefab)
	{
		HashSet<GameObject> availableObjects;
		available.TryGetValue(prefab, out availableObjects);
		if (availableObjects == null)
		{
			available[prefab] = new HashSet<GameObject>();
			occupied[prefab] = new HashSet<GameObject>();
		}

		GameObject provisionedObject = null;
		if (available[prefab].Count == 0)
		{
			provisionedObject = GameObject.Instantiate(Resources.Load(prefab)) as GameObject;
			occupied[prefab].Add(provisionedObject);
		}
		else
		{
			foreach (GameObject g in available[prefab])
			{
				provisionedObject = g;
				break;
			}
			available[prefab].Remove(provisionedObject);
			occupied[prefab].Add(provisionedObject);
		}

		provisionedObject.SetActive(true);
		return provisionedObject;
	}

	public static void Destroy(string prefab, GameObject gameObject)
	{
		HashSet<GameObject> occupiedObjects;
		occupied.TryGetValue(prefab, out occupiedObjects);
		if (occupiedObjects == null) Debug.LogError("ObjectPool: Tried to destroy non-existent prefab type " + prefab);
		if (occupied[prefab].Contains(gameObject))
		{
			occupied[prefab].Remove(gameObject);
			gameObject.SetActive(false);
			available[prefab].Add(gameObject);
		}
	}
}