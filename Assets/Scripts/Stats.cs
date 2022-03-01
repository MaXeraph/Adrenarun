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
        set 
        {
            _currentHealth = value;
            if (value <= 0 && owner == EntityType.ENEMY) { 
                Destroy(gameObject); UIManager.enemiesLeft -= 1;
                int rand = Random.Range(0, 10);
                if(rand == 0)
                {
                    GameObject newHealingPill = GameObject.Instantiate(Resources.Load("HealingPill")) as GameObject;
                    Transform pillTransform = newHealingPill.GetComponent<Transform>();
                    pillTransform.position = gameObject.transform.position;
                }
            }
            else if (owner == EntityType.PLAYER) UIManager.Health = currentHealth;
        }
    }
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

        currentHealth = maxHealth;
    }

}
