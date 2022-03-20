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
			if (!_currentCollisions.ContainsKey(gameObject)) ObjectPool.Subscribe(gameObject,
				(obj) => { _currentCollisions[obj] = 0; });
			int curTriggers;
			_currentCollisions.TryGetValue(gameObject, out curTriggers);
			_currentCollisions[gameObject] = curTriggers + 1;
			if (_currentCollisions[gameObject] == 2)
			{
				_currentCollisions[gameObject] = 0;
				BulletMono.Destroy(gameObject);
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		GameObject gameObject = other.gameObject;
		if (gameObject.tag == "Bullet" && _currentCollisions.ContainsKey(gameObject))
		{
			_currentCollisions[gameObject] = _currentCollisions[gameObject] - 1;
		}
	}
}