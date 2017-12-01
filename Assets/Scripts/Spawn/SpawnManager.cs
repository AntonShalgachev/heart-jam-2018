using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    List<Spawn> spawns;

    private void Awake()
    {
        spawns = new List<Spawn>(FindObjectsOfType<Spawn>());
    }
}
