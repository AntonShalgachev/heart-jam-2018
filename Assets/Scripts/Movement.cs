using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    public float maxSpeed;

    Rigidbody2D rigidBody;
    Vector2 dir;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        var newVelocity = dir * maxSpeed;
        rigidBody.velocity = newVelocity;
    }

    public void SetDirection(Vector2 dir)
    {
        this.dir = dir.normalized;
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