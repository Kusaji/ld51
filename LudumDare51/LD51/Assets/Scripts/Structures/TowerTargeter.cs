using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Handles finding nearest enemy and setting targets for towers.
/// </summary>
public class TowerTargeter : MonoBehaviour
{
    #region Variables
    [Header("Debug")]
    public bool debugMode;

    [Header("Target Info")]
    public GameObject target;
    public GameObject[] targetsArr = new GameObject[5];
    public GameObject projectileSpawnpoint;
    public float randomSphereSpawnPoint = 1f;
    public float distanceToTarget;
    public Vector3 cachedPosition;

    [Header("Components")]
    public TowerAttacker towerAttacker;
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        cachedPosition = transform.position;
    }
    private void Start()
    {
        StartCoroutine(FindTargetRoutine());
        targetsArr = new GameObject[towerAttacker.multishot];        
    }
    //private void Update()
    //{
    //    if (debugMode && target != null)
    //    {
    //        if (distanceToTarget > towerAttacker.EffectiveAttackRange)
    //        {
    //            Debug.DrawLine(projectileSpawnpoint.transform.position, target.transform.position, Color.white);
    //        }
    //        else if (distanceToTarget <= towerAttacker.EffectiveAttackRange)
    //        {
    //            Debug.DrawLine(projectileSpawnpoint.transform.position, target.transform.position, Color.blue);
    //        }
    //    }
    //}
    #endregion

    #region Methods
    public void FindNewTarget()
    {
        if (EnemyManager.Instance.activeEnemies.Count > 0)
        {
            

            for (int i = 0; i < targetsArr.Length; i++) {
                targetsArr[i] = null;
            }

            var closestEnemy = EnemyManager.Instance.activeEnemies[0];
            float closestEnemyDistance = Mathf.Infinity;


            for (int multishotIter = 0; multishotIter < towerAttacker.multishot; multishotIter++) {

                closestEnemy = EnemyManager.Instance.activeEnemies[0];
                closestEnemyDistance = Mathf.Infinity;

                for (int i = 0; i < EnemyManager.Instance.activeEnemies.Count; i++)
                {
                    bool checkDistance = true;
                    for (int z = 0; z < towerAttacker.multishot; z++)
                    {
                        //if (targetsArr[z].GetInstanceID() == EnemyManager.Instance.activeEnemies[i].GetInstanceID())
                        if (targetsArr[z] == EnemyManager.Instance.activeEnemies[i])
                            checkDistance = false;
                    }
                    if (checkDistance && Vector3.Distance(cachedPosition, EnemyManager.Instance.activeEnemiesScripts[i].MyPosition) < closestEnemyDistance)
                    {
                        closestEnemyDistance = Vector3.Distance(cachedPosition, EnemyManager.Instance.activeEnemiesScripts[i].MyPosition);
                        closestEnemy = EnemyManager.Instance.activeEnemies[i];
                        targetsArr[multishotIter] = EnemyManager.Instance.activeEnemies[i];
                    }
                }


            }
            
            target = closestEnemy;
            distanceToTarget = closestEnemyDistance;

        }
    }
    #endregion

    #region Coroutines
    public IEnumerator FindTargetRoutine()
    {
        while (gameObject)
        {
            FindNewTarget();
            yield return new WaitForSeconds(0.1f);
        }
    }
    #endregion
}
