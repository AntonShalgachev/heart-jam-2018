using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultPropulsion : PropulsionController
{
    public PropulsionParams propulsionParams;

    public override PropulsionParams GetParams()
    {
        return propulsionParams;
    }
}
