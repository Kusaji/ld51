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

    // Start is called before the first frame update
    void Start()
    {
        agent.Warp(transform.position);

        StartCoroutine(FindTargetRoutine());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            target = GetRandomTarget();

            if (target != null)
            {
                agent.SetDestination(target.transform.position);
            }
        }
    }

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
        while (health.isAlive)
        {
            GetRandomTarget();

            if (target != null)
            {
                agent.SetDestination(target.transform.position);
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
