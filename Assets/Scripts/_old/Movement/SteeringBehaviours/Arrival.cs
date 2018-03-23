using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteeringBehaviour))]
public class Arrival : MonoBehaviour
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
        var targetVel = target - (Vector2)transform.position;

        var distance = targetVel.magnitude;
        var speedMultiplier = Mathf.Clamp(distance / SLOWING_RADIUS, 0.0f, 1.0f);

        targetVel = targetVel.normalized * maxSpeed * speedMultiplier;
        Vector2 force = (targetVel - steering.Velocity) * jerkiness;

        steering.AddForce(force);
    }

    public void SetTarget(Vector2 target)
    {
        this.target = target;
    }
}
