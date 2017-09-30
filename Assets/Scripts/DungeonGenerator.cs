using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
	public int randomSeed;
	
	public int mapWidth;
	public int mapHeight;

	[Range(0.0f, 1.0f)]
	public float fillChance;
	public int smoothingIterations;
	public int cellularThreshold;

	public int wallSizeThreshold;
	public int roomSizeThreshold;

	private System.Random rnd;

	public enum TileType
	{
		Floor,
		Wall
	}

	public class DungeonData
	{
		public TileType[,] tiles;
		public Coord startingTile;

		public DungeonData(int w, int h, bool initToWall = false)
		{
			tiles = new TileType[w, h];

			if (initToWall)
			{
				for (int x = 0; x < w; x++)
				{
					for (int y = 0; y < h; y++)
					{
						tiles[x, y] = TileType.Wall;
					}
				}
			}
		}
	}

	//[Serializable]
	//public class IntRange
	//{
	//	public int min;
	//	public int max;

	//	public int Random(System.Random rnd)
	//	{
	//		return rnd.Next(min, max);
	//	}
	//}

	public struct Coord
	{
		public int x, y;

		public Coord(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
	}

	public DungeonData GenerateDungeon()
	{
		var data = new DungeonData(mapWidth, mapHeight);
		rnd = new System.Random(randomSeed);

		FillRandomly(data);
		for (int i = 0; i < smoothingIterations; i++)
		{
			Smooth(data);
		}

		//FillSmallRegions(data);
		FindStartingTile(data);

		return data;
	}

	void FillRandomly(DungeonData data)
	{
		for (int x = 0; x < mapWidth; ++x)
		{
			for (int y = 0; y < mapHeight; ++y)
			{
				if (x == 0 || x == mapWidth - 1 || y == 0 || y == mapHeight - 1)
					data.tiles[x, y] = TileType.Wall;
				else
					data.tiles[x, y] = (rnd.NextDouble() < fillChance) ? TileType.Floor : TileType.Wall;
			}
		}
	}

	void Smooth(DungeonData data)
	{
		for (int x = 0; x < mapWidth; ++x)
		{
			for (int y = 0; y < mapHeight; ++y)
			{
				int wallCount = CountSurroundingWalls(data, x, y);

				if (wallCount > cellularThreshold)
					data.tiles[x, y] = TileType.Wall;
				else if (wallCount < cellularThreshold)
					data.tiles[x, y] = TileType.Floor;
			}
		}
	}

	int CountSurroundingWalls(DungeonData data, int cx, int cy)
	{
		int wallsCount = 0;

		for (int x = cx - 1; x <= cx + 1; ++x)
		{
			for (int y = cy - 1; y <= cy + 1; ++y)
			{
				if (x == cx && y == cy)
					continue;

				if (!IsInMapRange(x, y) || data.tiles[x, y] == TileType.Wall)
					wallsCount++;
			}
		}

		return wallsCount;
	}

	bool IsInMapRange(int x, int y)
	{
		return x >= 0 && x < mapWidth && y >= 0 && y < mapHeight;
	}

	void FillSmallRegions(DungeonData data)
	{

	}

	List<List<Coord>> GetRegions(DungeonData data, TileType tileType)
	{
		List<List<Coord>> regions = new List<List<Coord>>();
		bool[,] flooded = new bool[mapWidth, mapHeight];

		for (int x = 0; x < mapWidth; ++x)
		{
			for (int y = 0; y < mapHeight; ++y)
			{
				if (!flooded[x, y] && data.tiles[x, y] == tileType)
				{
					List<Coord> newRegion = GetRegionTiles(data, flooded, x, y);
					regions.Add(newRegion);

					foreach (Coord tile in newRegion)
					{
						flooded[tile.x, tile.y] = true;
					}
				}
			}
		}

		return regions;
	}

	List<Coord> GetRegionTiles(DungeonData data, bool[,] flooded, int startX, int startY)
	{
		List<Coord> tiles = new List<Coord>();
		var tileType = data.tiles[startX, startY];

		Queue<Coord> queue = new Queue<Coord>();
		queue.Enqueue(new Coord(startX, startY));
		flooded[startX, startY] = true;

		while (queue.Count > 0)
		{
			Coord tile = queue.Dequeue();
			tiles.Add(tile);

			for (int x = tile.x - 1; x <= tile.x + 1; ++x)
			{
				for (int y = tile.y - 1; y <= tile.y + 1; ++y)
				{
					if (IsInMapRange(x, y) && (y == tile.y || x == tile.x))
					{
						if (!flooded[x, y] && data.tiles[x, y] == tileType)
						{
							flooded[x, y] = true;
							queue.Enqueue(new Coord(x, y));
						}
					}
				}
			}
		}

		return tiles;
	}

	void FindStartingTile(DungeonData data)
	{
		for (int i = 0; i < mapWidth; i++)
		{
			for (int j = 0; j < mapHeight; j++)
			{
				if (data.tiles[i, j] == TileType.Floor)
					data.startingTile = new Coord(i, j);
			}
		}
	}
}
