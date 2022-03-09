using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtilleryMono : BulletMono
{
    public static float vertexHeight = 10f;

    public Vector3 gpPlane;

    public float d;

    public float dir = 1f;
    
    private ArtilleryAttackBehaviour _attackBehaviour;

    public float[] quadratic = new float[3];

    public float incrementX;

    // direction is actually the position of the enemy
    public static GameObject create(ArtilleryAttackBehaviour attackBehaviour, Vector3 position, Vector3 direction)
    {
        GameObject newBullet = Instantiate(Resources.Load("Artillery")) as GameObject;

        ArtilleryMono behaviour = newBullet.GetComponent<ArtilleryMono>();
        
        behaviour._attackBehaviour = attackBehaviour;
        
        newBullet.transform.position = position;

        Vector3 vertex = position + (direction - position) / 2 + new Vector3(0, ArtilleryMono.vertexHeight, 0);

        behaviour.gpPlane = MathModule.determinePlaneNormal(position, direction, vertex);

        behaviour.d = -1 * (behaviour.gpPlane.x * position.x + behaviour.gpPlane.y * position.y + behaviour.gpPlane.z * position.z);
        
        Vector3 positionXY = MathModule.convertToXY(behaviour.gpPlane, position, behaviour.d);
        Vector3 targetXY = MathModule.convertToXY(behaviour.gpPlane, direction, behaviour.d);
        if (targetXY.x < positionXY.x) behaviour.dir = -1f;
        Vector3 vertexXY = MathModule.convertToXY(behaviour.gpPlane, vertex, behaviour.d);

        behaviour.quadratic = MathModule.calculateQuadratic(new Vector3[3] { positionXY, targetXY, vertexXY });
        
        behaviour.incrementX = Mathf.Abs(targetXY.x - positionXY.x)/200; 

        return newBullet;
    }

    void Update()
    {
        Vector3 posXY = MathModule.convertToXY(gpPlane, transform.position, d);
        
        float nextX = posXY.x + incrementX * SpeedManager.bulletSpeedScaling * dir;
        float nextY = quadratic[0]*nextX*nextX + quadratic[1]*nextX + quadratic[2];
        Vector3 newPosXY = new Vector3(nextX, nextY, 0);
        Vector3 newPosition = MathModule.convertToPlane(gpPlane, newPosXY, d);

        transform.position = newPosition;
    }

    void OnTriggerEnter(Collider c)
    {
        _attackBehaviour.onHit(this, c.gameObject);
    }
}