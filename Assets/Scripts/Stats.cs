using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: refactor away from MonoBehaviour
public class Stats : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    public EntityType owner;
    bool dead = false;

    public Stats(float maxHealth = 100f)
    {
        this.maxHealth = maxHealth;
    }

    void Start()
    {
        // TODO: refactor for more dynamic assignment
        if (gameObject.tag == "Player")
        {
            owner = EntityType.PLAYER;
            UIManager.MaxHealth = maxHealth;
        }
        else owner = EntityType.ENEMY;

        setHealth(maxHealth);
    }

    void setHealth(float health)
    {
        currentHealth = Mathf.Clamp(health, 0, maxHealth);
        if (owner == EntityType.PLAYER)
        {
            UIManager.Health = currentHealth;
            SpeedManager.updateSpeeds(currentHealth / maxHealth);
        }
    }

    void setMaxHealth(float health)
    {
        maxHealth = health;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (owner == EntityType.PLAYER)
        {
            UIManager.MaxHealth = maxHealth;
            SpeedManager.updateSpeeds(currentHealth / maxHealth);
        }
    }

    public void heal(float healthGained)
    {
        setHealth(currentHealth + healthGained);
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void takeDamage(float damage)
    {
        setHealth(currentHealth - damage);
        if (currentHealth <= 0)
        {
            if (gameObject.tag == "Player" && !dead)
            {
                dead = true;
                deathUI.reveal(deathUI.instance);
            }
        else Destroy(gameObject);
        }
    }
}
