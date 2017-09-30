using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
	public float maxSpeed;
	public float maxForce;
	public float forceScale;

	Rigidbody2D rigidBody;
	Vector2 dir;

	private void Awake()
	{
		rigidBody = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		var velocity = rigidBody.velocity;
		var newVelocity = dir * maxSpeed;
		var force = (newVelocity - velocity) * forceScale;
		force = Vector2.ClampMagnitude(force, maxForce);

		rigidBody.AddForce(force);
	}

	public void SetDirection(Vector2 dir)
	{
		this.dir = dir.normalized;
	}

	public float GetVelocity()
	{
		return rigidBody.velocity.magnitude;
	}
}
