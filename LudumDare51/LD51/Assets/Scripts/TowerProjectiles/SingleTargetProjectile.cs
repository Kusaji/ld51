using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTargetProjectile : MonoBehaviour
{
    public GameObject target;
    public float damage;
    public Rigidbody rb;
    public float moveSpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == target)
        {
            other.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
