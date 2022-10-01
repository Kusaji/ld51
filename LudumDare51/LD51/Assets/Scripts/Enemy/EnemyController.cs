using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Components")]
    public EnemyHealth health;
    public NavMeshAgent agent;

    public GameObject target;
    public Structure targetStructure;

    public float attackDamage;
    public float attackRange;
    public float attackDelay;
    public float distanceToTarget;

    public bool isAttacking;

    // Start is called before the first frame update
    void Start()
    {
        agent.Warp(transform.position);


        StartCoroutine(FindTargetRoutine());
    }

    private void Update()
    {

    }

    //todo
    //Iterate over structure list and find closest with priority.
    public GameObject GetRandomTarget()
    {
        if (PlayerStructures.instance.structures.Count > 0)
        {
            return PlayerStructures.instance.structures[Random.Range(0, PlayerStructures.instance.structures.Count)];
        }
        else
        {
            return null;
        }
    }

    public IEnumerator FindTargetRoutine()
    {
        yield return new WaitForSeconds(0.1f);

        while (health.isAlive)
        {
            if (PlayerStructures.instance.structures.Count > 0)
            {
                var closestTower = PlayerStructures.instance.structures[0];
                var closestTowerDistance = Mathf.Infinity;

                for (int i = 0; i < PlayerStructures.instance.structures.Count; i++)
                {
                    if (Vector3.Distance(transform.position, PlayerStructures.instance.structures[i].transform.position) < closestTowerDistance)
                    {
                        closestTowerDistance = Vector3.Distance(transform.position, PlayerStructures.instance.structures[i].transform.position);
                        closestTower = PlayerStructures.instance.structures[i];
                    }
                }
                target = closestTower;
                targetStructure = closestTower.GetComponent<Structure>();
                agent.SetDestination(target.transform.position);
                distanceToTarget = closestTowerDistance;
                StartCoroutine(CalculateDistance());
            }
            yield return new WaitForSeconds(0.25f);
        }
    }




    public IEnumerator CalculateDistance()
    {
        while (health.isAlive)
        {
            if (target != null)
            {
                distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

                if (distanceToTarget <= attackRange && !isAttacking)
                {
                    StartCoroutine(AttackRoutine());
                    isAttacking = true;
                }
            }
            yield return new WaitForSeconds(0.25f);
        }
    }

    public IEnumerator AttackRoutine()
    {
        while (health.isAlive != false)
        {
            if (distanceToTarget <= attackRange && target != null)
            {
                targetStructure.DealDamage(attackDamage);
            }
            else
            {
                isAttacking = false;
                yield break;
            }
            yield return new WaitForSeconds(attackDelay);
        }
    }
}
