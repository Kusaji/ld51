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
    public List<GameObject> targets;
    public GameObject projectileSpawnpoint;
    public float randomSphereSpawnPoint = 1f;
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
            if (distanceToTarget > towerAttacker.EffectiveAttackRange)
            {
                Debug.DrawLine(projectileSpawnpoint.transform.position, target.transform.position, Color.white);
            }
            else if (distanceToTarget <= towerAttacker.EffectiveAttackRange)
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

            if (towerAttacker.multishot > 1)
                targets.Clear();

            for (int i = 0; i < EnemyManager.Instance.activeEnemies.Count; i++)
            {
                if (Vector3.Distance(transform.position, EnemyManager.Instance.activeEnemies[i].transform.position) < closestEnemyDistance)
                {
                    closestEnemyDistance = Vector3.Distance(transform.position, EnemyManager.Instance.activeEnemies[i].transform.position);
                    closestEnemy = EnemyManager.Instance.activeEnemies[i];
                }
                if (towerAttacker.multishot > 1 && Vector3.Distance(transform.position, EnemyManager.Instance.activeEnemies[i].transform.position) < towerAttacker.EffectiveAttackRange)
                {
                    targets.Add(EnemyManager.Instance.activeEnemies[i]);
                }
            }
            
            target = closestEnemy;
            distanceToTarget = closestEnemyDistance;

            if (towerAttacker.multishot > 1)
                SortTargets();

        }
    }

    private void SortTargets()
    {
        if (targets != null && targets.Count > 1)
        {
            targets.Sort((a, b) => CompareByDistance(Vector3.Distance(b.transform.position, transform.position), Vector3.Distance(a.transform.position, transform.position)));
            int desiredCount = towerAttacker.multishot;
            if (targets.Count > desiredCount)
                targets.RemoveRange(towerAttacker.multishot, targets.Count - desiredCount);
        }
    }
    public static int CompareByDistance(float a, float b)
    {
        if (a > b)
            return -1;
        else if (a < b)
            return 1;
        else
            return 0;
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
