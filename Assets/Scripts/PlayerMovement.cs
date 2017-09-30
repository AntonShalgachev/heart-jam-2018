using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float maxSpeed;
	public float maxForce;
	public float forceScale;

	Rigidbody2D rigidBody;

	private void Start()
	{
		rigidBody = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		var dir = GetCurrentDirection();
		var velocity = rigidBody.velocity;
		var newVelocity = dir * maxSpeed;
		var force = (newVelocity - velocity) * forceScale;
		force = Vector2.ClampMagnitude(force, maxForce);

		rigidBody.AddForce(force);
	}

	Vector2 GetCurrentDirection()
	{
		Vector2 dir = Vector2.zero;

		if (Input.GetKey(KeyCode.W))
			dir += new Vector2(0.0f, 1.0f);
		if (Input.GetKey(KeyCode.S))
			dir += new Vector2(0.0f, -1.0f);
		if (Input.GetKey(KeyCode.A))
			dir += new Vector2(-1.0f, 0.0f);
		if (Input.GetKey(KeyCode.D))
			dir += new Vector2(1.0f, 0.0f);

		dir.Normalize();
		return dir;
	}
}
