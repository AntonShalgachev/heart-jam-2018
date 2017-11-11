using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public GameObject dungeon;
	public GameObject playerPrefab;
	public GameObject zombiePrefab;
	public HealthBar healthBar;

	public HudController hud;

	DungeonGenerator dungeonGenerator;
	TileGenerator tileGenerator;
	PlayerController playerController;

	DungeonGenerator.DungeonData dungeonData;

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

	}

	void SetupGame()
	{
		dungeonGenerator = dungeon.GetComponent<DungeonGenerator>();
		tileGenerator = dungeon.GetComponent<TileGenerator>();

		dungeonData = dungeonGenerator.GenerateDungeon();

		tileGenerator.GenerateDungeon();

		SpawnPlayer();
		SpawnZombies();

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
}
