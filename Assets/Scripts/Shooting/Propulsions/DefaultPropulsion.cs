using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultPropulsion : MonoBehaviour
{
    public PropulsionController.PropulsionParams propulsionParams;

    PropulsionController controller;

    private void Awake()
    {
        controller = GetComponent<PropulsionController>();
    }

    private void Start()
    {
        controller.propulsionParams = propulsionParams;
    }
}
