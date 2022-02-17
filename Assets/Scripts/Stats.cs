using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// To be attached to Player Object
public class Stats : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    public bool isPlayer;

    public Stats(float maxHealth = 100f)
    {
        this.maxHealth = maxHealth;
    }

    void Start(){
        isPlayer = gameObject.tag == "Player";
        // TODO: we need to refactor Stats.cs and decide to 
        //       add the component to the prefab, or through a script.
        maxHealth = 100f;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        SpeedManager.updateSpeeds(currentHealth / maxHealth);
    }

    public void takeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
