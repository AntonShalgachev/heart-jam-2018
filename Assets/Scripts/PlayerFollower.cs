using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
	GameObject player;

	public void SetPlayer(GameObject player)
	{
		this.player = player;
	}

	private void Update()
	{
		if (player != null)
		{
			var playerPos = player.transform.position;
			transform.position = new Vector3(playerPos.x, playerPos.y, transform.position.z);
		}
	}
}
