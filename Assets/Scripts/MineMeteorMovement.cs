using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineMeteorMovement : MonoBehaviour
{
    public class Path
    {
        public GameObject spawnPoint;
        public OccupiablePoint holdPoint;
        public GameObject destinationPoint;

        public float speed;
        public float holdDuration;
    }

    private enum State
    {
        MovingIn,
        Holding,
        MovingOut,
    }

    Path path;

    Vector3 target;
    float delay;
    State state;

    private void Update()
    {
        if (path == null)
            return;

        delay -= Time.deltaTime;

        if (delay < 0.0f)
        {
            // adds some sort of easing
            var speed = path.speed * DistanceToTarget();

            var newPos = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
            transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);

            const float Epsilon = 0.1f;

            if (DistanceToTarget() < Epsilon)
                nextState();
        }
    }

    private float DistanceToTarget()
    {
        return ((Vector2)transform.position - (Vector2)target).magnitude;
    }

    public void SetPath(Path path)
    {
        this.path = path;

        path.holdPoint.IsOccupied = true;

        target = path.holdPoint.transform.position;
        state = State.MovingIn;
    }

    private void nextState()
    {
        switch(state)
        {
            case State.MovingIn:
                state = State.Holding;
                target = path.holdPoint.transform.position;
                delay = path.holdDuration;
                break;
            case State.Holding:
                state = State.MovingOut;
                target = path.destinationPoint.transform.position;
                delay = 0.0f;
                path.holdPoint.IsOccupied = false;
                break;
            case State.MovingOut:
                Destroy(gameObject);
                break;
        }
    }

    public void Abort()
    {
        state = State.Holding;
        delay = 0.0f;
    }
}
