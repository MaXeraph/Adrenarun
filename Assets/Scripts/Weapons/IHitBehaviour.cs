using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitBehaviour 
{
    float damage { get; }
    void startBehaviour();
    void onHit();
}
