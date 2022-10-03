using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatballController : MonoBehaviour
{
    #region Variables

    [Header("Assigned Stats")]
    public float moveSpeed;
    public float enemyDamage;
    
    public float minimumStructureDamage;
    public float maxStructureDamage;
    public float currentStructureDamage;

    public float lifeLength;
    public float meatballSize;

    [Header("Runtime Stats | Do not assign")]
    public bool hasHitGround;
    public Vector3 randomDirection;


    [Header("References / Compoennts")]
    public Rigidbody rb;
    public MeatballHitController meatballHit;
    public AudioController audioController;

    #endregion

    #region Unity Callbacks

    private void Start()
    {
        //Shape the delicious meatball
        MakeRandomMeatball();
        transform.localScale = new Vector3(meatballSize, meatballSize, meatballSize);

        transform.Translate(Vector3.up * (meatballSize), Space.Self);

        audioController.PlayOneShot(0, 0.60f);
        audioController.PlayOneShot(2, 0.20f);

        PickRandomDirection();
    }

    private void FixedUpdate()
    {
        rb.AddForce(randomDirection * moveSpeed * Time.deltaTime);
    }
    #endregion

    #region Methods

    public void StartMeatballRoutine()
    {
        StartCoroutine(MeatballRoutine());
        Destroy(gameObject, lifeLength);
    }

    public void PickRandomDirection ()
    {
        int selectedDirection = Random.Range(0, 4);

        switch (selectedDirection)
        {
            case 0:
                randomDirection = new Vector3(0f, 0f, 1f);
                break;
            case 1:
                randomDirection = new Vector3(0f, 0f, -1f);
                break;
            case 2:
                randomDirection = new Vector3(1f, 0f, 0f);
                break;
            case 3:
                randomDirection = new Vector3(-1f, 0f, 0f);
                break;
        }
    }

    public void MakeRandomMeatball()
    {
        //If we roll a 15 damage meatball, it is catastrophic potentially run ending.
        int randomStuctureDamage = (int)Random.Range(minimumStructureDamage, maxStructureDamage + 1);
        currentStructureDamage = randomStuctureDamage;

        //Mega Meatball
        if (currentStructureDamage == 15)
        {
            meatballSize = 7f;
            Debug.Log($"Meatball Size : {meatballSize}");
        }
        else if (currentStructureDamage <= 14 && currentStructureDamage >= 6f)
        {
            meatballSize = 3f;
            Debug.Log($"Meatball Size : {meatballSize}");
        }
        else if (currentStructureDamage <= 5f) 
        {
            meatballSize = 1f;
            Debug.Log($"Meatball Size : {meatballSize}");
        }
    }

    #endregion

    #region Coroutines
    /// <summary>
    /// Reduce speed and size of meatball over lifespan.
    /// </summary>
    /// <returns></returns>
    public IEnumerator MeatballRoutine()
    {
        while (gameObject)
        {
            if (moveSpeed > 0)
            {
                moveSpeed -= 4f;
            }
            if (meatballSize > 0.1f)
            {
                meatballSize -= 0.030f;
                transform.localScale = new Vector3(meatballSize, meatballSize, meatballSize);
                meatballHit.transform.localScale = new Vector3(meatballSize, meatballSize, meatballSize);
            }
            else if (meatballSize < 0.1f)
            {
                Destroy(gameObject);
            }
            yield return new WaitForSeconds(0.1f);
        }

    }
    #endregion
}
