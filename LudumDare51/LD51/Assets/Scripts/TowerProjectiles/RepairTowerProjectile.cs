using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairTowerProjectile : MonoBehaviour
{
    public GameObject target;
    public GameObject healEffectPrefab;
    public float moveSpeed;
    private Rigidbody rb;
    public float distanceToTarget;
    public bool hitTarget;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        hitTarget = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            transform.LookAt(target.transform.position);
        }
        else if (target == null)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.forward * moveSpeed;

        if (target != null)
        {
            distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

            if (distanceToTarget < 0.5f && !hitTarget)
            {
                hitTarget = true;

                Instantiate(
                    healEffectPrefab,
                    target.transform.position,
                    Quaternion.identity);

                Destroy(gameObject);
            }
        }
        else if (target == null)
        {
            Destroy(gameObject);
        }
    }
}
