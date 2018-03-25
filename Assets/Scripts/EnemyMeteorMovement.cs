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

    public bool Paused { get; set; }

    private void Awake()
    {
        Paused = false;
    }

    private void Update()
    {
        if (path == null)
            return;

        if (Paused)
            return;
        
        var newPos = Vector2.MoveTowards(transform.position, target, path.speed * Time.deltaTime);
        transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);

        const float Epsilon = 0.1f;

        if (DistanceToTarget() < Epsilon)
            Destroy(gameObject);
    }

    private float DistanceToTarget()
    {
        return ((Vector2)transform.position - (Vector2)target).magnitude;
    }

    public void SetPath(Path path)
    {
        this.path = path;

        target = path.destinationPoint.transform.position;
    }
}
