using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    public List<GameObject> enemyPrefabs;
    public List<GameObject> activeEnemies;
    public float spawnTime;

    public int wave; 
    public int enemiesToSpawn; //increment every 10 seconds
    public int enemySpawnIncrement; //How many more enemies per wave?

    private Vector3 spawnPosition;

    public Transform enemyTransform;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        enemyTransform = GameObject.Find("Enemies").transform;
        transform.position = GameObject.Find("Bastion").transform.position;
        StartCoroutine(SpawnEnemyRoutine());
    }

    public void GetRandomSpawnPos()
    {
        spawnPosition = Random.onUnitSphere;
        spawnPosition.y = 0;
        spawnPosition = spawnPosition.normalized * 30f;
    }

    public void SpawnEnemy()
    {
        var spawnedEnemy = 
            Instantiate(
            enemyPrefabs[Random.Range(0, enemyPrefabs.Count)], //what
            spawnPosition, //where
            Quaternion.identity,
            enemyTransform); //rotation

        activeEnemies.Add(spawnedEnemy);
    }

    public IEnumerator SpawnEnemyRoutine()
    {
        while (PlayerResources.Instance.isAlive)
        {
            //Grab spot to spawn a clump of enemies
            GetRandomSpawnPos();

            //Spawn amount of enemies for said clump
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                SpawnEnemy();
            }

            //Increment waves
            wave++;
            //Increase next wave amount
            enemiesToSpawn = wave + 5;

            //TODO countdown for spawn
            for (int i = (int)spawnTime; i > 0; i--)
            {
                DebugTextCanvas.Instance._SetDbText($"Ass", $"Time until next wave : {i}");
                //Add animations or whatever you want here.
                yield return new WaitForSeconds(1f);
            }

            //Wait 10 seconds (We did it, the theme is here.)
            yield return new WaitForEndOfFrame();
        }
    }
}
