using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatballController : MonoBehaviour
{
    #region Variables

    [Header("Assigned Stats")]
    public float moveSpeed;
    public float damage;
    public float lifeLength;
    public float meatballSize;

    [Header("Runtime Stats | Do not assign")]
    public bool hasHitGround;


    [Header("References / Compoennts")]
    public Rigidbody rb;

    #endregion

    #region Unity Callbacks

    private void Start()
    {
        //Shape the delicious meatball
        transform.localScale = new Vector3(meatballSize, meatballSize, meatballSize);    
    }

    private void FixedUpdate()
    {
        rb.AddForce(Vector3.forward * moveSpeed * Time.deltaTime);
    }
    #endregion

    #region Methods

    public void StartMeatballRoutine()
    {
        StartCoroutine(MeatballRoutine());
        Destroy(gameObject, lifeLength);
    }

    #endregion

    #region Coroutines
    /// <summary>
    /// Reduce speed and size of meatball over lifespan.
    /// </summary>
    /// <returns></returns>
    public IEnumerator MeatballRoutine()
    {
        while (moveSpeed > 0)
        {
            moveSpeed -= 4f;
            meatballSize -= 0.030f;
            transform.localScale = new Vector3(meatballSize, meatballSize, meatballSize);
            yield return new WaitForSeconds(0.1f);
        }
    }

    #endregion
}
