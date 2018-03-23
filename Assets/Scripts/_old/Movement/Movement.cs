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
    float velocityMultiplier = 1.0f;

	private void Awake()
	{
		rigidBody = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		var velocity = rigidBody.velocity;
		var newVelocity = dir * maxSpeed * velocityMultiplier;
		var force = (newVelocity - velocity) * forceScale;
		force = Vector2.ClampMagnitude(force, maxForce);

		rigidBody.AddForce(force);
	}

	public void SetDirection(Vector2 dir)
	{
		this.dir = dir.normalized;
	}

    public void SetVelocityMultiplier(float val)
    {
        velocityMultiplier = val;
    }

    public float GetSpeed()
    {
        return rigidBody.velocity.magnitude;
    }

    public Vector2 GetVelocity()
    {
        return rigidBody.velocity;
    }

    public Vector2 GetDirection()
	{
		return dir;
	}
}
