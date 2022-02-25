using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatPowerUp : AbstractPowerUp
{
    public StatPowerUp(float _modifier){
        modifier = _modifier;
    }

    public override Vector3[] applyPowerUp(Vector3[] direction){
        return new Vector3[0];
    }
}
