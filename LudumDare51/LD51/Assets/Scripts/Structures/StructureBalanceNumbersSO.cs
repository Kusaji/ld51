using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/StructureBalanceNumbersSO", order = 1)]
public class StructureBalanceNumbersSO : ScriptableObject
{
    [Header("Population")]
    public int minimumPopulationToFunction;
    public float effectivenessExponent;
    public float rangeEffectivenessExponent;

    [Header("Defense")]
    public float maxHealth;
    public float incomingDamageDefenseMod;

    [Header("Build Time")]
    public float maxBuildProgress;
    
    
}
