using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Dictionary<string, bool> _firingBehavior = new Dictionary<string, bool>()
    {
        { "singleShot", true },
        { "shotgun", false },
        { "repeater", false }
    };

    private float _fireRate; // shots per second
    private int _magazineSize;

    private int _currentMagazine;

    private IHitBehaviour _hitBehaviour;
    private Dictionary<string, bool> _hitTypeModifiers = new Dictionary<string, bool>()
    {
        { "exploding", false },
        { "pierceThrough", false }
    };
    private Dictionary<string, float> _hitStatsModifiers = new Dictionary<string, float>()
    {
        { "damage", 10f }
    };

    void Start(){
        _hitBehaviour = new BulletBehaviour();
        _fireRate = 1f;
        _magazineSize = 100;
        _currentMagazine = 100;
    }

    void Fire()
    {
        if (_currentMagazine > 0)
        {
            _hitBehaviour.startBehaviour();
        }
        else
        {
            Debug.Log("Out of ammo");
        }
    }

    void Reload()
    {
        _currentMagazine = _magazineSize;
    }
}
