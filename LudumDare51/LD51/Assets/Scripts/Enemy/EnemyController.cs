using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public EnemyHealth health;
    public NavMeshAgent agent;
    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        agent.Warp(transform.position);

        target = GetRandomTarget();
        
        if (target != null)
        {
            agent.SetDestination(target.transform.position);
        }
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
        return PlayerResources.instance.structures[Random.Range(0, PlayerResources.instance.structures.Count)];
    }
}
