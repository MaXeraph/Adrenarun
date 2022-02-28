using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//To be attached to Player or Enemy Objects
public class Weapon : MonoBehaviour
{
    private bool _initialized = false;
    Dictionary<PowerUpType, bool> _firingBehavior = new Dictionary<PowerUpType, bool>()
    {
        { PowerUpType.NONE, true }, // singleShot == !repeater
        { PowerUpType.SHOTGUN, false},
        { PowerUpType.REPEATER, false}
    };
    public Dictionary<PowerUpType, bool> firingBehavior
    {
        get => _firingBehavior;
        set {
            _firingBehavior = value;
        }
    }

    public AbstractAttackBehaviour _attackBehaviour;
    public float _fireRate; // shots per second
    public float _reloadSpeed;
    bool _reloading = false;
    public int _magazineSize;
    int _currentMagazine;
    double lastShot = 0;

    public bool Initialize(AbstractAttackBehaviour attackBehaviour, float fireRate = 0.1f, int magSize = 1, float reloadSpeed = 1f)
    {
        if (_initialized) return false;
        _initialized = true;
        _attackBehaviour = attackBehaviour;
        _fireRate = fireRate;
        _magazineSize = magSize;
        _currentMagazine = magSize;
        _reloadSpeed = reloadSpeed;

        if (_attackBehaviour.Owner.ToString() == "PLAYER")
        {
            UIManager.weapon = this;
        }

        return true;
    }

    public bool Attack(Vector3 position, Vector3 direction)
    {

        if (Time.time - lastShot > _fireRate && !_reloading)
        {
            if (_currentMagazine > 0)
            {                
                List<(Vector3, float)> attackDirections = new List<(Vector3, float)>();
                attackDirections.Add((direction, 0f));
                
                if (!firingBehavior[PowerUpType.NONE])
                {                
                    // apply each powerup in poweruplist, creating a growing list of firing directions
                    // assumptions: firingPowerUps in PowerUpManager is sorted (handled in PowerUpManager)
                    List<AbstractFiringPowerUp> firingPowerUps = GameObject.FindWithTag("Player").GetComponent<PowerUpManager>().firingPowerUps; 
                    for (int i = 0; i < firingPowerUps.Count; i++)
                    {
                        attackDirections = firingPowerUps[i].applyPowerUp(attackDirections);
                    }
                }

                // for each direction, initiate an attack
                for (int j = 0; j < attackDirections.Count; j++)
                {
                    StartCoroutine(ShootAfterDelay(position, attackDirections[j].Item1, attackDirections[j].Item2));
                }
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

    IEnumerator ShootAfterDelay(Vector3 position, Vector3 direction, float delay)
    {
        yield return new WaitForSeconds(delay);
        _attackBehaviour.initiateAttack(position, direction);
    }
}
