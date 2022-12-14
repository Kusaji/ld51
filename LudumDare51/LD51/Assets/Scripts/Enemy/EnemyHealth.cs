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
    public float healthPerWaveExponent;

    [Header("Prefabs")]
    public GameObject bloodPrefab;

    [Header("Components")]
    public ParticleSystem hitParticles;

    public EnemyController enemyController;
    #endregion

    #region Unity Callbacks
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = maxHealth + Mathf.Pow(EnemyManager.Instance.wave, healthPerWaveExponent) * healthPerWave;
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
            EnemyManager.Instance.activeEnemiesScripts.Remove(enemyController);
            Instantiate(
                bloodPrefab,
                transform.position,
                Quaternion.Euler(90f, 0f, 0f));
            Destroy(gameObject);
        } else
        {
            if (hitParticles != null)
                hitParticles.Play();
        }
    }
    #endregion
}
