using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyEfx : MonoBehaviour {

    public GameObject diePrefub;
    // Use this for initialization
    private bool isQuitting = false;

    void Start ()
    {

    }
    void OnApplicationQuit()
    {
        isQuitting = true;
    }
    void OnDestroy()
    {
        if (diePrefub != null && !isQuitting && !GameHandler.isRestarting())
        {
            var _inst = Instantiate(diePrefub);
            _inst.transform.position = transform.position;
        }
    }
}

