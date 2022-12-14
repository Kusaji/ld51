using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatballHitController : MonoBehaviour
{
    public MeatballController meatball;
    public float screenShakeStrength;
    public float screenShakeReductionAmount;

    private void OnTriggerEnter(Collider other)
    {
        //Start having meatball shrink and slow down.
        if (!other.gameObject.CompareTag("Untagged") && !meatball.hasHitGround)
        {
            meatball.hasHitGround = true;
            meatball.StartMeatballRoutine();
            meatball.audioController.PlayOneShot(1, 0.60f);

            if (meatball.meatballSize == 7)
            {
                screenShakeStrength = 150f;
                Camera.main.GetComponent<CameraController>().ShakeCameraImpulse(Random.onUnitSphere, screenShakeStrength);
            }
            else if (meatball.meatballSize == 3)
            {
                screenShakeStrength = 75f;
                Camera.main.GetComponent<CameraController>().ShakeCameraImpulse(Random.onUnitSphere, screenShakeStrength);
            }
            else
            {
                screenShakeStrength = 30f;
                Camera.main.GetComponent<CameraController>().ShakeCameraImpulse(Random.onUnitSphere, screenShakeStrength);
            }
        }

        //Camera shake, hell yeah.
        if (!other.gameObject.CompareTag("Untagged") && meatball.hasHitGround)
        {
            Camera.main.GetComponent<CameraController>().ShakeCameraImpulse(Random.onUnitSphere, screenShakeStrength);
            meatball.audioController.PlayOneShot(3, 0.20f);

            if (screenShakeStrength - screenShakeReductionAmount > 0)
            {
                screenShakeStrength -= screenShakeReductionAmount;
            }
        }

        //Damage to enemies
        if (other.gameObject.CompareTag("Enemy"))
        {
            var enemyHealth = other.gameObject.GetComponent<EnemyHealth>();

            enemyHealth.TakeDamage(meatball.enemyDamage);
        }
        
        //Damage to structures
        if (other.gameObject.CompareTag("Structure"))
        {
            var structureHealth = other.gameObject.GetComponent<StructureHitbox>().structure;
            structureHealth.DealDamage(meatball.currentStructureDamage);
        }
    }
}
