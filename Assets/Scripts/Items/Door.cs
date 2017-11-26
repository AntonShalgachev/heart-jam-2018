using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    bool locked = true;
    HingeJoint2D doorJoint;

    private void Awake()
    {
        doorJoint = GetComponent<HingeJoint2D>();
    }

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

        doorJoint.useMotor = true;
        var motor = doorJoint.motor;
        motor.motorSpeed *= -1;
        doorJoint.motor = motor;
    }

    public bool IsLocked()
    {
        return locked;
    }
}
