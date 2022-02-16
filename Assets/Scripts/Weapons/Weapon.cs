using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//To be attached to Player or Enemy Objects
public class Weapon : MonoBehaviour
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

    void Start()
    {
        _hitBehaviour = new BulletBehaviour(); //TODO: make this to be passable instead of a default
        _fireRate = 0.1f; 
        _magazineSize = 10;
        _currentMagazine = _magazineSize;
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
