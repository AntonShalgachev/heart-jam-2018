using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFollower : MonoBehaviour
{
    [SerializeField]
    private bool destroyIfTargetLost;

    private GameObject target;
    private bool targetSet = false;

    private Movement movement;

    private void Start()
    {
        movement = GetComponent<Movement>();
        Debug.Assert(movement);
    }

    private void Update()
    {
        if (target)
        {
            movement.SetDirection(target.transform.position - transform.position);
        } else if (destroyIfTargetLost && targetSet)
        {
            Destroy(gameObject);
        }
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
        this.targetSet = true;
    }
}
