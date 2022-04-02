using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//To be attached to Player or Enemy Objects
public class Weapon : MonoBehaviour
{
    private bool _initialized = false;
    public List<AbstractFiringPowerUp> firingMods = new List<AbstractFiringPowerUp>();

    public AbstractAttackBehaviour _attackBehaviour;
    public float _fireRate; // shots per second
    public float _reloadSpeed;
    bool _reloading = false;
    public int _magazineSize;
    int _currentMagazine;
    double lastShot = 0;

    public bool Initialize(AbstractAttackBehaviour attackBehaviour, float fireRate = 0.1f, int magSize = 1, float reloadSpeed = 0f)
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
        lastShot = SpeedManager.realTime;

        return true;
    }

    public bool Attack(Vector3 position, Vector3 direction, EntityType entityType)
    {

        if (SpeedManager.realTime - lastShot > _fireRate && !_reloading)
        {
            if (_currentMagazine > 0)
            {
                List<(Vector3, float)> attackDirections = new List<(Vector3, float)>();
                attackDirections.Add((direction, 0f));

                // apply each powerup in poweruplist, creating a growing list of firing directions
                // assumptions: firingMods is sorted (handled in PowerUpManager)
                for (int i = 0; i < firingMods.Count; i++)
                {
                    attackDirections = firingMods[i].applyPowerUp(attackDirections);
                }

                // for each direction, initiate an attack
                for (int j = 0; j < attackDirections.Count; j++)
                {
                    StartCoroutine(ShootAfterDelay(position, attackDirections[j].Item1, attackDirections[j].Item2));
                }
                if (_reloadSpeed > 0) _currentMagazine -= 1; // Comment out for infinite ammo
                if (entityType == EntityType.PLAYER)
                {
                    AudioManager.PlayFireAudio();
                }
                lastShot = SpeedManager.realTime;
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
        if (_attackBehaviour.Owner.ToString() == "PLAYER")
        {
            UIManager.Reloading = true;
            AudioManager.PlayReloadAudio();
        }
        else DOTween.To(() => _currentMagazine, x => _currentMagazine = x, _magazineSize, _reloadSpeed).OnComplete(finishReload);
    }

    public void finishReload()
    {
        _reloading = false;
        _currentMagazine = _magazineSize;
    }

    IEnumerator ShootAfterDelay(Vector3 position, Vector3 direction, float delay)
    {
        yield return new WaitForSeconds(delay);
        _attackBehaviour.initiateAttack(position, direction);
    }
}
