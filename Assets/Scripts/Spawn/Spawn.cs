using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject[] objects;

    public void Trigger()
    {
        var prefab = RandomHelper.Instance().GetItem(objects);

        Instantiate(prefab, transform.position, Quaternion.identity);
    }
}
