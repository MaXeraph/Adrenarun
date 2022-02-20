using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpContainer
{
    public PowerUpEffect effect;

    // Set the effect of the powerup on instantiation of the container
    public PowerUpContainer(){
        setEffect();
    }

    // Randomly generate an effect that corresponds to a powerup
    public void setEffect(){
        Debug.Log("setEffect called");
        effect = (PowerUpEffect)Random.Range(1, 5);
    } 
}
