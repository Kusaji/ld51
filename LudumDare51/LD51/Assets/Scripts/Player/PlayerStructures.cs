using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Rendering;
//using UnityEditor.AI;
using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// Singleton.
/// Collection of spawnable player prefabs.
/// Contains lists accessed by other scripts for structure tracking.
/// </summary>
public class PlayerStructures : MonoBehaviour
{
    #region Variables
    [Header("Singleton")]
    public static PlayerStructures instance;

    [Header("Active Structures")]
    public List<GameObject> structures;
    public List<Structure> structuresScripts;

    [Header("Available Structures")]
    public List<GameObject> structurePrefabs;
    public List<StructureBalanceNumbersSO> structureBalanceNumbers;

    [Header("Inactive Structures")] //Match this with structure prefabs
    public List<GameObject> inactiveStructurePrefabs;

    [Header("Placement Materials")]
    public Material canPlaceHere;
    public Material noPlaceHere;

    [Header("References")]
    public AudioController audioController;

    [Header("Runtime | Do not set")]
    [Space(50)]
    public Vector3 mousePosition;
    public Vector3 MouseHitPosition;
    public GameObject bastion;
    public Transform structuresTransform;
    public int spawningTowerInt;
    public bool spawningTower;
    public bool canSpawnOnMouse;
    public bool canSpawnOnMouseLastFrame;
    public bool forceSetMaterials;
    public GameObject inactiveTower;
    private int minimumPopOfSelectedTower;
    public bool firstStructurePlaced = false;
    #endregion

    #region Events and Delegates
    //Events
    public delegate void TowerUpdateEvent();
    public static event TowerUpdateEvent TowerUpdate;
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        structuresTransform = GameObject.Find("Structures").gameObject.transform;

        //StartCoroutine(GameWaitsForNavMesh());
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

            if (forceSetMaterials || canSpawnOnMouse != canSpawnOnMouseLastFrame)
            {
                forceSetMaterials = false;
                MeshRenderer[] meshRenderers = inactiveTower.gameObject.GetComponentsInChildren<MeshRenderer>();
                
                for (int mrIter = 0; mrIter < meshRenderers.Length; mrIter++)
                {
                    int matArrayLength = meshRenderers[mrIter].sharedMaterials.Length;
                    if (matArrayLength == 1)
                        meshRenderers[mrIter].sharedMaterial = canSpawnOnMouse ? canPlaceHere : noPlaceHere;
                    else
                    {
                        Material[] matArray = new Material[matArrayLength];
                        for (int i = 0; i < matArrayLength; i++)
                        {
                            matArray[i] = canSpawnOnMouse ? canPlaceHere : noPlaceHere;
                        }
                        meshRenderers[mrIter].sharedMaterials = matArray;
                    }
                }
            }
        }
    }
    #endregion

    #region Methods
    public void AddStructure(GameObject structure)
    {
        structuresScripts.Add(structure.GetComponent<Structure>());
        structures.Add(structure);
        BroadcastTowerUpdate();
    }

    public void RemoveStructure(GameObject structure)
    {
        structuresScripts.Remove(structure.GetComponent<Structure>());
        structures.Remove(structure);        
        BroadcastTowerUpdate();
    }

    public void SpawnPlacementTower(int towerPrefab)
    {
        if (!spawningTower)
        {
            minimumPopOfSelectedTower = structureBalanceNumbers[towerPrefab].minimumPopulationToFunction;
            if (!spawningTower && PlayerResources.Instance.population >= minimumPopOfSelectedTower)
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

                forceSetMaterials = true;
            }
        }
    }

    public void BuildStructure(int towerPrefab)
    {
        GetMousePosition();

        if (canSpawnOnMouse && PlayerResources.Instance.population >= minimumPopOfSelectedTower)
        {
            var builtStructure = Instantiate(
                structurePrefabs[towerPrefab],
                MouseHitPosition,
                Quaternion.identity,
                structuresTransform
                ); ;

            AddStructure(builtStructure);

            builtStructure.GetComponent<Structure>().AddPopFromPlayer(minimumPopOfSelectedTower);

            spawningTower = false;
            Destroy(inactiveTower);

            audioController.PlayOneShot(0, 0.1f);
            firstStructurePlaced = true;
            //NavMeshBuilder.BuildNavMeshAsync();

        }
    }
    
    [Header("Tower Placement")]
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask structureMask;
    [SerializeField] private Vector3 towerSize = Vector3.one;
    public void GetMousePosition()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(PlayerInput.ScaledMousePosition);
        //if Physics.Raycast(ray, out hit)
        if (Physics.Raycast(ray, out hit, 9999f, groundMask))
        {
            MouseHitPosition = hit.point;

            canSpawnOnMouseLastFrame = canSpawnOnMouse;

            bool collisionOkay;
            //Debug.DrawRay(MouseHitPosition + Vector3.up * 10, Vector3.down * 10, Color.red);
            
            if (Physics.BoxCast(MouseHitPosition + Vector3.up * 10f, towerSize, Vector3.down,quaternion.identity, 10f, structureMask))
            {
                //Debug.Log("Hitting");
                collisionOkay = false;
            }
            else
            {
                //Debug.Log("Not hitting");
                collisionOkay = true;
            }
            /*
            //if (!hit.transform.gameObject.CompareTag("Structure") && !hit.transform.gameObject.CompareTag("Environment"))
            if (hit.transform.gameObject.CompareTag("Ground"))
            {
                collisionOkay = true;
            } 
            else
            {
                collisionOkay = false;
            }
            */
            bool builderInRange = false;
            for (int i = 0; i < structures.Count; i++)
            {
                if (structures[i] != null)
                {
                    var builder = structures[i].GetComponent<BuilderTower>();
                    if (builder != null && builder.GetPointInRange(MouseHitPosition))
                    {
                        builderInRange = true;
                        break;
                    }
                }
            }

            canSpawnOnMouse = builderInRange && collisionOkay;
        }
    }

    public void BroadcastTowerUpdate()
    {
        if (TowerUpdate != null)
        {
            TowerUpdate();
        }
    }
    #endregion

    #region Coroutines
    IEnumerator GameWaitsForNavMesh()
    {
        Time.timeScale = 0f;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        //NavMeshBuilder.BuildNavMeshAsync();
        //yield return new WaitUntil(() => NavMeshBuilder.isRunning);
        Time.timeScale = 1f;
    }
    #endregion
}
