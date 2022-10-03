using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy Health, with methods for taking damage.
/// </summary>
public class EnemyHealth : MonoBehaviour
{
    #region Variables
    [Header("Health")]
    public bool isAlive;
    public float maxHealth;
    public float currentHealth;
    public GameObject deathPrefab;

    #endregion

    #region Unity Callbacks
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        isAlive = true;
    }
    #endregion

    #region Methods
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            EnemyManager.Instance.activeEnemies.Remove(gameObject);
            if (deathPrefab != null)
            {
                Instantiate(deathPrefab, transform.position,Quaternion.Euler(new Vector3(90,0,0)));
            }

            Destroy(gameObject);
        }
    }
    #endregion
}
