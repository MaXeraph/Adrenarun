using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtilleryMono : BulletMono
{
	public static float vertexHeight = 40f;

	public Vector3 gpPlane;

	public float d;

	private ArtilleryAttackBehaviour _attackBehaviour;

	public float[] quadratic = new float[3];

	public float incrementX;

	// direction is actually the position of the enemy
	public static GameObject Create(ArtilleryAttackBehaviour attackBehaviour, Vector3 position, Vector3 direction)
	{
		GameObject newBullet = ObjectPool.Create("Artillery");

		ArtilleryMono behaviour = newBullet.GetComponent<ArtilleryMono>();

		behaviour._attackBehaviour = attackBehaviour;

		newBullet.transform.position = position;

		Vector3 vertex = position + (direction - position) / 2 + new Vector3(0, ArtilleryMono.vertexHeight, 0);

		behaviour.gpPlane = MathModule.determinePlaneNormal(position, direction, vertex);

		behaviour.d = -1 * (behaviour.gpPlane.x * position.x + behaviour.gpPlane.y * position.y + behaviour.gpPlane.z * position.z);

		Vector3 positionXY = MathModule.convertToXY(behaviour.gpPlane, position, behaviour.d);
		Vector3 targetXY = MathModule.convertToXY(behaviour.gpPlane, direction, behaviour.d);
		Vector3 vertexXY = MathModule.convertToXY(behaviour.gpPlane, vertex, behaviour.d);

		behaviour.quadratic = MathModule.calculateQuadratic(new Vector3[3] { positionXY, targetXY, vertexXY });

		behaviour.incrementX = (targetXY.x - positionXY.x) / 300;

		return newBullet;
	}

	public static void Destroy(GameObject artillery)
	{
		ObjectPool.Destroy("Artillery", artillery);
	}

	void Update()
	{
		Vector3 posXY = MathModule.convertToXY(gpPlane, transform.position, d);

		float nextX = posXY.x + incrementX * SpeedManager.bulletSpeedScaling;
		float nextY = quadratic[0] * nextX * nextX + quadratic[1] * nextX + quadratic[2];
		Vector3 newPosXY = new Vector3(nextX, nextY, 0);
		Vector3 newPosition = MathModule.convertToPlane(gpPlane, newPosXY, d);

		transform.position = newPosition;
	}

	void OnTriggerEnter(Collider c)
	{
		_attackBehaviour.onHit(this, c.gameObject);
	}
}