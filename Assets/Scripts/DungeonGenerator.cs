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

	public IntRange numberOfZombiesRange;

	private System.Random rnd;
	private DungeonData currentDungeon;
	private int numberOfZombies;

	public enum TileType
	{
		Floor,
		Wall
	}

	public class DungeonData
	{
		public TileType[,] tiles;
		public Coord playerSpawn;
		public List<Coord> zombieSpawns;

		public DungeonData(int w, int h)
		{
			tiles = new TileType[w, h];
		}
	}

	[Serializable]
	public class IntRange
	{
		public int min;
		public int max;

		public IntRange(int min, int max)
		{
			this.min = min;
			this.max = max;
		}

		public int Random(System.Random rnd)
		{
			return rnd.Next(min, max);
		}
	}

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
		rnd = new System.Random(randomSeed);

		currentDungeon = new DungeonData(mapWidth, mapHeight);
		InitData();

		GenerateTiles();
		GenerateSpawns();

		return currentDungeon;
	}

	void GenerateTiles()
	{
		FillRandomly();
		for (int i = 0; i < smoothingIterations; i++)
			Smooth();

		//FillSmallRegions();
	}

	void GenerateSpawns()
	{
		GeneratePlayerSpawnTile();
		GenerateZombieSpawnTiles();
	}

	void InitData()
	{
		numberOfZombies = numberOfZombiesRange.Random(rnd);
		currentDungeon.zombieSpawns = new List<Coord>();
	}

	void FillRandomly()
	{
		for (int x = 0; x < mapWidth; ++x)
		{
			for (int y = 0; y < mapHeight; ++y)
			{
				if (x == 0 || x == mapWidth - 1 || y == 0 || y == mapHeight - 1)
					currentDungeon.tiles[x, y] = TileType.Wall;
				else
					currentDungeon.tiles[x, y] = (rnd.NextDouble() < fillChance) ? TileType.Floor : TileType.Wall;
			}
		}
	}

	void Smooth()
	{
		for (int x = 0; x < mapWidth; ++x)
		{
			for (int y = 0; y < mapHeight; ++y)
			{
				int wallCount = CountSurroundingWalls(x, y);

				if (wallCount > cellularThreshold)
					currentDungeon.tiles[x, y] = TileType.Wall;
				else if (wallCount < cellularThreshold)
					currentDungeon.tiles[x, y] = TileType.Floor;
			}
		}
	}

	int CountSurroundingWalls(int cx, int cy)
	{
		int wallsCount = 0;

		for (int x = cx - 1; x <= cx + 1; ++x)
		{
			for (int y = cy - 1; y <= cy + 1; ++y)
			{
				if (x == cx && y == cy)
					continue;

				if (!IsInMapRange(x, y) || currentDungeon.tiles[x, y] == TileType.Wall)
					wallsCount++;
			}
		}

		return wallsCount;
	}

	bool IsInMapRange(int x, int y)
	{
		return x >= 0 && x < mapWidth && y >= 0 && y < mapHeight;
	}

	void FillSmallRegions()
	{

	}

	List<List<Coord>> GetRegions(TileType tileType)
	{
		List<List<Coord>> regions = new List<List<Coord>>();
		bool[,] flooded = new bool[mapWidth, mapHeight];

		for (int x = 0; x < mapWidth; ++x)
		{
			for (int y = 0; y < mapHeight; ++y)
			{
				if (!flooded[x, y] && currentDungeon.tiles[x, y] == tileType)
				{
					List<Coord> newRegion = GetRegionTiles(flooded, x, y);
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

	List<Coord> GetRegionTiles(bool[,] flooded, int startX, int startY)
	{
		List<Coord> tiles = new List<Coord>();
		var tileType = currentDungeon.tiles[startX, startY];

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
						if (!flooded[x, y] && currentDungeon.tiles[x, y] == tileType)
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

	void GeneratePlayerSpawnTile()
	{
		for (int i = 0; i < mapWidth; i++)
		{
			for (int j = 0; j < mapHeight; j++)
			{
				if (currentDungeon.tiles[i, j] == TileType.Floor)
					currentDungeon.playerSpawn = new Coord(i, j);
			}
		}
	}

	void GenerateZombieSpawnTiles()
	{
		while(currentDungeon.zombieSpawns.Count < numberOfZombies)
		{
			var x = new IntRange(0, mapWidth - 1).Random(rnd);
			var y = new IntRange(0, mapHeight - 1).Random(rnd);

			if (currentDungeon.tiles[x, y] == TileType.Floor)
				currentDungeon.zombieSpawns.Add(new Coord(x, y));
		}
	}
}
