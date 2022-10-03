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
    public float healthPerWave;

    [Header("Prefabs")]
    public GameObject bloodPrefab;


    #endregion

    #region Unity Callbacks
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = maxHealth + EnemyManager.Instance.wave * healthPerWave;
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
            Instantiate(
                bloodPrefab,
                transform.position,
                Quaternion.Euler(90f, 0f, 0f));
            Destroy(gameObject);
        }
    }
    #endregion
}
