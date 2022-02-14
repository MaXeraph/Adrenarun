using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Dictionary<string, int> _firingBehavior = new Dictionary<string, int>()
    {
        { "singleShot", 0 },
        { "shotgun", 0 },
        { "repeater", 0 }
    };
    private IHitBehaviour _bulletType;
    private float _fireRate; // shots per second
    private int _currentClip;
    private Dictionary<string, float> _modifiers = new Dictionary<string, float>()
    {
        { "fireRate", 0 },
        { "maxClip", 0 },
        { "damage", 0 }
    };

    void Start(){
        _bulletType = new BulletPeashooter();
        _fireRate = 1f;
        _currentClip = 100;
    }

    void Fire()
    {
        if (_currentClip > 0)
        {
            _bulletType.startBehaviour();
        }
        else
        {
            Debug.Log("Out of ammo");
        }
    }

    void Reload()
    {
        _currentClip = 100;
    }
}
