using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropulsionController : MonoBehaviour
{
    [Serializable]
    public class PropulsionParams
    {
        public float damageMultiplier;
        public float reloadingSpeed;
        public float spread;
        public float energyConsumption;

        public bool limitedAmmo;
        public int maxAmmo;
        public int initialAmmo;
    }

    public virtual PropulsionParams GetParams()
    {
        return null;
    }
}
