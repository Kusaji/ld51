using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles instantiation of enemy prefabs and scaling wave difficulty.
/// </summary>
public class EnemyManager : MonoBehaviour
{
    #region Variables
    [Header("Singleton")]
    public static EnemyManager Instance;

    
    public List<GameObject> enemyPrefabs
    {
        get
        {
            if (wave >= 35)
                return wave35enemyPrefabs;
            else if (wave >= 25)
                return wave25enemyPrefabs;
            else if (wave >= 15)
                return wave15enemyPrefabs;
            else if (wave >= 5)
                return wave5enemyPrefabs;
            else
                return wave0enemyPrefabs;
        }
    }
    [Header("Lists")]
    public List<GameObject> wave0enemyPrefabs;
    public List<GameObject> wave5enemyPrefabs;
    public List<GameObject> wave15enemyPrefabs;
    public List<GameObject> wave25enemyPrefabs;
    public List<GameObject> wave35enemyPrefabs;
    public List<GameObject> activeEnemies;
    public List<EnemyController> activeEnemiesScripts;

    [Header("Settings")]
    public float spawnTime;
    public int enemiesToSpawn; //increment every 10 seconds
    public float enemySpawnIncrement; //How many more enemies per wave?
    public float enemySpawnExponent; //Exponentially more enemies per wave!
    public float spawnRadius = 35f;

    [Header("Runtime Stats")]
    public int wave; 

    [Header("References")]
    public Transform enemyTransform;

    //Private
    private Vector3 spawnPosition;
    #endregion

    #region Unity Callbacks
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
    #endregion

    #region Methods
    public void GetRandomSpawnPos()
    {
        spawnPosition = Random.onUnitSphere;
        spawnPosition.y = 0;
        spawnPosition = spawnPosition.normalized * spawnRadius;
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
        activeEnemiesScripts.Add(spawnedEnemy.GetComponent<EnemyController>());
    }
    #endregion

    #region Coroutines
    public IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitUntil(() => PlayerStructures.instance.firstStructurePlaced);

        //TODO countdown for spawn
        for (int i = (int)spawnTime; i > 0; i--)
        {
            string timeString = "<mspace=0.5em> " + i;
            if (i == 10)
                timeString = "<mspace=0.5em>10";
            DebugTextCanvas.Instance._SetDbText($"Ass", $"Time until next wave : {timeString}");
            DebugTextCanvas.Instance._SetDbText($"wave", $"<size=65%>Wave : {wave}");
            //Add animations or whatever you want here.
            yield return new WaitForSeconds(1f);
        }

        while (PlayerResources.Instance.isAlive)
        {
            //Grab spot to spawn a clump of enemies
            GetRandomSpawnPos();

            //Spawn amount of enemies for said clump
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                SpawnEnemy();
                yield return null;
                yield return null;
            }
            
            //Increment waves
            wave++;
            DebugTextCanvas.Instance._SetDbText($"wave", $"<size=65%>Wave : {wave}");
            //Increase next wave amount
            enemiesToSpawn = Mathf.RoundToInt(Mathf.Pow(wave, enemySpawnExponent) * enemySpawnIncrement + 5f);

            //TODO countdown for spawn
            for (int i = (int)spawnTime; i > 0; i--)
            {
                string timeString = "<mspace=0.5em> " + i;
                if (i == 10)
                    timeString = "<mspace=0.5em>10";
                DebugTextCanvas.Instance._SetDbText($"Ass", $"Time until next wave : {timeString}");
                //Add animations or whatever you want here.
                yield return new WaitForSeconds(1f);
            }

            //Wait 10 seconds (We did it, the theme is here.)
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion
}
