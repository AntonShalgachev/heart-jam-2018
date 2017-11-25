using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    bool locked = true;

    public void TryUnlock(GameObject item)
    {
        var key = item.GetComponent<Key>();
        if (!key)
            return;

        // Here we can check colors

        locked = false;
    }

    public bool IsLocked()
    {
        return locked;
    }
}
