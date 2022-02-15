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
    // _weapon = new Weapon();

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
        Func<Transform, Transform, Vector3> pathfind, 
        Func<Transform, Transform, Vector3> aim, 
        Action<Transform, Vector3> move)
    {
        EnemyBehaviour eb = gameObject.AddComponent<EnemyBehaviour>();
        eb.Initialize(target, pathfind, aim, move);

        return eb;
    }

    // Initialize must be run for the Update loop to run.
    // This is because the mechanisms (Pathfind, Aim, etc.) aren't set until Initialize is called.
    public bool Initialize(GameObject target, 
        Func<Transform, Transform, Vector3> pathfind, 
        Func<Transform, Transform, Vector3> aim,
        Action<Transform, Vector3> move)
    {
        if (_initialized) return false;
        _initialized = true;

        _targetTransform = target.GetComponent<Transform>();
        Pathfind = pathfind;
        GetAimDirection = aim;
        Move = move;
        
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
        
        // Fire.
        if (!_cooldown)
        {
            // _weapon.Fire(aimDirection);
            _cooldown = true;
            StartCoroutine(Cooldown());
        }
    }
}
