using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public event Action onSpawnReached;
    public Converter converter;
    public Firearm firearm;

    Movement movement;
    Inventory inventory;
    Health health;

    private void Awake()
	{
		movement = GetComponent<Movement>();
        inventory = GetComponent<Inventory>();
        health = GetComponent<Health>();
	}

	private void Update()
	{
		var dir = GetCurrentDirection();
		movement.SetDirection(dir);

        if (Input.GetMouseButtonDown(0))
        {
            var consumption = firearm.EnergyConsumption();
            if (health.GetHealth() > consumption && firearm.TryShoot())
            {
                health.TakeDamage(consumption);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            converter.TryShoot();
        }
    }

	Vector2 GetCurrentDirection()
	{
		var dir = GetWASDDirection();

		return dir;
	}

	Vector2 GetWASDDirection()
	{
		Vector2 dir = Vector2.zero;

		if (Input.GetKey(KeyCode.W))
			dir += new Vector2(0.0f, 1.0f);
		if (Input.GetKey(KeyCode.S))
			dir += new Vector2(0.0f, -1.0f);
		if (Input.GetKey(KeyCode.A))
			dir += new Vector2(-1.0f, 0.0f);
		if (Input.GetKey(KeyCode.D))
			dir += new Vector2(1.0f, 0.0f);

		return dir;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
        var obj = collision.gameObject;

		if (obj.layer == LayerMask.NameToLayer("PlayerSpawn"))
		{
			if (onSpawnReached != null)
				onSpawnReached.Invoke();
		}

        inventory.TryAddItem(obj.GetComponent<Collectible>());

        TryOpenChest(obj.GetComponent<Chest>());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var obj = collision.gameObject;

        TryOpenDoor(obj.GetComponent<Door>());
    }

    bool TryOpenChest(Chest chest)
    {
        if (!chest)
            return false;
        if (!chest.IsLocked())
            return false;

        foreach (var item in inventory.GetItems())
        {
            chest.TryUnlock(item.gameObject);
            if (!chest.IsLocked())
            {
                inventory.RemoveItem(item);

                // Get come reward from the chest
                Debug.Log("Chest opened!");

                return true;
            }
        }

        Debug.Log("Can't open chest now");

        return false;
    }

    bool TryOpenDoor(Door door)
    {
        if (!door)
            return false;
        if (!door.IsLocked())
            return false;

        foreach (var item in inventory.GetItems())
        {
            door.TryOpen(item.gameObject);
            if (!door.IsLocked())
            {
                inventory.RemoveItem(item);
                
                Debug.Log("Door opened!");

                return true;
            }
        }

        Debug.Log("Can't open door now");

        return false;
    }
}
