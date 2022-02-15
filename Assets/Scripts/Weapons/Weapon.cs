using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    GameObject cameraView;

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

    Dictionary<string, bool> _hitTypeModifiers = new Dictionary<string, bool>()
    {
        { "exploding", false },
        { "pierceThrough", false }
    };
    Dictionary<string, float> _hitStatsModifiers = new Dictionary<string, float>()
    {
        { "damage", 10f },
        { "bulletSpeed", 20f }
    };

    void Start()
    {
        cameraView = transform.GetChild(0).gameObject;

        _hitBehaviour = new BulletBehaviour();
        _fireRate = 0.1f; 
        _magazineSize = 10;
        _currentMagazine = _magazineSize;
    }

    public void Attack()
    {
        if (Time.time - lastShot > _fireRate)
        {
            if (_currentMagazine > 0)
            {
                Vector3 bulletPosition = Camera.main.transform.forward + Camera.main.transform.position;
                Vector3 bulletDirection = Camera.main.transform.forward;
                _hitBehaviour.startBehaviour(bulletPosition, bulletDirection, _hitStatsModifiers);

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
