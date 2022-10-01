using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bastion : Structure
{
    public override void DealDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0 && isAlive)
        {
            isAlive = false;

            //Remove from structurel ist
            if (PlayerStructures.instance.structures.Contains(gameObject))
            {
                PlayerStructures.instance.structures.Remove(gameObject);
            }

            //Explosion / Death prefab
            Instantiate(
                explosionPrefab,
                transform.position + new Vector3(0.0f, 0.25f, 0.0f),
                Quaternion.Euler(new Vector3(-90f, 0.0f, 0.0f)));

            Camera.main.GetComponent<CameraController>().ShakeCameraImpulse(Random.onUnitSphere, 10f);

            PlayerResources.Instance.isAlive = false;

            GameObject.Destroy(gameObject);
        }
    }
}