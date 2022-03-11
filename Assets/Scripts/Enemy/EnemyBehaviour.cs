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
    private Action<GameObject, Vector3> NavAgentMove;
    // Method to determine aiming direction from Transform to Transform, returns Vector3, the direction to fire in.
    private Func<Transform, Transform, Vector3> GetAimDirection;
    
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
        NavAgentMove = info.navAgentMove;
        GetAimDirection = info.aim;
        info.navAgentSetup(new Vector3(0, 0, 0));
        
        return true;
    }
    
    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update()
    {
        if (!_initialized) return;

        Animation anim = gameObject.transform.GetChild(0).GetComponent<Animation>();
        // Look. Determine look direction.
        _direction = (_targetTransform.position - transform.position).normalized;
        //_direction.y = 0;
        _lookRotation = Quaternion.LookRotation(_direction);
        transform.rotation = _lookRotation;
        
        NavAgentMove(this.gameObject, _targetTransform.position);
        
        // Aim. Determine firing direction.
        Vector3 aimDirection = GetAimDirection(transform, _targetTransform);

        // TODO: spawn bullet outside of model
        if (_weapon.Attack(transform.position, aimDirection, EntityType.ENEMY))
        {
            foreach (AnimationState state in anim)
            {
                if (state.name == "shoot") anim.Play("shoot");
            }
        }
    }
}
