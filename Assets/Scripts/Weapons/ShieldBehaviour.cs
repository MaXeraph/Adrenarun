using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBehaviour : MonoBehaviour
{
	private Dictionary<GameObject, int> _currentCollisions = new Dictionary<GameObject, int>();
	
	void OnTriggerEnter(Collider other)
	{
		GameObject gameObject = other.gameObject;
		if (gameObject.tag == "Bullet" && gameObject.GetComponent<BulletMono>()._attackBehaviour.Owner != EntityType.ENEMY)
		{
			int curTriggers;
			_currentCollisions.TryGetValue(gameObject, out curTriggers);
			_currentCollisions[gameObject] = curTriggers + 1;
			if (_currentCollisions[gameObject] == 2)
			{
				_currentCollisions.Remove(gameObject);
				ObjectPool.Destroy("Bullet", gameObject);
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		GameObject gameObject = other.gameObject;
		if (gameObject.tag == "Bullet" && _currentCollisions.ContainsKey(gameObject))
		{
			_currentCollisions[gameObject] = _currentCollisions[gameObject] - 1;
			if (_currentCollisions[gameObject] == 0) _currentCollisions.Remove(gameObject);
		}
	}
}