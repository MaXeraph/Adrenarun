using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//To be attached to Player or Enemy Objects
public class Weapon
{
    Dictionary<string, bool> _firingBehavior = new Dictionary<string, bool>()
    {
        { "singleShot", true }, // singleShot == !repeater
        { "shotgun", false },
        { "repeater", false }
    };

    AbstractAttackBehaviour _attackBehaviour;
    float _fireRate; // shots per second
    public float _reloadSpeed;
    bool _reloading = false;
    public int _magazineSize;
    int _currentMagazine;
    double lastShot = 0;

    public Weapon(AbstractAttackBehaviour attackBehaviour, float fireRate = 0.1f, int magSize = 1, float reloadSpeed = 1f)
    {
        _attackBehaviour = attackBehaviour;
        _fireRate = fireRate; 
        _magazineSize = magSize;
        _currentMagazine = magSize;
        _reloadSpeed = reloadSpeed;
    }

    public bool Attack(Vector3 position, Vector3 direction)
    {
        if (Time.time - lastShot > _fireRate && !_reloading)
        {
            if (_currentMagazine > 0)
            {
                _attackBehaviour.initiateAttack(position, direction);
                _currentMagazine -= 1; // Comment out for infinite ammo
                lastShot = Time.time;
                return true;
            }
            else
            {
                //Debug.Log("Out of ammo");
                Reload();
                return false;
            }
        }
        return false;
    }

    public void Reload()
    {
        _reloading = true;
        DOTween.To(() => _currentMagazine, x => _currentMagazine = x, _magazineSize, _reloadSpeed).OnComplete(finishReload);
        //_currentMagazine = _magazineSize;
    }

    void finishReload()
    {
        _reloading = false;
    }

}
