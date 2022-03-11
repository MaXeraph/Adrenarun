using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class EnemyMovements
{
    private const float _grenadierBaseSpeed = 6f;
    private const float _rangedBaseSpeed = 8f;

    public static void GrenadierSetup(Vector3 transform) {

    }
    public static void GrenadierMovement(GameObject gameObject, Vector3 playerPosition) {
        GenericMovement(gameObject, playerPosition, _grenadierBaseSpeed);
    }

    public static void TurretMovement(GameObject gameObject, Vector3 playerPosition) {

    }
    public static void TurretSetup(Vector3 transform) {
        
    }

    public static void RangedSetup(Vector3 transform) {

    }

    public static void RangedMovement(GameObject gameObject, Vector3 playerPosition) {
        GenericMovement(gameObject, playerPosition, _rangedBaseSpeed);
    }

    //Grenadier and Ranged movement is pretty much exactly the same, it works the same as before its just now one function
    private static void GenericMovement(GameObject gameObject, Vector3 target, float baseSpeed)
    {
        Vector3 position = gameObject.transform.position;
        Animation anim = gameObject.transform.GetChild(0).GetComponent<Animation>();
        NavMeshAgent navAgent = gameObject.GetComponent<NavMeshAgent>();
        navAgent.speed = baseSpeed * SpeedManager.enemyMovementScaling;

        //Stop calculating distance for Y-axis because it makes it innacurate when the player jumps
        target.y = 0;
        position.y = 0;
        float playerDistance = Vector3.Distance(position, target);

        //Start moving/animating if slightly in front of stopping distance
        if (playerDistance > navAgent.stoppingDistance+5)
        {
            navAgent.SetDestination(target);
            anim.Play(anim.clip.name);
        }
        //Stop animating if in range of stopping distance
        else if (playerDistance <= navAgent.stoppingDistance) anim.Stop();
    }


}
