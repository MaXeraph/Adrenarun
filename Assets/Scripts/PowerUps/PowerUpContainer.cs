using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpContainer
{
    public string effect;

    // Set the effect of the powerup on instantiation of the container
    public PowerUpContainer(){
        setEffect();
    }

    // Randomly generate a string that corresponds to a powerup
    public void setEffect(){
        Debug.Log("setEffect called");
        int type = Random.Range(0, 4);
        switch(type){
            case 0:
                effect = "damage";
                break;
            case 1:
                effect = "firerate";
                break;
            case 2:
                effect = "reload";
                break;
            case 3:
                effect = "clip";
                break;
            case 4:
                effect = "adrenalin";
                break;
            default:
                break;
        }
    } 
}