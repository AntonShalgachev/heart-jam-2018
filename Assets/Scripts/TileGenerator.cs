using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
	static string TILES_NAME = "Tiles";
	static string PLAYER_SPAWN_NAME = "PlayerSpawn";
	static string ZOMBIE_SPAWNS_NAME = "ZombieSpawns";

	public GameObject emptyTile;
	public GameObject wallTile;
	public float tileSize;

	DungeonGenerator.DungeonData currentDungeon;
	GameObject tilesObject;
	GameObject playerSpawnObject;
	GameObject zombieSpawnsObject;

	private void Start()
	{
		tilesObject = GameObject.Find(TILES_NAME);
		Debug.Assert(tilesObject);

		playerSpawnObject = GameObject.Find(PLAYER_SPAWN_NAME);
		Debug.Assert(playerSpawnObject);

		zombieSpawnsObject = GameObject.Find(ZOMBIE_SPAWNS_NAME);
		Debug.Assert(zombieSpawnsObject);
	}

	public void GenerateDungeon()
	{
		SpawnTiles();
		CreatePlayerSpawn();
		CreateZombieSpawns();
	}

	void SpawnTiles()
	{
		var dungeonGenerator = GetComponent<DungeonGenerator>();
		var dungeonData = dungeonGenerator.GenerateDungeon();

		SpawnTiles(dungeonData);
	}

	void SpawnTiles(DungeonGenerator.DungeonData dungeonData)
	{
		currentDungeon = dungeonData;

		var map = dungeonData.tiles;
		var width = map.GetLength(0);
		var height = map.GetLength(1);

		foreach (Transform child in tilesObject.transform)
			Destroy(child.gameObject);

		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				var prefab = emptyTile;
				if (map[i, j] == DungeonGenerator.TileType.Wall)
					prefab = wallTile;
				
				var pos = GetTileCenter(i, j);

				var tile = Instantiate(prefab, pos, Quaternion.identity, tilesObject.transform);
				tile.transform.localScale = Vector2.Scale(tile.transform.localScale, Vector2.one * tileSize);
			}
		}
	}

	public Vector2 GetTileCenter(int x, int y)
	{
		var width = currentDungeon.tiles.GetLength(0);
		var height = currentDungeon.tiles.GetLength(1);
		return new Vector2((x - 0.5f * width) * tileSize, (y - 0.5f * height) * tileSize);
	}

	void CreatePlayerSpawn()
	{
		var playerSpawn = new GameObject("Spawn");
		playerSpawn.transform.parent = playerSpawnObject.transform;
		playerSpawn.transform.position = GetTileCenter(currentDungeon.playerSpawn.x, currentDungeon.playerSpawn.y);
	}

	void CreateZombieSpawns()
	{
		foreach (var coord in currentDungeon.zombieSpawns)
		{
			var spawn = new GameObject("Spawn");
			spawn.transform.parent = zombieSpawnsObject.transform;
			spawn.transform.position = GetTileCenter(coord.x, coord.y);
		}
	}

	public GameObject GetPlayerSpawn()
	{
		return playerSpawnObject.transform.GetChild(0).gameObject;
	}

	public List<GameObject> GetZombieSpawns()
	{
		List<GameObject> spawns = new List<GameObject>();
		for (int i = 0; i < zombieSpawnsObject.transform.childCount; i++)
		{
			spawns.Add(zombieSpawnsObject.transform.GetChild(i).gameObject);
		}

		return spawns;
	}
}
