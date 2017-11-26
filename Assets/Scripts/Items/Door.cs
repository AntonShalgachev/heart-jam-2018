using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    bool locked = true;

    public void TryOpen(GameObject item)
    {
        var key = item.GetComponent<Key>();
        if (!key)
            return;

        // Here we can check colors

        OpenDoor();
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
