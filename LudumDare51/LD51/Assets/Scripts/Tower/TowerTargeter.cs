using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerTargeter : MonoBehaviour
{
    public GameObject target;
    public float distanceToTarget;

    private void Start()
    {
        StartCoroutine(FindTargetRoutine());
    }

    public void FindNewTarget()
    {
        if (EnemyManager.Instance.activeEnemies.Count > 0)
        {
            var closestEnemy = EnemyManager.Instance.activeEnemies[0];
            float closestEnemyDistance = Mathf.Infinity;

            for (int i = 0; i < EnemyManager.Instance.activeEnemies.Count; i++)
            {
                closestEnemyDistance = Vector3.Distance(transform.position, EnemyManager.Instance.activeEnemies[i].transform.position);
                closestEnemy = EnemyManager.Instance.activeEnemies[i];
            }

            target = closestEnemy;
            distanceToTarget = closestEnemyDistance;
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
