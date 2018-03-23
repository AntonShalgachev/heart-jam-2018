using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : ProjectileController
{
    public GameObject bulletPrefab;

    public override GameObject GetBulletPrefab()
    {
        return bulletPrefab;
    }
}
