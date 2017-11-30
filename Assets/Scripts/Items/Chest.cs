using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public bool locked = true;

    public void TryUnlock(GameObject item)
    {
        if (!IsLocked())
            return;

        var key = item.GetComponent<Key>();
        if (!key)
            return;

        // Here we can check colors

        Unlock();
    }

    public bool IsLocked()
    {
        return locked;
    }

    void Unlock()
    {
        locked = false;
        OnUnlocked();
    }

    void OnUnlocked()
    {
        var drop = GetComponent<DeathDrop>();
        if (drop)
            drop.Drop();
    }
}
