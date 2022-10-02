using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameObject projectileSpawnpoint;
    public float distanceToTarget;

    [Header("Components")]
    public TowerAttacker towerAttacker;
    #endregion

    #region Unity Callbacks
    private void Start()
    {
        StartCoroutine(FindTargetRoutine());
    }
    private void Update()
    {
        if (debugMode && target != null)
        {
            if (distanceToTarget > towerAttacker.attackRange)
            {
                Debug.DrawLine(projectileSpawnpoint.transform.position, target.transform.position, Color.white);
            }
            else if (distanceToTarget <= towerAttacker.attackRange)
            {
                Debug.DrawLine(projectileSpawnpoint.transform.position, target.transform.position, Color.blue);
            }
        }
    }
    #endregion

    #region Methods
    public void FindNewTarget()
    {
        if (EnemyManager.Instance.activeEnemies.Count > 0)
        {
            var closestEnemy = EnemyManager.Instance.activeEnemies[0];
            float closestEnemyDistance = Mathf.Infinity;

            for (int i = 0; i < EnemyManager.Instance.activeEnemies.Count; i++)
            {
                if (Vector3.Distance(transform.position, EnemyManager.Instance.activeEnemies[i].transform.position) < closestEnemyDistance)
                {
                    closestEnemyDistance = Vector3.Distance(transform.position, EnemyManager.Instance.activeEnemies[i].transform.position);
                    closestEnemy = EnemyManager.Instance.activeEnemies[i];
                }
            }

            target = closestEnemy;
            distanceToTarget = closestEnemyDistance;
        }
    }
    #endregion

    #region Coroutines
    public IEnumerator CalculateDistanceToTargetRoutine()
    {
        while (gameObject)
        {
            if (target != null)
            {
                distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

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
