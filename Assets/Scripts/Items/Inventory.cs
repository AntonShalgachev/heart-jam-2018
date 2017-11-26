using Assets.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : PropertyChanger
{
    public int capacity;
    public GameObject inventoryHolder;

    List<Collectible> inventory = new List<Collectible>();

    public bool TryAddItem(Collectible item)
    {
        if (!item)
            return false;

        if (inventory.Count >= capacity)
            return false;

        inventory.Add(item);
        AttachObject(item.gameObject);
        Debug.LogFormat("Item '{0}' added to inventory", item.name);
        OnPropertyChanged("Inventory");
        return true;
    }

    public void AttachObject(GameObject obj)
    {
        obj.transform.parent = inventoryHolder.transform;
        obj.transform.localPosition = Vector2.zero;

#warning antonsh do we need this?
        var renderer = obj.GetComponent<Renderer>();
        if (renderer)
            renderer.enabled = false;
    }

    public List<Collectible> GetItems()
    {
        return new List<Collectible>(inventory);
    }

    public void RemoveItem(Collectible item)
    {
        inventory.Remove(item);
        OnPropertyChanged("Inventory");
    }
}
