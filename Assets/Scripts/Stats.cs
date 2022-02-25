using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: refactor away from MonoBehaviour
public class Stats : MonoBehaviour
{
    private float _currentHealth;

    public float currentHealth
    {
        get => _currentHealth;
        set {
            _currentHealth = value;
            if (value <= 0) Destroy(gameObject);
        }
    }
    public float maxHealth;
    public EntityType owner;

    public Stats(float maxHealth = 100f)
    {
        this.maxHealth = maxHealth;
    }

    void Start()
    {
        // TODO: refactor for more dynamic assignment
        if (gameObject.tag == "Player") owner = EntityType.PLAYER;
        else owner = EntityType.ENEMY;
        maxHealth = 100f;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if(owner == EntityType.PLAYER) {
            SpeedManager.updateSpeeds(currentHealth / maxHealth);
        }
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
