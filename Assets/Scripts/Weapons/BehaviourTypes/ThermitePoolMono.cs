using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermitePoolMono : MonoBehaviour
{
    private double _lastFixedUpdate = 0;
    private double _timeSinceTick = 0;
    private double _damageCooldown = 1;
    private float _damage = 20f;
    private int _ticks = 0;
    private ArtilleryAttackBehaviour _attackBehaviour;
    private Dictionary<GameObject, int> _currentCollisions = new Dictionary<GameObject, int>();
    void Start()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.red;
    }
    public void Initialize(ArtilleryAttackBehaviour attackBehaviour)
    {
        _attackBehaviour = attackBehaviour;
    }
    void FixedUpdate()
    {
        _timeSinceTick += (Time.time - _lastFixedUpdate) * SpeedManager.coreSpeed;
        _lastFixedUpdate = Time.time;
        
        // Fire only when X 'real time' has passed since last damage tick.
        if (_timeSinceTick >= _attackBehaviour.thermiteDamageCooldown)
        {
            _timeSinceTick = 0;
            foreach (KeyValuePair<GameObject, int> entry in _currentCollisions)
            {
                // If is colliding with both hitboxes and is the player...
                if (entry.Value == 2 && entry.Key.tag == "Player")
                {
                    Stats statsComponent = entry.Key.GetComponent<Stats>();
                    // ...lower hp.
                    statsComponent.currentHealth -= _attackBehaviour._damage;
                }
            }
            _ticks++;
        }

        if (_ticks == _attackBehaviour.thermiteDurability) Destroy(this);
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject gameObject = other.gameObject;
        Stats statsComponent = gameObject.GetComponent<Stats>();
        if (statsComponent != null)
        {
            int curTriggers;
            _currentCollisions.TryGetValue(gameObject, out curTriggers);
            _currentCollisions[gameObject] = curTriggers + 1;
        }
    }

    void OnTriggerExit(Collider other)
    {
        GameObject gameObject = other.gameObject;
        Stats statsComponent = gameObject.GetComponent<Stats>();
        if (statsComponent != null && _currentCollisions.ContainsKey(gameObject))
        {
            _currentCollisions[gameObject] = _currentCollisions[gameObject] - 1;
            if (_currentCollisions[gameObject] == 0) _currentCollisions.Remove(gameObject);
        }
    }
}