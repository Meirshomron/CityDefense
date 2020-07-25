using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BombSpawner : MonoBehaviour
{
    public string[] bombTypes;
    [SerializeField] private int totalSpawnPoints;
    [SerializeField] private Vector3[] spawnPoints;

    private bool spawningEnabled;
    private int currentWaveIdx;
    private Camera cam;

    private float maxDelayBetweenBombs;
    private float minDelayBetweenBombs;

    void Start () 
    {
        // Defualt values.
        maxDelayBetweenBombs = 2f;
        minDelayBetweenBombs = 1f;

        cam = Camera.main;
        CreateSpawnPoints();
	}

    /// <summary>
    /// Create the spawn points anchored to the middle-left with an offset on the y-axis.
    /// </summary>
    private void CreateSpawnPoints()
    {
        for (int i = 0; i < totalSpawnPoints; i++)
        {
            spawnPoints[i] = new Vector3(0, 0.3f + i * 0.1f, 0);
            spawnPoints[i] = cam.ViewportToWorldPoint(spawnPoints[i]);
            spawnPoints[i].z = 0;
        }
    }

    /// <summary>
    /// Spawn a bomb.
    /// </summary>
    IEnumerator SpawnBombs()
    {
        while (spawningEnabled)
        {
            // Spawn a random bomb from the list of bombs every random delay time in range.
            float delay = Random.Range(minDelayBetweenBombs, maxDelayBetweenBombs);
            yield return new WaitForSeconds(delay);

            if (spawningEnabled)
            {
                int spawnIndex = Random.Range(0, spawnPoints.Length);
                int bombIndex = Random.Range(0, bombTypes.Length);
                Vector3 spawnPoint = spawnPoints[spawnIndex];

                GameObject spawnedBomb = ObjectPooler.Instance.GetPooledObject(bombTypes[bombIndex], 5f);

                if (spawnedBomb != null)
                {
                    spawnedBomb.transform.position = spawnPoint;
                    spawnedBomb.transform.rotation = Quaternion.identity;
                    spawnedBomb.SetActive(true);
                    UserModel.Instance.BombsSpawned++;
                }
            }
        }
    }

    /// <summary>
    /// Update the bomb spawning parameters accoridng to the current wave's difficulty.
    /// </summary>
    public void SetCurrentWaveDifficulty(Wave wave)
    {
        maxDelayBetweenBombs = wave.bombMaxDelay;
        minDelayBetweenBombs = wave.bombMinDelay;
    }

    /// <summary>
    /// Start spawning bombs again.
    /// </summary>
    public void Resume()
    {
        if (!spawningEnabled)
        {
            spawningEnabled = true;
            StartCoroutine(SpawnBombs());
        }
    }

    /// <summary>
    /// Stop spawning bombs, and remove all current active bombs.
    /// </summary>
    public void Pause()
    {
        spawningEnabled = false;
        for (int bombTypeIdx = 0; bombTypeIdx < bombTypes.Length; bombTypeIdx++)
        {
            ObjectPooler.Instance.ReturnTypeToPool(bombTypes[bombTypeIdx]);
        }
    }
}
