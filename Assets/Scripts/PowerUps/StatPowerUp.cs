using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatPowerUp : AbstractPowerUp
{
    public float modifier;

    public StatPowerUp(float _modifier){
        modifier = _modifier;
    }

    public void applyPowerUp(ref float attribute){
        attribute += modifier;
    }
}
