using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
	public GameObject emptyTile;
	public GameObject wallTile;
	public bool spawnTiles;
	public float tileSize;

	public GameObject floorObject;
	public GameObject tilesObject;
	public GameObject wallsObject;
	public GameObject playerSpawnObject;
	public GameObject zombieSpawnsObject;
	public GameObject treasureSpawnObject;

	DungeonGenerator.DungeonData currentDungeon;
	bool[,] boolDungeonMap;

	List<Vector3> vertices;
	List<int> triangles;
	Dictionary<int, List<Triangle>> triangleDict = new Dictionary<int, List<Triangle>>();
	List<List<int>> outlines = new List<List<int>>();
	HashSet<int> checkedVertices = new HashSet<int>();
	SquareGrid squareGrid;

	public void GenerateDungeon()
	{
		var dungeonGenerator = GetComponent<DungeonGenerator>();
		currentDungeon = dungeonGenerator.GenerateDungeon();

		if (spawnTiles)
			GenerateTiles();

		CreateFloor();
		CreateCaveMesh();
		CreatePlayerSpawn();
		CreateZombieSpawns();
		CreateTreasureSpawn();
	}

	void GenerateTiles()
	{
		var map = currentDungeon.tiles;
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

	Vector2 GetTileCenter(int x, int y)
	{
		var width = currentDungeon.tiles.GetLength(0);
		var height = currentDungeon.tiles.GetLength(1);
		return new Vector2((x - 0.5f * width) * tileSize, (y - 0.5f * height) * tileSize);
	}

	void CreateFloor()
	{
		var width = currentDungeon.tiles.GetLength(0) * tileSize / 10;
		var height = currentDungeon.tiles.GetLength(1) * tileSize / 10;
		floorObject.transform.localScale = new Vector3(width, 1.0f, height);
	}

	void CreateCaveMesh()
	{
		triangleDict.Clear();
		outlines.Clear();
		checkedVertices.Clear();

		var width = currentDungeon.tiles.GetLength(0);
		var height = currentDungeon.tiles.GetLength(1);
		boolDungeonMap = new bool[width, height];
		for (int i = 0; i < width; i++)
			for (int j = 0; j < height; j++)
				boolDungeonMap[i, j] = currentDungeon.tiles[i, j] == DungeonGenerator.TileType.Wall;

		squareGrid = new SquareGrid(boolDungeonMap, tileSize);

		vertices = new List<Vector3>();
		triangles = new List<int>();

		for (int x = 0; x < squareGrid.squares.GetLength(0); ++x)
		{
			for (int y = 0; y < squareGrid.squares.GetLength(1); ++y)
			{
				TriangulateSquare(squareGrid.squares[x, y]);
			}
		}

		Mesh mesh = new Mesh();

		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray();
		mesh.RecalculateNormals();

		int tileAmount = 5;
		Vector2[] uvs = new Vector2[vertices.Count];
		for (int i = 0; i < vertices.Count; ++i)
		{
			float percentX = Mathf.InverseLerp(-boolDungeonMap.GetLength(0) / 2 * tileSize, boolDungeonMap.GetLength(0) / 2 * tileSize, vertices[i].x) * tileAmount;
			float percentY = Mathf.InverseLerp(-boolDungeonMap.GetLength(1) / 2 * tileSize, boolDungeonMap.GetLength(1) / 2 * tileSize, vertices[i].y) * tileAmount;
			uvs[i] = new Vector2(percentX, percentY);
		}
		mesh.uv = uvs;

		var walls = wallsObject.GetComponent<MeshFilter>();
		walls.mesh = mesh;

		Generate2DColliders();
	}

	void Generate2DColliders()
	{
		EdgeCollider2D[] currentColliders = gameObject.GetComponents<EdgeCollider2D>();
		for (int i = 0; i < currentColliders.Length; ++i)
		{
			Destroy(currentColliders[i]);
		}

		CalculateMeshOutlines();

		foreach (List<int> outline in outlines)
		{
			EdgeCollider2D edgeCollider = wallsObject.AddComponent<EdgeCollider2D>();
			Vector2[] edgePoints = new Vector2[outline.Count];

			for (int i = 0; i < outline.Count; ++i)
			{
				edgePoints[i] = new Vector2(vertices[outline[i]].x, vertices[outline[i]].y);
			}

			edgeCollider.points = edgePoints;
		}
	}

	void TriangulateSquare(Square square)
	{
		switch (square.configuration)
		{
			case 0:
				break;

			// 1 point
			case 1:
				MeshFromPoints(square.centerLeft, square.centerBottom, square.bottomLeft);
				break;
			case 2:
				MeshFromPoints(square.bottomRight, square.centerBottom, square.centerRight);
				break;
			case 4:
				MeshFromPoints(square.topRight, square.centerRight, square.centerTop);
				break;
			case 8:
				MeshFromPoints(square.topLeft, square.centerTop, square.centerLeft);
				break;

			// 2 points
			case 3:
				MeshFromPoints(square.centerRight, square.bottomRight, square.bottomLeft, square.centerLeft);
				break;
			case 6:
				MeshFromPoints(square.centerTop, square.topRight, square.bottomRight, square.centerBottom);
				break;
			case 9:
				MeshFromPoints(square.topLeft, square.centerTop, square.centerBottom, square.bottomLeft);
				break;
			case 12:
				MeshFromPoints(square.topLeft, square.topRight, square.centerRight, square.centerLeft);
				break;
			case 5:
				MeshFromPoints(square.centerTop, square.topRight, square.centerRight, square.centerBottom, square.bottomLeft, square.centerLeft);
				break;
			case 10:
				MeshFromPoints(square.topLeft, square.centerTop, square.centerRight, square.bottomRight, square.centerBottom, square.centerLeft);
				break;

			// 3 points
			case 7:
				MeshFromPoints(square.centerTop, square.topRight, square.bottomRight, square.bottomLeft, square.centerLeft);
				break;
			case 11:
				MeshFromPoints(square.topLeft, square.centerTop, square.centerRight, square.bottomRight, square.bottomLeft);
				break;
			case 13:
				MeshFromPoints(square.topLeft, square.topRight, square.centerRight, square.centerBottom, square.bottomLeft);
				break;
			case 14:
				MeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.centerBottom, square.centerLeft);
				break;

			// 4 point
			case 15:
				MeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.bottomLeft);
				break;
		}
	}

	void MeshFromPoints(params Node[] points)
	{
		AssignVertices(points);

		if (points.Length >= 3)
			CreateTriangle(points[0], points[1], points[2]);
		if (points.Length >= 4)
			CreateTriangle(points[0], points[2], points[3]);
		if (points.Length >= 5)
			CreateTriangle(points[0], points[3], points[4]);
		if (points.Length >= 6)
			CreateTriangle(points[0], points[4], points[5]);
	}

	void AssignVertices(Node[] points)
	{
		for (int i = 0; i < points.Length; ++i)
		{
			if (points[i].vertexIndex == -1)
			{
				points[i].vertexIndex = vertices.Count;
				vertices.Add(points[i].position);

				Debug.Assert(points[i].position.z == 0.0f);
				if (points[i].position.z != 0.0f)
					Debug.Log(points[i].position.z);
			}
		}
	}

	void CreateTriangle(Node a, Node b, Node c)
	{
		triangles.Add(a.vertexIndex);
		triangles.Add(b.vertexIndex);
		triangles.Add(c.vertexIndex);

		Triangle triangle = new Triangle(a.vertexIndex, b.vertexIndex, c.vertexIndex);
		AddTriangleToDict(triangle.vertexIndexA, triangle);
		AddTriangleToDict(triangle.vertexIndexB, triangle);
		AddTriangleToDict(triangle.vertexIndexC, triangle);
	}

	void AddTriangleToDict(int vertexIndexKey, Triangle triangle)
	{
		if (triangleDict.ContainsKey(vertexIndexKey))
		{
			triangleDict[vertexIndexKey].Add(triangle);
		}
		else
		{
			List<Triangle> triangleList = new List<Triangle>();
			triangleList.Add(triangle);
			triangleDict.Add(vertexIndexKey, triangleList);
		}
	}

	void CalculateMeshOutlines()
	{
		for (int vertexIndex = 0; vertexIndex < vertices.Count; ++vertexIndex)
		{
			if (!checkedVertices.Contains(vertexIndex))
			{
				int newOutlineVertex = GetConnectedOutlineVertex(vertexIndex);
				if (newOutlineVertex != -1)
				{
					checkedVertices.Add(vertexIndex);

					List<int> newOutline = new List<int>();
					newOutline.Add(vertexIndex);
					outlines.Add(newOutline);
					FollowOutline(newOutlineVertex, outlines.Count - 1);
					outlines[outlines.Count - 1].Add(vertexIndex);
				}
			}
		}
	}

	void FollowOutline(int vertexIndex, int outlineIndex)
	{
		outlines[outlineIndex].Add(vertexIndex);
		checkedVertices.Add(vertexIndex);

		int nextVertexIndex = GetConnectedOutlineVertex(vertexIndex);
		if (nextVertexIndex != -1)
			FollowOutline(nextVertexIndex, outlineIndex);
	}

	int GetConnectedOutlineVertex(int vertexIndex)
	{
		List<Triangle> trianglesContainingVertex = triangleDict[vertexIndex];

		for (int i = 0; i < trianglesContainingVertex.Count; ++i)
		{
			Triangle triangle = trianglesContainingVertex[i];

			for (int j = 0; j < 3; ++j)
			{
				int vertexB = triangle[j];

				if (vertexB != vertexIndex && !checkedVertices.Contains(vertexB))
				{
					if (IsOutlineEdge(vertexIndex, vertexB))
						return vertexB;
				}
			}
		}

		return -1;
	}

	bool IsOutlineEdge(int vertexA, int vertexB)
	{
		List<Triangle> trianglesContainingVertexA = triangleDict[vertexA];
		int sharedTriangleCount = 0;

		for (int i = 0; i < trianglesContainingVertexA.Count; ++i)
		{
			if (trianglesContainingVertexA[i].Contains(vertexB))
			{
				sharedTriangleCount++;
				if (sharedTriangleCount > 1)
					break;
			}
		}

		return sharedTriangleCount == 1;
	}

	struct Triangle
	{
		public int vertexIndexA;
		public int vertexIndexB;
		public int vertexIndexC;
		int[] vertices;


		public Triangle(int a, int b, int c)
		{
			vertexIndexA = a;
			vertexIndexB = b;
			vertexIndexC = c;

			vertices = new int[3];
			vertices[0] = a;
			vertices[1] = b;
			vertices[2] = c;
		}

		public int this[int i]
		{
			get
			{
				return vertices[i];
			}
		}

		public bool Contains(int vertexIndex)
		{
			return vertexIndex == vertexIndexA || vertexIndex == vertexIndexB || vertexIndex == vertexIndexC;
		}
	}

	public class SquareGrid
	{
		public Square[,] squares;

		public SquareGrid(bool[,] map, float squareSize)
		{
			int nodeCountX = map.GetLength(0);
			int nodeCountY = map.GetLength(1);

			float mapWidth = nodeCountX * squareSize;
			float mapHeight = nodeCountY * squareSize;

			ControlNode[,] controlNodes = new ControlNode[nodeCountX, nodeCountY];

			for (int x = 0; x < nodeCountX; ++x)
			{
				for (int y = 0; y < nodeCountY; ++y)
				{
					var pos = new Vector2(-mapWidth / 2.0f + x * squareSize, -mapHeight / 2.0f + y * squareSize);
					controlNodes[x, y] = new ControlNode(pos, map[x, y], squareSize);
				}
			}

			squares = new Square[nodeCountX - 1, nodeCountY - 1];
			for (int x = 0; x < nodeCountX - 1; ++x)
			{
				for (int y = 0; y < nodeCountY - 1; ++y)
				{
					squares[x, y] = new Square(controlNodes[x, y + 1], controlNodes[x + 1, y + 1], controlNodes[x + 1, y], controlNodes[x, y]);
				}
			}
		}
	}

	public class Square
	{
		public ControlNode topLeft, topRight, bottomLeft, bottomRight;
		public Node centerTop, centerRight, centerBottom, centerLeft;

		public int configuration;

		public Square(ControlNode topLeft, ControlNode topRight, ControlNode bottomRight, ControlNode bottomLeft)
		{
			this.topLeft = topLeft;
			this.topRight = topRight;
			this.bottomRight = bottomRight;
			this.bottomLeft = bottomLeft;

			centerTop = this.topLeft.right;
			centerRight = this.bottomRight.above;
			centerBottom = this.bottomLeft.right;
			centerLeft = this.bottomLeft.above;

			configuration = 0;

			if (this.topLeft.active)
				configuration += 8;
			if (this.topRight.active)
				configuration += 4;
			if (this.bottomRight.active)
				configuration += 2;
			if (this.bottomLeft.active)
				configuration += 1;
		}
	}

	public class Node
	{
		public Vector3 position;
		public int vertexIndex = -1;

		public Node(Vector3 position)
		{
			this.position = position;
		}
	}

	public class ControlNode : Node
	{
		public bool active;
		public Node above, right;

		public ControlNode(Vector3 pos, bool active, float squareSize) : base(pos)
		{
			this.active = active;
			above = new Node(position + Vector3.up * squareSize / 2.0f);
			right = new Node(position + Vector3.right * squareSize / 2.0f);
		}
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

	void CreateTreasureSpawn()
	{
		var spawn = new GameObject("Spawn");
		spawn.transform.parent = treasureSpawnObject.transform;
		spawn.transform.position = GetTileCenter(currentDungeon.treasureSpawn.x, currentDungeon.treasureSpawn.y);
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

	public GameObject GetTreasureSpawn()
	{
		return treasureSpawnObject.transform.GetChild(0).gameObject;
	}
}
