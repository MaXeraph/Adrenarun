using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class EnemyMovements
{

    public static void GrenadierSetup(Vector3 transform) {

    }
    public static void GrenadierMovement(GameObject gameObject, Vector3 playerPosition) {
        Vector3 position = gameObject.transform.position;
        NavMeshAgent navAgent = gameObject.GetComponent<NavMeshAgent>();
        float playerDistance = Vector3.Distance(position, playerPosition);
        if (playerDistance > 20) {
            navAgent.SetDestination(playerPosition);
        }
    }

    public static void TurretMovement(GameObject gameObject, Vector3 playerPosition) {

    }
    public static void TurretSetup(Vector3 transform) {
        
    }
}
