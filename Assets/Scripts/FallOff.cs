using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallOff : MonoBehaviour
{

	public float fall_damage = 10;

	void OnTriggerEnter(Collider c)
	{
		Stats statsComponent = c.GetComponent<Stats>();
		if (statsComponent)
		{
			if (statsComponent.owner == EntityType.PLAYER)
			{
				PlayerCentral _player = c.GetComponent<PlayerCentral>();
				CharacterController _controller = c.GetComponent<CharacterController>();
				_controller.enabled = false;
				c.transform.position = _player.SpawnLocation;
				_controller.enabled = true;
				AudioManager.PlayImpactAudio();
				statsComponent.currentHealth -= fall_damage;
			}
		}
	}
}
