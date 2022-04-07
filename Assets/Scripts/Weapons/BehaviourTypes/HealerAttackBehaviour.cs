using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerAttackBehaviour : AbstractAttackBehaviour
{
	private static float debuffMultiplier = 2f;
	private IEnumerator debuffDecay;

	public HealerAttackBehaviour(EntityType owner, float damage) : base(owner, damage)
	{
	}

	public override void initiateAttack(Vector3 position, Vector3 direction)
	{
        position -= direction.normalized * 1.1f;
		RaycastHit[] hits = Physics.RaycastAll(position, direction);
		Array.Sort(hits,
			(RaycastHit a, RaycastHit b) => ((a.transform.position - position).magnitude -
			                                (b.transform.position - position).magnitude) > 0 ? 1 : -1);

        AudioSource audio = hits[0].transform.gameObject.GetComponent<AudioSource>();
		for (int i = 0; i < hits.Length; i++)
		{
			RaycastHit hit = hits[i];
			Stats statsComponent = hit.transform.gameObject.GetComponent<Stats>();
			if (statsComponent && statsComponent.owner != _owner)
			{
                if (audio != null && !audio.isPlaying) audio.Play();
				statsComponent.currentHealth -= _damage;
                if (debuffDecay != null) SpeedManager.instance.StopCoroutine(debuffDecay);
                debuffDecay = applyDebuff(statsComponent, this);
				SpeedManager.instance.StartCoroutine(debuffDecay);
                return;
			}
			if (hit.transform.root.gameObject.tag == "PlatformObjects") {
                if (audio != null) audio.Stop();
                return;
            }
		}
	}

	private IEnumerator applyDebuff(Stats target, object source)
	{
		target.damageTakenMultipliers[source] = debuffMultiplier;
		yield return new WaitForSeconds(1f);
		target.damageTakenMultipliers.Remove(source);
		debuffDecay = null;
	}
}