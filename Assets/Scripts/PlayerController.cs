using Assets.Scripts.Items;
using Assets.Scripts.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PropertyChanger
{
	public event Action onSpawnReached;
    public Converter converter;
    public GameObject firearmsHolder;

    public Firearm[] defaultFirearms;

    List<Firearm> equippedFirearms = new List<Firearm>();
    public Firearm currentFirearm;
    public int currentFirearmIndex = -1;

    Movement movement;
    Inventory inventory;
    Health health;
    Targets targets;

    private void Awake()
	{
		movement = GetComponent<Movement>();
        inventory = GetComponent<Inventory>();
        health = GetComponent<Health>();
        targets = GetComponent<Targets>();
    }

    private void Start()
    {
        foreach (var prefab in defaultFirearms)
        {
            var item = Instantiate(prefab.gameObject, transform.position, Quaternion.identity);
            TryAddWeapon(item.GetComponent<Firearm>());
        }

        if (defaultFirearms.Length > 0)
            SelectWeapon(0);
    }

    private void Update()
	{
		var dir = GetCurrentDirection();
		movement.SetDirection(dir);

        if (Input.GetMouseButtonDown(0))
        {
            OnPropertyChanged("Shoot");
            if (currentFirearm)
            {
                var consumption = currentFirearm.EnergyConsumption();
                if (health.GetHealth() > consumption && currentFirearm.TryShoot())
                {
                    health.TakeDamage(consumption);
                }
            }

            if (!currentFirearm.HasAmmo())
            {
                RemoveWeapon(currentFirearm);
                currentFirearm = null;
                SelectWeapon(0);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            converter.TryShoot();
        }

        if (Input.GetMouseButtonDown(2))
            NextWeapon();
    }

    Firearm FindWeapon(string id)
    {
        foreach (var firearm in equippedFirearms)
        {
            if (firearm.id == id)
                return firearm;
        }

        return null;
    }

    void TryAddWeapon(Firearm item, bool select = false)
    {
        if (!item)
            return;

        var firearm = FindWeapon(item.id);

        if (firearm)
        {
            firearm.Merge(item);

            Destroy(item.gameObject);
        }
        else
        {
            AddNewWeapon(item);

            if (select)
                SelectWeapon(NumberOfWeapons() - 1);

            item.transform.parent = firearmsHolder.transform;
            item.transform.localPosition = Vector2.zero;
            item.transform.localRotation = Quaternion.identity;
        }
    }

    void AddNewWeapon(Firearm firearm)
    {
        equippedFirearms.Add(firearm);
    }

    void RemoveWeapon(Firearm firearm)
    {
        equippedFirearms.Remove(firearm);
        Destroy(firearm.gameObject);
    }

    void SelectWeapon(int index)
    {
        if (index < 0 || index >= equippedFirearms.Count)
        {
            Debug.LogError("Weapon index out of bounds");
            return;
        }

        currentFirearmIndex = index;
        currentFirearm = equippedFirearms[index];

        foreach (var firearm in equippedFirearms)
        {
            firearm.gameObject.SetActive(firearm == currentFirearm);
        }
    }

    int NumberOfWeapons()
    {
        return equippedFirearms.Count;
    }

    void NextWeapon()
    {
        if (NumberOfWeapons() == 0)
            return;

        var index = (currentFirearmIndex + 1) % NumberOfWeapons();
        SelectWeapon(index);
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
        OnEncounter(collision.gameObject, true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnEncounter(collision.gameObject, false);

    }

    void OnEncounter(GameObject obj, bool trigger)
    {
        inventory.TryAddItem(obj.GetComponent<Collectible>());
        TryAddWeapon(obj.GetComponent<Firearm>(), true);

        TryOpenChest(obj.GetComponent<Chest>());
        TryOpenDoor(obj.GetComponent<Door>());
        targets.TryAddTarget(obj.GetComponent<GameEvents>());
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

        door.TryOpen(null);
        if (!door.IsLocked())
        {
            Debug.Log("Door opened without the key!");
            return true;
        }

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
