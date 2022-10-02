using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/StructureBalanceNumbersSO", order = 1)]
public class StructureBalanceNumbersSO : ScriptableObject
{
    public int minimumPopulationToFunction;

    public float maxHealth;
    public float maxBuildProgress;
    public float incomingDamageDefenseMod;
}
