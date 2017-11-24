using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropulsionController : MonoBehaviour
{
    [Serializable]
    public struct PropulsionParams
    {
        public float damageMultiplier;
        public float reloadingSpeed;
        public float spread;
    }

    public PropulsionParams propulsionParams;
}
