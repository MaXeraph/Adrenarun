using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedManager : MonoBehaviour
{
    public static float coreSpeed = 1f; // 1:1 with health percentage
    public static float playerMovementScaling;
    public static float bulletSpeedScaling;
    public static float enemySpawnScaling;
    public static float adrenalinModifier = 0f;

    void Start() {
        playerMovementScaling = coreSpeed; //TODO: determine player movement ratio based on speed
        bulletSpeedScaling = coreSpeed; //TODO: determine bullet speed ratio based on speed
        enemySpawnScaling = coreSpeed; //TODO: determine spawn rate based on speed
    }
    public static void updateSpeeds(float healthRatio) {
        coreSpeed = healthRatio;
        updateGameObjectSpeed();
    }

    static void updateGameObjectSpeed(){
        playerMovementScaling = coreSpeed + adrenalin();
        bulletSpeedScaling = coreSpeed;
        enemySpawnScaling = 1 / coreSpeed;
    }

    static float adrenalin(){
        // experimental function for adjusting the game speed
        // the lower hp we are, the more adrenalin does
        //
        // change highScale to help out higher hp values
        // change lowScale to help out lower hp values
        // 
        // example with 1 adrenalin powerup:
        // 10% hp = ~28% game speed equivalent 
        // 20% hp = ~36% 
        // 50% hp = ~60%
        // 80% hp = ~84% 
        if (adrenalinModifier > 0)
        {
            float highScale = 1f + adrenalinModifier;
            float lowScale = 1.3f + (adrenalinModifier * 3f);
            float healthDifference = 100f - (100f * coreSpeed);

            return ((0.1f * (highScale * healthDifference)) * lowScale) / 100f;
        }
        return 0f;

    }

}