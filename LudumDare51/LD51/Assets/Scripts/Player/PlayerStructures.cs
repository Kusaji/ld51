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

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        structuresTransform = GameObject.Find("Structures").gameObject.transform;
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
            inactiveTower.transform.position = mousePosition;
        }
    }

    public void AddStructure(GameObject structure)
    {
        structures.Add(structure);
    }

    public void RemoveStructure(GameObject structure)
    {
        structures.Remove(structure);
    }

    public void SpawnPlacementTower(int towerPrefab)
    {
        GetMousePosition();

        spawningTower = true;
        spawningTowerInt = towerPrefab;

        inactiveTower = Instantiate(
            inactiveStructurePrefabs[towerPrefab],
            mousePosition,
            Quaternion.identity,
            structuresTransform
            );
    }

    public void BuildStructure(int towerPrefab)
    {
        GetMousePosition();

        if (canSpawnOnMouse)
        {
            var builtStructure = Instantiate(
                structurePrefabs[towerPrefab],
                mousePosition,
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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            mousePosition = hit.point;

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
}
