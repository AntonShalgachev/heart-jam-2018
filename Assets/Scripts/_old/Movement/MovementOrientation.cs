using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class MovementOrientation : MonoBehaviour
{
	private Movement movement;

	private void Awake()
	{
		movement = GetComponent<Movement>();
	}

	private void Update()
	{
		var dir = movement.GetDirection();
		if (dir.magnitude > 1e-3)
			transform.rotation = Quaternion.FromToRotation(Vector2.right, dir);
	}
}
