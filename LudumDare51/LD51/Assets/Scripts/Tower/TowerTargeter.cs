using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerTargeter : MonoBehaviour
{
    [Header("Debug")]
    public bool debugMode;

    [Header("Target Info")]
    public GameObject target;
    public GameObject projectileSpawnpoint;
    public float distanceToTarget;

    [Header("Components")]
    public TowerAttacker towerAttacker;

    private void Start()
    {
        StartCoroutine(FindTargetRoutine());

    }

    private void Update()
    {
        if (debugMode)
        {
            if (distanceToTarget > towerAttacker.attackRange)
            {
                Debug.DrawLine(projectileSpawnpoint.transform.position, target.transform.position, Color.red);
            }
            else if (distanceToTarget <= towerAttacker.attackRange)
            {
                Debug.DrawLine(projectileSpawnpoint.transform.position, target.transform.position, Color.blue);
            }
        }
    }

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
}
