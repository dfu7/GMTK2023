using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSpawner : MonoBehaviour
{
    public GameObject slimePrefab;        // Prefab of the slime to spawn
    public int spawnCountOnStart = 5;     // Number of slimes to spawn on start
    public float spawnRadius = 5f;        // Radius of the spawning area
    public float spawnInterval = 3f;      // Time interval between spawns
    public float minScale = 0.5f;         // Minimum scale of the spawned slimes
    public float maxScale = 1.5f;         // Maximum scale of the spawned slimes

    private List<GameObject> activeSlimes = new List<GameObject>();    // List of currently active slimes

    private void Start()
    {
        // Spawn initial slimes
        for (int i = 0; i < spawnCountOnStart; i++)
        {
            SpawnSlime();
        }

        // Start the spawn coroutine
        StartCoroutine(SpawnSlimes());
    }

    private IEnumerator SpawnSlimes()
    {
        while (true)
        {
            // Wait until all the initial slimes have died
            yield return new WaitUntil(() => activeSlimes.Count == 0);

            // Spawn a new set of slimes
            for (int i = 0; i < spawnCountOnStart; i++)
            {
                SpawnSlime();
            }

            // Wait for the spawn interval
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnSlime()
    {
        // Generate a random position within the spawn radius
        Vector3 randomPosition = transform.position + Random.insideUnitSphere * spawnRadius;
        randomPosition.y = transform.position.y;

        // Spawn a slime at the random position
        GameObject slime = Instantiate(slimePrefab, randomPosition, Quaternion.identity);

        // Set a random scale for the spawned slime
        float scale = Random.Range(minScale, maxScale);
        slime.transform.localScale = new Vector3(scale, scale, scale);

        // Add the slime to the list of active slimes
        activeSlimes.Add(slime);

        // Subscribe to the slime's death event
        SlimeHealth slimeHealth = slime.GetComponent<SlimeHealth>();
        if (slimeHealth != null)
        {
            slimeHealth.OnDeath += HandleSlimeDeath;
        }
    }

    private void HandleSlimeDeath(GameObject slime)
    {
        // Remove the slime from the list of active slimes
        activeSlimes.Remove(slime);
    }
}