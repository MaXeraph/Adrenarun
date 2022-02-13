using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Bullet
{
    int clipSize { get; }
    float fireRate { get; } // shots per second
    float damage { get; }
    
    void onShoot();
    void onHit();
    void onDestroy();
}
