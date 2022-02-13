using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Bullet
{
    int clipSize { get; }
    int fireRate { get; } // shots per second
    int damage { get; }
    
    void onShoot();
    void onHit();
    void onDestroy();
}
