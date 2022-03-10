using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletDetection : MonoBehaviour
{
	private static Vector3 planeXZ = new Vector3(0, 1, 0);
	private Collider[] collisions = new Collider[36];
	private Collider self;

	private float radius = 1f;
	private int layerMask = 1 << 2; // Only look at IgnoreRaycast layer for bullets

	// Array of coordinates of bullets relative to the player (i.e. assuming player is 0, 0, 0) projected onto the XZ plane
	public Vector3[] bulletDirections = new Vector3[36];
	public int numDir = 0;
	
	void Start()
	{
		// Can replace with the object this is attached to
		self = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
	}

	void Update()
	{
		// Player capsule has height 2, radius 0.5
		// We want bullets to be detected as if collider with height 3, radius 1
		// point0 is the center of the top hemisphere, point1 is the center of the bottom hemisphere
		Vector3 point0 = self.transform.position;
		point0.y += 2f / 2f - 0.5f;
		Vector3 point1 = self.transform.position;
		point1.y -= 2f / 2f - 0.5f;
		int numCollisions = Physics.OverlapCapsuleNonAlloc(point0, point1, radius, collisions, layerMask);
		Array.Clear(bulletDirections, 0, bulletDirections.Length);
		numDir = 0;
		for (int i = 0; i < numCollisions; i++)
		{
			if (collisions[i].gameObject.tag == "Bullet")
			{
				Transform bullet = collisions[i].gameObject.transform;
				Vector3 bulletDir = bullet.position - self.ClosestPoint(bullet.position);
				Vector3 projectedDir = Vector3.ProjectOnPlane(bulletDir, planeXZ);
				bulletDirections[numDir++] = projectedDir;
			}
		}
	}
}