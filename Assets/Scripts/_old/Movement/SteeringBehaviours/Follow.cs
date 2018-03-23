using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteeringBehaviour))]
public class Follow : MonoBehaviour
{
    public float maxSpeed;
    public float jerkiness;
    public float behindDistance;
    public float forwardDistance;
    public float evadeRadius;

    SteeringBehaviour steering;
    Arrival arrival;
    Evade evade;

    GameObject leader;

    private void Awake()
    {
        steering = GetComponent<SteeringBehaviour>();

        arrival = gameObject.AddComponent<Arrival>();
        arrival.maxSpeed = maxSpeed;
        arrival.jerkiness = jerkiness;

        evade = gameObject.AddComponent<Evade>();
        evade.maxSpeed = maxSpeed;
        evade.jerkiness = jerkiness;
    }

    public void FixedUpdate()
    {
        arrival.enabled = false;
        evade.enabled = false;

        if (!leader)
            return;

        UpdateArrival();
        UpdateEvade();
    }

    void UpdateArrival()
    {
        var leaderPos = (Vector2)leader.transform.position;

        var leaderDir = SteeringBehaviour.GetDirection(leader);

        var arrivalTarget = leaderPos - leaderDir * behindDistance;

        arrival.enabled = true;
        arrival.SetTarget(arrivalTarget);
    }

    void UpdateEvade()
    {
        var leaderPos = (Vector2)leader.transform.position;

        var leaderDir = SteeringBehaviour.GetDirection(leader);

        var evadeTarget = leaderPos + leaderDir * forwardDistance;
        var position = (Vector2)transform.position;

        evade.enabled = (position - evadeTarget).magnitude < evadeRadius;
        evade.SetTarget(evadeTarget);
    }

    public void SetLeader(GameObject leader)
    {
        this.leader = leader;
    }
}
