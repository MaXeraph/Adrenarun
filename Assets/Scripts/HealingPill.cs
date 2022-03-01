using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPill : MonoBehaviour
{
    void Update(){
        gameObject.transform.rotation *= Quaternion.AngleAxis(-5, Vector3.forward);
    }
    
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Stats statComponent = other.gameObject.GetComponent<Stats>();
            statComponent.currentHealth += 15;
            if(statComponent.currentHealth >= statComponent.maxHealth)
            {
                statComponent.currentHealth = statComponent.maxHealth;
            }
            Destroy(gameObject);
        }
    }
}
