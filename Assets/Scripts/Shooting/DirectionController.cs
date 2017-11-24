using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionController : MonoBehaviour
{
    public struct DirectionParam
    {
        public DirectionParam(Vector2 dir, float speed)
        {
            direction = dir;
            this.speed = speed;
        }

        public Vector2 direction;
        public float speed;
    }

    List<DirectionParam> directionParams;

    public List<DirectionParam> getDirectionParams()
    {
        return directionParams;
    }

    public void setDirectionParams(List<DirectionParam> directionParams)
    {
        this.directionParams = directionParams;
    }
}
