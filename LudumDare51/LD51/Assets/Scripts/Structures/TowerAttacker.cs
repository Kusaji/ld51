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

    public bool instantAttack;
    public GameObject attackProjectilePrefab;

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
                    if (instantAttack)
                    {
                        targeter.target.GetComponent<EnemyHealth>().TakeDamage(attackDamage);
                        Debug.DrawLine(targeter.projectileSpawnpoint.transform.position, targeter.target.transform.position, Color.red, 0.35f);
                    }
                    else if (!instantAttack)
                    {
                        var projectile = Instantiate(
                            attackProjectilePrefab,
                            targeter.projectileSpawnpoint.transform.position,
                            Quaternion.identity);
                        var projectileSettings = projectile.GetComponent<SingleTargetProjectile>();
                        projectileSettings.target = targeter.target;
                        projectileSettings.damage = attackDamage;
                    }

                    yield return new WaitForSeconds(attackDelay / structure.effectivenessExponent);
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
