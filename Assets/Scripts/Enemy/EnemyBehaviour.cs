using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Generic EnemyBehaviour. Should be initialized after creation via Initialize(), otherwise is not usable.
 */
public class EnemyBehaviour : MonoBehaviour
{
    private bool _initialized = false;
    // Transform of the enemy's target.
    private Transform _targetTransform;
    private Quaternion _lookRotation;
    private Vector3 _direction;
    private bool _cooldown = false;
    private float _cooldownDelay = 1f;
    private Weapon _weapon;

    // Method to pathfind from Transform to Transform, returns Vector3, the direction to move in next.
    private Func<Transform, Transform, Vector3> Pathfind;
    // Method to determine aiming direction from Transform to Transform, returns Vector3, the direction to fire in.
    private Func<Transform, Transform, Vector3> GetAimDirection;
    // Method to move Transform in the direction of Vector3.
    private Action<Transform, Vector3> Move;
    
    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(_cooldownDelay);
        _cooldown = false;
    }

    public static EnemyBehaviour AddToGameObject(
        GameObject gameObject, 
        GameObject target, 
        EnemyInfo info,
        Weapon weapon)
    {
        EnemyBehaviour eb = gameObject.AddComponent<EnemyBehaviour>();
        eb.Initialize(target, info, weapon);

        return eb;
    }

    // Initialize must be run for the Update loop to run.
    // This is because the mechanisms (Pathfind, Aim, etc.) aren't set until Initialize is called.
    public bool Initialize(GameObject target, 
        EnemyInfo info,
        Weapon weapon)
    {
        if (_initialized) return false;
        _initialized = true;

        _weapon = weapon;
        _targetTransform = target.GetComponent<Transform>();
        Pathfind = info.pathfind;
        GetAimDirection = info.aim;
        Move = info.move;
        
        return true;
    }
    
    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update()
    {
        if (!_initialized) return;
        
        // Look. Determine look direction.
        _direction = (_targetTransform.position - transform.position).normalized;
        _lookRotation = Quaternion.LookRotation(_direction);
        transform.rotation = _lookRotation;
        
        // Pathfind. Determine direction to move in.
        Vector3 moveDirection = Pathfind(transform, _targetTransform);
        
        // Move. Ensure movement is valid, then move.
        Move(transform, moveDirection);
        
        // Aim. Determine firing direction.
        Vector3 aimDirection = GetAimDirection(transform, _targetTransform);

        // TODO: spawn bullet outside of model
        _weapon.Attack(transform.position + aimDirection*2, aimDirection);
    }
}
