using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    GameObject cameraView;
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
        cameraView = transform.GetChild(0).gameObject;
        _hitBehaviour = new BulletBehaviour();
        _fireRate = 1f;
        _magazineSize = 10;
        _currentMagazine = 10;
    }

    public void Attack()
    {
        if (_currentMagazine > 0)
        {
            _hitBehaviour.startBehaviour(Camera.main.transform.forward + Camera.main.transform.position, cameraView.transform.forward, 25, 20f);
            _currentMagazine -= 1;
            Debug.Log("Current magazine: " + _currentMagazine);
        }
        else
        {
            Debug.Log("Out of ammo");
        }
    }

    public void Reload()
    {
        _currentMagazine = _magazineSize;
        Debug.Log("Reloaded");
    }
}
