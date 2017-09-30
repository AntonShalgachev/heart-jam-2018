using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public GameObject dungeon;
	public GameObject playerPrefab;

	DungeonGenerator dungeonGenerator;
	TileGenerator tileGenerator;

	DungeonGenerator.DungeonData dungeonData;

	private void Start()
	{
		dungeonGenerator = dungeon.GetComponent<DungeonGenerator>();
		tileGenerator = dungeon.GetComponent<TileGenerator>();

		dungeonData = dungeonGenerator.GenerateDungeon();

		tileGenerator.GenerateTiles();

		SpawnPlayer();
	}

	void SpawnPlayer()
	{
		var playerPos = tileGenerator.GetTileCenter(dungeonData.startingTile.x, dungeonData.startingTile.y);
		var player = Instantiate(playerPrefab, playerPos, Quaternion.identity);

		Camera.main.GetComponent<PlayerFollower>().SetPlayer(player);
	}
}
