using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeteorMovement : MonoBehaviour
{
    public class Path
    {
        public GameObject spawnPoint;
        public GameObject destinationPoint;

        public float speed;
    }

    Path path;

    Vector3 target;

    private void Update()
    {
        if (path == null)
            return;

        // maybe better to use physics here
        transform.position = Vector3.MoveTowards(transform.position, target, path.speed * Time.deltaTime);

        const float Epsilon = 0.1f;

        if (DistanceToTarget() < Epsilon)
            Destroy(gameObject);
    }

    private float DistanceToTarget()
    {
        return (transform.position - target).magnitude;
    }

    public void SetPath(Path path)
    {
        this.path = path;

        target = path.destinationPoint.transform.position;
    }
}
