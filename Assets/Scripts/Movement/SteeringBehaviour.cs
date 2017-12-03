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

    static public Vector2 GetVelocityDirection(GameObject obj)
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

    static public Vector2 GetOrientationDirection(GameObject obj)
    {
        return obj.transform.right;
    }

    static public Vector2 GetVelocityOrOrientationDirection(GameObject obj)
    {
        var dir = GetVelocityDirection(obj);
        if (dir.magnitude < Mathf.Epsilon)
            dir = GetOrientationDirection(obj);

        return dir;
    }

    public Vector2 VelocityDirection
    {
        get
        {
            return Velocity.normalized;
        }
    }

    public Vector2 OrientationDirection
    {
        get
        {
            return transform.right;
        }
    }

    //public Vector2 VelocityOrDirection
    //{
    //    get
    //    {
    //        var val = Velocity;

    //        return val;
    //    }
    //}

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
