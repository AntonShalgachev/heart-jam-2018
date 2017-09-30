using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
	static string TILES_NAME = "Tiles";

	public GameObject emptyTile;
	public GameObject wallTile;
	public float tileSize;

	DungeonGenerator.DungeonData currentDungeon;

	public void GenerateTiles()
	{
		var dungeonGenerator = GetComponent<DungeonGenerator>();
		var dungeonData = dungeonGenerator.GenerateDungeon();

		GenerateTiles(dungeonData);
	}

	public void GenerateTiles(DungeonGenerator.DungeonData dungeonData)
	{
		currentDungeon = dungeonData;

		var map = dungeonData.tiles;
		var width = map.GetLength(0);
		var height = map.GetLength(1);

		var tiles = GameObject.Find(TILES_NAME);
		if (tiles == null)
		{
			tiles = new GameObject(TILES_NAME);
			tiles.transform.parent = transform;
		}

		foreach (Transform child in tiles.transform)
		{
			Destroy(child.gameObject);
		}

		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				var prefab = emptyTile;
				if (map[i, j] == DungeonGenerator.TileType.Wall)
					prefab = wallTile;
				
				var pos = GetTileCenter(i, j);

				var tile = GameObject.Instantiate(prefab, pos, Quaternion.identity, tiles.transform);
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
}
