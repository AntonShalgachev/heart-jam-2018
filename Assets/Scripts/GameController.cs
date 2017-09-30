using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public GameObject dungeon;
	public GameObject playerPrefab;
	public GameObject zombiePrefab;

	DungeonGenerator dungeonGenerator;
	TileGenerator tileGenerator;

	DungeonGenerator.DungeonData dungeonData;

	private void Start()
	{
		Invoke("SetupGame", 0.1f);
	}

	void SetupGame()
	{
		dungeonGenerator = dungeon.GetComponent<DungeonGenerator>();
		tileGenerator = dungeon.GetComponent<TileGenerator>();

		dungeonData = dungeonGenerator.GenerateDungeon();

		tileGenerator.GenerateDungeon();

		SpawnPlayer();
		SpawnZombies();
	}

	void SpawnPlayer()
	{
		var playerPos = tileGenerator.GetPlayerSpawn().transform.position;
		var player = Instantiate(playerPrefab, playerPos, Quaternion.identity);

		Camera.main.GetComponent<PlayerFollower>().SetPlayer(player);
	}

	void SpawnZombies()
	{
		var spawns = tileGenerator.GetZombieSpawns();

		foreach (var spawn in spawns)
		{
			var pos = spawn.transform.position;
			var zombie = Instantiate(zombiePrefab, pos, Quaternion.identity);
		}
	}
}
