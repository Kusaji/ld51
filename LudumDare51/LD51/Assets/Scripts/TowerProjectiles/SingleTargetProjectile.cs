using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behavior for tower single target projectile.
/// </summary>
public class SingleTargetProjectile : MonoBehaviour
{
    #region Variables
    [Header("Stats")]
    public float moveSpeed;

    [Header("Runtime Stats | Do not Set")]
    public float damage;
    public GameObject target;
    public Rigidbody rb;

    public GameObject collisionEffectPrefab;
    public Vector3 collisionEffectEulerAngles;
    #endregion

    #region Unity Callbacks
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
    #endregion

    #region Collision

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == target)
        {
            other.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
            if (collisionEffectPrefab != null)
                Instantiate(collisionEffectPrefab, other.gameObject.transform.position, Quaternion.Euler(collisionEffectEulerAngles));
            Destroy(gameObject);
        }
    }
    #endregion
}
