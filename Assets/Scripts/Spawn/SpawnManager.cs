using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public int[] enemiesPerWave;
    public float enemySpawnInterval;
    public float wavesInterval;

    List<Spawn> spawns;
    int currentWave = -1;
    bool waveEnded;
    float timeBeforeNextWave;
    float enemySpawnDelay;
    int enemiesLeftToSpawn;
    int enemiesLeftToDestroy;

    private void Awake()
    {
        spawns = new List<Spawn>(FindObjectsOfType<Spawn>());
        Debug.LogFormat("Found {0} spawns", spawns.Count);
    }

    private void Start()
    {
        EndWave();
    }

    private void Update()
    {
        timeBeforeNextWave -= Time.deltaTime;

        enemySpawnDelay -= Time.deltaTime;

        if (!waveEnded && enemySpawnDelay < 0.0f && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();

            enemySpawnDelay = enemySpawnInterval;
            enemiesLeftToDestroy++;
            enemiesLeftToSpawn--;
        }
    }

    int GetWaveEnemies()
    {
        int size = enemiesPerWave.Length;
        if (currentWave >= size)
            return enemiesPerWave[size - 1];

        return enemiesPerWave[currentWave];
    }

    void StartWaveIn(float delay)
    {
        Debug.LogFormat("Starting new wave in {0}", delay);

        timeBeforeNextWave = delay;
        Invoke("StartWave", delay);
    }

    void StartWave()
    {
        Debug.Log("Starting new wave");

        currentWave++;
        waveEnded = false;
        enemySpawnDelay = enemySpawnInterval;
        enemiesLeftToSpawn = GetWaveEnemies();
    }

    void EndWave()
    {
        Debug.Log("Ending wave");

        waveEnded = true;
        StartWaveIn(wavesInterval);
    }

    void SpawnEnemy()
    {
        var spawn = RandomHelper.Instance().GetItem(spawns);

        var enemy = spawn.Trigger().GetComponent<Enemy>();
        enemy.OnDeath += OnEnemyDestroyed;
        enemy.onConverted += OnEnemyDestroyed;
    }

    public void OnEnemyDestroyed()
    {
        enemiesLeftToDestroy--;
        if (enemiesLeftToDestroy <= 0 && enemiesLeftToSpawn <= 0)
            EndWave();
    }
}
