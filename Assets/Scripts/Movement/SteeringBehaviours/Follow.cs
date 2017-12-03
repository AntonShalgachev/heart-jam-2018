using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteeringBehaviour))]
public class Follow : MonoBehaviour
{
    public float maxSpeed;
    public float maxEvadeSpeed;
    public float jerkiness;
    public float evadeJerkiness;
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
        evade.maxSpeed = maxEvadeSpeed;
        evade.jerkiness = evadeJerkiness;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(EvadeTarget, evadeRadius);
        Gizmos.DrawLine(LeaderPos, ArrivalTarget);
    }

    Vector2 LeaderPos
    {
        get
        {
            return leader.transform.position;
        }
    }

    Vector2 LeaderDir
    {
        get
        {
            return SteeringBehaviour.GetVelocityOrOrientationDirection(leader);
        }
    }

    Vector2 ArrivalTarget
    {
        get
        {
            return LeaderPos - LeaderDir * behindDistance;
        }
    }

    Vector2 EvadeTarget
    {
        get
        {
            return LeaderPos + LeaderDir * forwardDistance;
        }
    }

    Vector2 Position
    {
        get
        {
            return transform.position;
        }
    }

    void UpdateArrival()
    {
        arrival.enabled = true;
        arrival.SetTarget(ArrivalTarget);
    }

    void UpdateEvade()
    {
        evade.enabled = (Position - EvadeTarget).magnitude < evadeRadius;
        evade.SetTarget(LeaderPos);
    }

    public void SetLeader(GameObject leader)
    {
        this.leader = leader;
    }
}
