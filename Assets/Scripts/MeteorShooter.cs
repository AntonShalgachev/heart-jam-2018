using Assets.Scripts.ShipSatellite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorShooter : MonoBehaviour
{
    public enum ClosestDistanceMode
    {
        ToSelf,
        ToShip,
    }

    [SerializeField]
    private float shootingPeriod;
    [SerializeField]
    private float shootingRadius;
    [SerializeField]
    private LayerMask targetLayerMask;
    [SerializeField]
    private ClosestDistanceMode closestDistanceMode;
    [SerializeField]
    private GameObject bulletPrefab;

    private bool isEnabled;
    private float delay;

    private GameObject bulletHolder;
    private GameObject ship;

	void Start ()
    {
        var shipSatellite = GetComponent<ship_satellite>();
        Debug.Assert(shipSatellite);
        ship = shipSatellite.ship;
        Debug.Assert(ship);

        SetActive(false);

        bulletHolder = new GameObject("Bullets");
    }
    
	void Update ()
    {
        if (!isEnabled)
            return;

        delay -= Time.deltaTime;

        if (delay < 0.0f)
        {
            Shoot();
            delay = shootingPeriod;
        }
	}

    void Shoot()
    {
        var radiusMult = 1.0f;
        if (TutorialController.Instance.IsEnabled())
            radiusMult = 20.0f;
        var colliders = Physics2D.OverlapCircleAll(transform.position, shootingRadius * radiusMult, targetLayerMask);

        Collider2D closestCollider = null;
        float closestDist = 0.0f;
        foreach (var collider in colliders)
        {
            var thisDist = DistanceToTarget(collider.transform);
            if (!closestCollider || thisDist < closestDist)
            {
                closestCollider = collider;
                closestDist = thisDist;
            }
        }

        if (closestCollider)
        {
            var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity, bulletHolder.transform);

            bullet.GetComponent<TargetFollower>().SetTarget(closestCollider.gameObject);
        }
    }

    float DistanceToTarget(Transform transform)
    {
        GameObject target = null;
        switch(closestDistanceMode)
        {
            case ClosestDistanceMode.ToSelf:
                target = gameObject;
                break;
            case ClosestDistanceMode.ToShip:
                target = ship;
                break;
        }

        Debug.Assert(target);
        
        return (target.transform.position - transform.position).magnitude;
    }

    public void SetActive(bool enabled)
    {
        isEnabled = enabled;
        delay = enabled ? shootingPeriod : 0.0f;
    }
}
