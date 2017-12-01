using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject[] objects;

    public GameObject Trigger()
    {
        var prefab = RandomHelper.Instance().GetItem(objects);

        return Instantiate(prefab, transform.position, Quaternion.identity);
    }
}
