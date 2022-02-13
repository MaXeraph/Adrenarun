using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // Start is called before the first frame update


    private int _fireRate; // shots per second
    private int _bulletType; // 0 = normal - uses dictionary
    private int _mods; // 0 = no mods TODO: could look into using bitwise to get unique combinations
    private int _currentClip;

    void Start(){
        _bulletType = 0;
        _mods = 0;
        _fireRate = Globals.Guns.bulletTypes[_bulletType].fireRate;
        _currentClip = Globals.Guns.bulletTypes[_bulletType].clipSize;
    }

    void Fire()
    {
        if (_currentClip > 0)
        {
            Globals.Guns.bulletTypes[_bulletType].onShoot();
            Debug.Log("Firing");
        }
        else
        {
            Debug.Log("Out of ammo");
        }
    }

    void Reload()
    {
        _currentClip = Globals.Guns.bulletTypes[_bulletType].clipSize;
    }
}
