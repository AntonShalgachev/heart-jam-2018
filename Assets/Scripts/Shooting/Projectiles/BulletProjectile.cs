using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    public GameObject bulletPrefab;

    ProjectileController projectileController;

    private void Awake()
    {
        projectileController = GetComponent<ProjectileController>();
    }

    private void Start()
    {
        projectileController.SetBulletPrefab(bulletPrefab);
    }
}
