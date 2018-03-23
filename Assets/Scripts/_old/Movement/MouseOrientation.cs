using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOrientation : MonoBehaviour
{
	public float maxVelocity;

	Rigidbody2D rigidBody;

	private void Awake()
	{
		rigidBody = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		Vector2 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 pos = gameObject.transform.position;

		var dir = targetPos - pos;
		var targetOrientation = Quaternion.FromToRotation(Vector2.right, dir);

		transform.rotation = Quaternion.RotateTowards(transform.rotation, targetOrientation, maxVelocity * Time.deltaTime);
	}
}
