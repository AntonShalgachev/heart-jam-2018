using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int capacity;
    public GameObject inventoryHolder;

    List<Collectible> inventory = new List<Collectible>();

    public bool TryAddItem(Collectible item)
    {
        if (inventory.Count >= capacity)
            return false;

        inventory.Add(item);
        AttachObject(item.gameObject);

        return true;
    }

    public bool TryAddItem(GameObject obj)
    {
        var item = obj.GetComponent<Collectible>();
        if (!item)
            return false;

        return TryAddItem(item);
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
}
