using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool needsKey;

    bool locked = true;

    public void TryOpen(GameObject item)
    {
        if (!ValidateKey(item))
            return;

        OpenDoor();
    }

    public bool ValidateKey(GameObject item)
    {
        if (!needsKey)
            return true;

        // Here we can check colors

        return item && item.GetComponent<Key>();
    }

    void OpenDoor()
    {
        locked = false;

        gameObject.SetActive(false);
    }

    public bool IsLocked()
    {
        return locked;
    }
}
