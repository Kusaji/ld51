using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles generation and allocation of player resources.
/// </summary>
public class PlayerResources : MonoBehaviour
{
    #region Variables
    [Header("Singleton")]
    public static PlayerResources Instance;

    [Header("Player Status")]
    public bool isAlive;

    [Header("Resources")]
    public int population;
    public int units;
    public int magicRunes;

    public GameObject gameOverGO;

    [HideInInspector]
    public int overallPopulation;
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        Instance = this;
        isAlive = true;
        overallPopulation = population;
    }
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
            population = 999999;
    }
#endif
#endregion

    #region Methods

    public void AddPopulation(int amount)
    {
        population += amount;        
    }
    public void TrackPopulationGeneration(int amount)
    {
        overallPopulation += amount;
    }
    public void TrackPopulationDestruction(int amount)
    {
        overallPopulation += amount;
    }

    public void SpendPopulation(int amount)
    {
        if (population >= amount)
        {
            population -= amount;
        }
        else
        {
            //Debug.Log("Not enough population.");
        }
    }
    public void GameOver()
    {
        isAlive = false;
        gameOverGO.SetActive(true);
    }
#endregion
}
