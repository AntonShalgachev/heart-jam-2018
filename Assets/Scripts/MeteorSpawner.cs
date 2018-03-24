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
        public List<GameObject> holdPoints;
        public List<GameObject> destinationPoints;

        public RandomHelper.Range speed;
        public RandomHelper.Range holdDuration;
    }

    [SerializeField]
    private List<MineMeteorPathParams> mineMeteorPaths;
    [SerializeField]
    private float mineMeteorSpawnPeriod;

    [SerializeField]
    private GameObject minerMeteorPrefab;

    private GameObject meteorHolder;

    private void Start()
    {
        meteorHolder = new GameObject("Meteors");

        StartCoroutine(StartSpawningMineMeteors());
    }

    private void Update()
    {

    }

    private MineMeteorMovement.Path getRandomMineMeteorPath()
    {
        Debug.Assert(mineMeteorPaths.Count > 0);

        var random = RandomHelper.Instance();
        var pathParams = random.GetItem(mineMeteorPaths);

        var path = new MineMeteorMovement.Path();
        path.spawnPoint = random.GetItem(pathParams.spawnPoints);
        path.holdPoint = random.GetItem(pathParams.holdPoints);
        path.destinationPoint = random.GetItem(pathParams.destinationPoints);

        path.speed = pathParams.speed.GetRandom();
        path.holdDuration = pathParams.holdDuration.GetRandom();

        return path;
    }

    private void SpawnMineMeteor(MineMeteorMovement.Path path)
    {
        var meteor = Instantiate(minerMeteorPrefab, path.spawnPoint.transform.position, Quaternion.identity, meteorHolder.transform);
        var meteorMovement = meteor.GetComponent<MineMeteorMovement>();
        Debug.Assert(meteorMovement);

        meteorMovement.SetPath(path);
    }

    private IEnumerator StartSpawningMineMeteors()
    {
        while (true)
        {
            var path = getRandomMineMeteorPath();
            SpawnMineMeteor(path);
            yield return new WaitForSeconds(mineMeteorSpawnPeriod);
        }
    }
}
