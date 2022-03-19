using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
	private static Dictionary<string, HashSet<GameObject>> available = new Dictionary<string, HashSet<GameObject>>();
	private static Dictionary<string, HashSet<GameObject>> occupied = new Dictionary<string, HashSet<GameObject>>();

	// Callbacks to execute when an pooled object is 'destroyed'
	private static Dictionary<GameObject, HashSet<Action<GameObject>>> subscriptions =
		new Dictionary<GameObject, HashSet<Action<GameObject>>>();
	
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
			HashSet<Action<GameObject>> callbacks;
			subscriptions.TryGetValue(gameObject, out callbacks);
			if (callbacks != null)
			{
				foreach (Action<GameObject> callback in callbacks)
				{
					callback(gameObject);
				}
			}
			available[prefab].Add(gameObject);
		}
	}

	// Returns function to unsubscribe
	public static Action Subscribe(GameObject gameObject, Action<GameObject> callback)
	{
		HashSet<Action<GameObject>> callbacks;
		subscriptions.TryGetValue(gameObject, out callbacks);
		if (callbacks == null) subscriptions[gameObject] = new HashSet<Action<GameObject>>();
		subscriptions[gameObject].Add(callback);
		return () => { subscriptions[gameObject].Remove(callback); };
	}
}