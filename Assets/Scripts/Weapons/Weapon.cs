using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//To be attached to Player or Enemy Objects
public class Weapon
{
    Dictionary<string, bool> _firingBehavior = new Dictionary<string, bool>()
    {
        { "singleShot", true }, // singleShot == !repeater
        { "shotgun", false },
        { "repeater", false }
    };

    IHitBehaviour _hitBehaviour;
    float _fireRate; // shots per second
    int _magazineSize;
    int _currentMagazine;
    double lastShot = 0;

    public Weapon(IHitBehaviour hitBehaviour, float fireRate = 0.1f, int magSize = 1)
    {
        _hitBehaviour = hitBehaviour;
        _fireRate = fireRate; 
        _magazineSize = magSize;
        _currentMagazine = magSize;
    }

    public void Attack(Vector3 position, Vector3 direction)
    {
        if (Time.time - lastShot > _fireRate)
        {
            if (_currentMagazine > 0)
            {
                _hitBehaviour.startBehaviour(position, direction);
                // _currentMagazine -= 1; // Comment out for infinite ammo
                lastShot = Time.time;
            }
            else
            {
                Debug.Log("Out of ammo");
            }
        }
    }

    public void Reload()
    {
        _currentMagazine = _magazineSize;
        Debug.Log("Reloaded");
    }
}