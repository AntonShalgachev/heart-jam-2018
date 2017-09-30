using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public GameObject dungeon;
	public GameObject playerPrefab;
	public GameObject zombiePrefab;
	public GameObject treasurePrefab;
	public HealthBar healthBar;

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
		SpawnTreasure();
	}

	void SpawnPlayer()
	{
		var playerPos = tileGenerator.GetPlayerSpawn().transform.position;
		var player = Instantiate(playerPrefab, playerPos, Quaternion.identity);

		Camera.main.GetComponent<PlayerFollower>().SetPlayer(player);
		healthBar.SetPlayer(player);
	}

	void SpawnZombies()
	{
		var spawns = tileGenerator.GetZombieSpawns();
		var zombies = new GameObject("Zombies");

		foreach (var spawn in spawns)
		{
			var pos = spawn.transform.position;
			var zombie = Instantiate(zombiePrefab, pos, Quaternion.identity, zombies.transform);
			zombie.name = "Zombie";
		}
	}

	void SpawnTreasure()
	{
		var pos = tileGenerator.GetTreasureSpawn().transform.position;
		var player = Instantiate(treasurePrefab, pos, Quaternion.identity);
	}
}
