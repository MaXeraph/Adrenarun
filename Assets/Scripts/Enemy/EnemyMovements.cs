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
        Animation anim = gameObject.transform.GetChild(0).GetComponent<Animation>();
        NavMeshAgent navAgent = gameObject.GetComponent<NavMeshAgent>();
        float playerDistance = Vector3.Distance(position, playerPosition);
        if (playerDistance > 20) {
            navAgent.SetDestination(playerPosition);
            anim.Play("grenad walk");
        }
        if (playerDistance > 16) anim.Play("grenad walk");
        else anim.Stop();

    }

    public static void TurretMovement(GameObject gameObject, Vector3 playerPosition) {

    }
    public static void TurretSetup(Vector3 transform) {
        
    }

    public static void RangedSetup(Vector3 transform) {

    }

    public static void RangedMovement(GameObject gameObject, Vector3 playerPosition) {
        Vector3 position = gameObject.transform.position;
        Animation anim = gameObject.transform.GetChild(0).GetComponent<Animation>();
        NavMeshAgent navAgent = gameObject.GetComponent<NavMeshAgent>();
        float playerDistance = Vector3.Distance(position, playerPosition);
        if (playerDistance > 15) {
            navAgent.SetDestination(playerPosition);
        }
        if (playerDistance > 10) anim.Play("mech ");
        else anim.Stop();
    }

}
