using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardDirection : MonoBehaviour
{
    public int sideDirections;
    public float mainSpeed;
    public float sideSpeed;
    public float sideAngle;

    DirectionController directionController;

    private void Awake()
    {
        directionController = GetComponent<DirectionController>();
    }

    private void Start()
    {
        var dirParams = new List<DirectionController.DirectionParam>
        {
            new DirectionController.DirectionParam(Vector2.right, mainSpeed),
        };

        for (var i = 0; i < sideDirections; i++)
        {
            dirParams.AddRange(new List<DirectionController.DirectionParam>{
                new DirectionController.DirectionParam(Quaternion.Euler(0, 0, (i+1)*sideAngle) * Vector2.right, sideSpeed),
                new DirectionController.DirectionParam(Quaternion.Euler(0, 0, -(i+1)*sideAngle) * Vector2.right, sideSpeed),
            });
        }

        directionController.setDirectionParams(dirParams);
    }
}
