using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPill : MonoBehaviour
{
    private static List<GameObject> currentPills = new List<GameObject>();

    public static GameObject SpawnPill(Vector3 position)
    {
        GameObject newHealingPill = ObjectPool.Create("HealingPill");
        Transform pillTransform = newHealingPill.GetComponent<Transform>();
        pillTransform.position = position;
        currentPills.Add(newHealingPill);
        return newHealingPill;
    }

    public static void DespawnPills()
    {
        foreach (GameObject pill in currentPills)
        {
            ObjectPool.Destroy("HealingPill", pill);
        }
        currentPills.Clear();
    }
    
    void Update(){
        gameObject.transform.rotation *= Quaternion.AngleAxis(-3, Vector3.forward);
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
            ObjectPool.Destroy("HealingPill", gameObject);
        }
    }
}
