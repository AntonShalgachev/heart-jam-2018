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
	public MeshRenderer fogOfWar;

	public HudController hud;

	DungeonGenerator dungeonGenerator;
	TileGenerator tileGenerator;
	PlayerController playerController;

	DungeonGenerator.DungeonData dungeonData;

	bool treasureCollected;

	public static GameController instance;

	private void Awake()
	{
		Debug.Assert(instance == null, "There can't be multiple game controllers");
		instance = this;

		Debug.Assert(hud);
	}

	private void Start()
	{
		Invoke("SetupGame", 0.1f);
	}

	private void Update()
	{
		if (playerController)
		{
			fogOfWar.material.SetVector("_PlayerPos", playerController.transform.position);
			fogOfWar.material.SetVector("_PlayerDir", playerController.transform.rotation * Vector2.right);
		}
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

		hud.InitObjectives();
	}

	void SpawnPlayer()
	{
		var playerPos = tileGenerator.GetPlayerSpawn().transform.position;
		var player = Instantiate(playerPrefab, playerPos, Quaternion.identity);

		Camera.main.GetComponent<PlayerFollower>().SetPlayer(player);
		healthBar.SetPlayer(player);

		playerController = player.GetComponent<PlayerController>();
		Debug.Assert(playerController);
		SetupPlayer();
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

	void SetupPlayer()
	{
		playerController.onTreasureCollected += OnTreasureCollected;
		playerController.onSpawnReached += OnSpawnReached;
	}

	void OnTreasureCollected()
	{
		Debug.Log("Treasure collected");
		treasureCollected = true;
		hud.OnTreasureCollected();
	}

	void OnSpawnReached()
	{
		if (!treasureCollected)
			return;

		Debug.Log("Spawn reached");
		hud.OnSpawnReached();
	}
}
