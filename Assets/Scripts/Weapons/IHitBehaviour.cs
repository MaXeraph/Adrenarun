using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitBehaviour 
{
    float Damage { get; set;}
    void startBehaviour(Vector3 position, Vector3 direction, int damage, float bulletSpeed);
    void onHit();
}
