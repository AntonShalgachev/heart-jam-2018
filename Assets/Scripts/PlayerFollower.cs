using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerFollower : MonoBehaviour
{
    private GameObject player;
    
	public void SetPlayer(GameObject player)
	{
		this.player = player;
	}

    private void Start()
    {
        var _player = GameObject.FindWithTag("Player");
        if(_player != null)
        {
            player = _player;
        }
        SceneManager.LoadScene("game_ui", LoadSceneMode.Additive);
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
