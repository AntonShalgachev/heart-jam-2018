using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardDirection : DirectionController
{
    public int sideDirections;
    public float mainSpeed;
    public float sideSpeed;
    public float sideAngle;

    public override List<DirectionParam> GetDirectionParams()
    {
        var dirParams = new List<DirectionParam>
        {
            new DirectionParam(Vector2.right, mainSpeed),
        };

        for (var i = 0; i < sideDirections; i++)
        {
            dirParams.AddRange(new List<DirectionParam>{
                new DirectionParam(Quaternion.Euler(0, 0, (i+1)*sideAngle) * Vector2.right, sideSpeed),
                new DirectionParam(Quaternion.Euler(0, 0, -(i+1)*sideAngle) * Vector2.right, sideSpeed),
            });
        }

        return dirParams;
    }
}
