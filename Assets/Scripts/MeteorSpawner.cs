using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    [Serializable]
    public class MineMeteorPathParams
    {
        public List<GameObject> spawnPoints;
        public List<OccupiablePoint> holdPoints;
        public List<GameObject> destinationPoints;

        public RandomHelper.Range speed;
        public RandomHelper.Range holdDuration;
    }
    [Serializable]
    public class EnemyMeteorPathParams
    {
        public List<GameObject> spawnPoints;
        public List<GameObject> destinationPoints;

        public RandomHelper.Range speed;
    }

    [SerializeField]
    private List<MineMeteorPathParams> mineMeteorPaths;
    [SerializeField]
    private float mineMeteorSpawnPeriod;
    [SerializeField]
    private List<EnemyMeteorPathParams> enemyMeteorPaths;
    [SerializeField]
    private float enemyMeteorSpawnPeriod;
    [SerializeField]
    private RandomHelper.IntRange enemyMeteorSpawnGroupSize;

    [SerializeField]
    private GameObject minerMeteorPrefab;
    [SerializeField]
    private GameObject enemyMeteorPrefab;

    [SerializeField]
    private GameHandler gameHandler;

    [SerializeField]
    private float miningCapacityMulMax;
    [SerializeField]
    private float miningCapacityMulMin;
    [SerializeField]
    private float miningCapacityDifficultyCoeff;
    [SerializeField]
    private float groupSizeDifficultyCoeff;

    private GameObject meteorHolder;

    private void Start()
    {
        meteorHolder = new GameObject("Meteors");

        StartCoroutine(StartSpawningMineMeteors());
        StartCoroutine(StartSpawningEnemyMeteors());
    }

    public float GetDifficultyCoefficient()
    {
        var dist = gameHandler.GetDistance();

        return Mathf.Log10(Mathf.Max(dist, 10.0f));
    }

    public float GetReverseDifficultyCoefficient(float min, float max, float k)
    {
        var dist = gameHandler.GetDistance();

        var p = Mathf.Pow(10.0f, k / (max - min));

        return k / Mathf.Log10(dist + p) + min;
    }

    private MineMeteorMovement.Path getRandomMineMeteorPath()
    {
        Debug.Assert(mineMeteorPaths.Count > 0);

        var random = RandomHelper.Instance();
        var pathParams = random.GetItem(mineMeteorPaths);

        var path = new MineMeteorMovement.Path();
        path.spawnPoint = random.GetItem(pathParams.spawnPoints);

        do
        {
            path.holdPoint = random.GetItem(pathParams.holdPoints);
        }
        while (path.holdPoint.IsOccupied);

        path.destinationPoint = random.GetItem(pathParams.destinationPoints);

        path.speed = pathParams.speed.GetRandom();
        path.holdDuration = pathParams.holdDuration.GetRandom();

        return path;
    }

    private EnemyMeteorMovement.Path getRandomEnemyMeteorPath()
    {
        Debug.Assert(enemyMeteorPaths.Count > 0);

        var random = RandomHelper.Instance();
        var pathParams = random.GetItem(enemyMeteorPaths);

        var path = new EnemyMeteorMovement.Path();
        path.spawnPoint = random.GetItem(pathParams.spawnPoints);
        path.destinationPoint = random.GetItem(pathParams.destinationPoints);

        path.speed = pathParams.speed.GetRandom();

        return path;
    }

    private void SpawnMineMeteor(MineMeteorMovement.Path path)
    {
        var meteor = Instantiate(minerMeteorPrefab, path.spawnPoint.transform.position, Quaternion.identity, meteorHolder.transform);
        var meteorMovement = meteor.GetComponent<MineMeteorMovement>();
        Debug.Assert(meteorMovement);

        meteorMovement.SetPath(path);

        var mineMeteor = meteor.GetComponent<mine_meteor>();
        Debug.Assert(mineMeteor);

        var mul = GetReverseDifficultyCoefficient(miningCapacityMulMin, miningCapacityMulMax, miningCapacityDifficultyCoeff);
        Debug.Log(String.Format("Distance {0}, mult {1}", gameHandler.GetDistance(), mul));
        mineMeteor.SetMineMultiplier(mul);
    }

    private void SpawnEnemyMeteor(EnemyMeteorMovement.Path path)
    {
        var meteor = Instantiate(enemyMeteorPrefab, path.spawnPoint.transform.position, Quaternion.identity, meteorHolder.transform);
        var meteorMovement = meteor.GetComponent<EnemyMeteorMovement>();
        Debug.Assert(meteorMovement);

        meteorMovement.SetPath(path);
    }

    private IEnumerator StartSpawningMineMeteors()
    {
        while (true)
        {
            SpawnMineMeteor(getRandomMineMeteorPath());
            yield return new WaitForSeconds(mineMeteorSpawnPeriod);
        }
    }

    private IEnumerator StartSpawningEnemyMeteors()
    {
        while (true)
        {
            var groupSize = enemyMeteorSpawnGroupSize.GetRandom();
            groupSize = (int)(groupSize * groupSizeDifficultyCoeff * GetDifficultyCoefficient());
            for (var i = 0; i < groupSize; i++)
                SpawnEnemyMeteor(getRandomEnemyMeteorPath());

            yield return new WaitForSeconds(enemyMeteorSpawnPeriod);
        }
    }
}
