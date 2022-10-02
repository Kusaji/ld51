using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behavior to automatically destroy an instantiated prefab.
/// </summary>
public class PrefabDestroyer : MonoBehaviour
{
    #region Variables
    public float destroyTime;
    #endregion

    #region Unity Callbacks
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }
    #endregion
}
