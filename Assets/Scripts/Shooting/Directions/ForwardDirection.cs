using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardDirection : MonoBehaviour
{
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
        directionController.setDirectionParams(new List<DirectionController.DirectionParam> {
            new DirectionController.DirectionParam(Vector2.right, mainSpeed),
            new DirectionController.DirectionParam(Quaternion.Euler(0, 0, sideAngle) * Vector2.right, sideSpeed),
            new DirectionController.DirectionParam(Quaternion.Euler(0, 0, -sideAngle) * Vector2.right, sideSpeed),
        });
    }
}
