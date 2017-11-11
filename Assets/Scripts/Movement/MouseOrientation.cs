using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOrientation : MonoBehaviour
{
	public float maxVelocity;
	//public float maxForce;
	//public float forceScale;
	//public float offset;

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

		//targetPos = Vector2.Lerp(pos, targetPos, offset);

		//var torque = Vector2.SignedAngle(transform.right, dir);
		//torque *= forceScale;
		//torque = Mathf.Clamp(torque, -maxForce, maxForce);

		//Debug.Log(torque);
		//rigidBody.AddTorque(torque);

		transform.rotation = Quaternion.RotateTowards(transform.rotation, targetOrientation, maxVelocity * Time.deltaTime);

		//rigidBody.angularVelocity = Mathf.Clamp(rigidBody.angularVelocity, -maxVelocity, maxVelocity);
	}
}
