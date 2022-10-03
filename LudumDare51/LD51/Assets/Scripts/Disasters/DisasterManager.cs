using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisasterManager : MonoBehaviour
{
    public List<GameObject> disasterPrefabs;
    public Vector3 spawnPosition;
    public float disasterCooldown;
    public float disasterMissChanceAtWave0;
    public float disasterMissChanceAtWave20;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DisasterRoutine());
    }

    public void SpawnDisaster()
    {
        float finlDisasterMissChance = Mathf.Lerp(disasterMissChanceAtWave0, disasterMissChanceAtWave20, EnemyManager.Instance.wave);

        if (Random.value > finlDisasterMissChance)
        {
            GetRandomSpawnPos();

            Instantiate(
                disasterPrefabs[Random.Range(0, disasterPrefabs.Count)],
                spawnPosition,
                Quaternion.Euler(new Vector3(0f, Random.Range(0f, 360f), 0f)));
        }
    }

    public void GetRandomSpawnPos()
    {
        spawnPosition = Random.onUnitSphere * 30f;
        spawnPosition.y = 15f;
        //spawnPosition = spawnPosition.normalized * 30f;
    }

    public IEnumerator DisasterRoutine()
    {
        yield return new WaitForSeconds(disasterCooldown);

        while (PlayerResources.Instance.isAlive)
        {
            SpawnDisaster();
            yield return new WaitForSeconds(disasterCooldown);
        }
    }
}
