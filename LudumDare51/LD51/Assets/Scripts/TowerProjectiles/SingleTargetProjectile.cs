using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTargetProjectile : MonoBehaviour
{
    public GameObject target;
    public float damage;

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target.transform.position);
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
