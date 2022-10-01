using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttacker : MonoBehaviour
{
    public TowerTargeter targeter;
    public Structure structure;
    public float attackRange;
    public float attackDamage;
    public float attackDelay;

    private void Start()
    {
        StartCoroutine(AttackRoutine());   
    }

    public void DealDamage()
    {
        targeter.target.GetComponent<EnemyHealth>().TakeDamage(attackDamage);
    }

    public IEnumerator AttackRoutine()
    {
        while (structure.isAlive)
        {
            if (targeter.target != null)
            {
                if (targeter.distanceToTarget <= attackRange)
                {
                    targeter.target.GetComponent<EnemyHealth>().TakeDamage(attackDamage);
                    Debug.DrawLine(targeter.projectileSpawnpoint.transform.position, targeter.target.transform.position, Color.red, 0.35f);
                    yield return new WaitForSeconds(attackDelay / structure.effectivenessExponent);
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
