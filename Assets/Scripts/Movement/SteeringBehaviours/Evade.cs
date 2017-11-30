using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteeringBehaviour))]
public class Evade : MonoBehaviour
{
    public float maxSpeed;
    public float jerkiness;

    SteeringBehaviour steering;
    Vector2 target;

    static float SLOWING_RADIUS = 4.0f;

    private void Awake()
    {
        steering = GetComponent<SteeringBehaviour>();
    }

    public void FixedUpdate()
    {
        var targetVel = (Vector2)transform.position - target;

        var distance = targetVel.magnitude;

        targetVel = targetVel.normalized * maxSpeed;
        Vector2 force = (targetVel - steering.Velocity) * jerkiness;

        steering.AddForce(force);
    }

    public void SetTarget(Vector2 target)
    {
        this.target = target;
    }
}
