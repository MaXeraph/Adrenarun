using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedManager : MonoBehaviour
{
    public static float coreSpeed = 1f; // 1:1 with health percentage
    public static float playerMovementScaling;
    public static float bulletSpeedScaling;
    public static float enemySpawnScaling;

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
        playerMovementScaling = coreSpeed;
        bulletSpeedScaling = coreSpeed;
        enemySpawnScaling = 1 / coreSpeed;
    }

}
