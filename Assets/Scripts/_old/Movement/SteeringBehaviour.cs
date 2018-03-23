using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SteeringBehaviour : MonoBehaviour
{
    public float maxForce;

    Rigidbody2D rigidBody;
    Vector2 force;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        ResetForce();
    }

    private void FixedUpdate()
    {
        force = Vector2.ClampMagnitude(force, maxForce);
        rigidBody.AddForce(force);

        ResetForce();
    }

   public void AddForce(Vector2 force)
    {
        this.force += force;
    }

    public void ResetForce()
    {
        force = Vector2.zero;
    }

    static public Vector2 GetDirection(GameObject obj)
    {
        return GetVelocity(obj).normalized;
    }

    static public float GetSpeed(GameObject obj)
    {
        return GetVelocity(obj).magnitude;
    }

    static public Vector2 GetVelocity(GameObject obj)
    {
        return obj.GetComponent<Rigidbody2D>().velocity;
    }

    public Vector2 Direction
    {
        get
        {
            return Velocity.normalized;
        }
    }

    public float Speed
    {
        get
        {
            return Velocity.magnitude;
        }
    }

    public Vector2 Velocity
    {
        get
        {
            return rigidBody.velocity;
        }
    }
}
