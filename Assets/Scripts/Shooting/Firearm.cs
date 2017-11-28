using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firearm : MonoBehaviour
{
    public string id;

    ProjectileController projectileController;
    PropulsionController propulsionController;
    DirectionController directionController;

    GameObject bulletsHolder;
    float shootingDelay = 0.0f;

    bool limitedAmmo;
    int ammo;
    int maxAmmo;

    private void Awake()
    {
        projectileController = GetComponent<ProjectileController>();
        propulsionController = GetComponent<PropulsionController>();
        directionController = GetComponent<DirectionController>();
    }

    private void Start()
    {
        var bulletsHolderName = "Firearm projectiles";
        bulletsHolder = GameObject.Find(bulletsHolderName);

        if (!bulletsHolder)
            bulletsHolder = new GameObject(bulletsHolderName);

        limitedAmmo = propulsionController.GetParams().limitedAmmo;
        ammo = propulsionController.GetParams().initialAmmo;
        maxAmmo = propulsionController.GetParams().maxAmmo;
    }

    private void Update()
    {
        shootingDelay -= Time.deltaTime;
    }

    public bool HasAmmo()
    {
        return !limitedAmmo || ammo > 0;
    }

    public void Merge(Firearm firearm)
    {
        if (id != firearm.id)
            return;

        if (limitedAmmo && firearm.limitedAmmo)
            AddAmmo(firearm.ammo);
    }

    void AddAmmo(int amount)
    {
        ammo = Mathf.Min(ammo + amount, maxAmmo);
    }

    public bool TryShoot()
    {
        if (shootingDelay > 0.0f)
            return false;

        if (!HasAmmo())
            return false;

        var directions = directionController.GetDirectionParams();
        var projectile = projectileController.GetBulletPrefab();
        var propulsionParams = propulsionController.GetParams();

        foreach (var direction in directions)
        {
            var dir = direction.direction;
            var shootingSpread = propulsionParams.spread;
            var speed = direction.speed;

            var spread = Random.Range(-shootingSpread, shootingSpread);
            dir = Quaternion.Euler(0, 0, spread) * transform.rotation * dir.normalized;

            Shoot(projectile, dir, speed, propulsionParams.damageMultiplier);
        }

        shootingDelay = propulsionParams.reloadingSpeed;

        if (limitedAmmo)
            ammo--;

        return true;
    }

    public void Shoot(GameObject bulletPrefab, Vector2 dir, float bulletSpeed, float multiplier)
    {
        var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity, bulletsHolder.transform);
        bullet.name = "Bullet";

        var attack = bullet.GetComponent<Attack>();
        Debug.Assert(attack);
        attack.SetMultiplier(multiplier);

        var body = bullet.GetComponent<Rigidbody2D>();
        Debug.Assert(body);

        body.AddForce(dir * bulletSpeed, ForceMode2D.Force);

        bullet.transform.rotation = Quaternion.FromToRotation(Vector2.up, dir);
    }

    public float EnergyConsumption()
    {
        return propulsionController.GetParams().energyConsumption;
    }
}
