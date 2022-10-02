using System.Collections;
using System.Collections.Generic;
using UnityEditor.AI;
using UnityEngine;

public class PlayerStructures : MonoBehaviour
{
    public static PlayerStructures instance;

    [Header("Active Structures")]
    public List<GameObject> structures;

    [Header("Available Structures")]
    public List<GameObject> structurePrefabs;

    [Header("Inactive Structures")] //Match this with structure prefabs
    public List<GameObject> inactiveStructurePrefabs;

    public Vector3 mousePosition;
    public GameObject bastion;
    public Transform structuresTransform;

    public int spawningTowerInt;
    public bool spawningTower;
    public bool canSpawnOnMouse;
    public GameObject inactiveTower;

    public delegate void TowerUpdateEvent();
    public static event TowerUpdateEvent TowerUpdate;

    public Vector3 MouseHitPosition;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        structuresTransform = GameObject.Find("Structures").gameObject.transform;

        StartCoroutine(GameWaitsForNavMesh());
    }

    IEnumerator GameWaitsForNavMesh()
    {
        
        Time.timeScale = 0f;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        NavMeshBuilder.BuildNavMeshAsync();        
        yield return new WaitUntil(() => NavMeshBuilder.isRunning);        
        Time.timeScale = 1f;
    }

    private void Update()
    {        
        if (Input.GetMouseButtonDown(0) && spawningTower)
        {
            BuildStructure(spawningTowerInt);
        }

        if (spawningTower && inactiveTower != null)
        {
            GetMousePosition();
            inactiveTower.transform.position = MouseHitPosition;
        }
    }

    public void AddStructure(GameObject structure)
    {
        structures.Add(structure);
        BroadcastTowerUpdate();
    }

    public void RemoveStructure(GameObject structure)
    {
        structures.Remove(structure);
        BroadcastTowerUpdate();
    }

    public void SpawnPlacementTower(int towerPrefab)
    {
        if (!spawningTower)
        {
            GetMousePosition();

            spawningTower = true;
            spawningTowerInt = towerPrefab;

            inactiveTower = Instantiate(
                inactiveStructurePrefabs[towerPrefab],
                MouseHitPosition,
                Quaternion.identity,
                structuresTransform
                );
        }
    }

    public void BuildStructure(int towerPrefab)
    {
        GetMousePosition();

        if (canSpawnOnMouse)
        {
            var builtStructure = Instantiate(
                structurePrefabs[towerPrefab],
                MouseHitPosition,
                Quaternion.identity,
                structuresTransform
                ); ;

            AddStructure(builtStructure);

            spawningTower = false;
            Destroy(inactiveTower);
            NavMeshBuilder.BuildNavMeshAsync();

        }
    }

    public void GetMousePosition()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(PlayerInput.ScaledMousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            MouseHitPosition = hit.point;

            if (!hit.transform.gameObject.CompareTag("Structure") && !hit.transform.gameObject.CompareTag("Environment"))
            {
                canSpawnOnMouse = true;
            } 
            else
            {
                canSpawnOnMouse = false;
            }
        }
    }


    public void BroadcastTowerUpdate()
    {
        if (TowerUpdate != null)
        {
            TowerUpdate();
        }
    }
}
