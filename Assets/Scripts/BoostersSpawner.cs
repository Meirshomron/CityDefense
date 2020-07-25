using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostersSpawner : MonoBehaviour 
{

	public string[] boosterTypes;

	[SerializeField] private float minSpawnOffset;
	[SerializeField] private float maxSpawnOffset;

	private float maxDelayBetweenBoosters;
	private float minDelayBetweenBoosters;

	private bool spawningEnabled;
	private float countdownTime;
	private Camera cam;

	void Start ()
	{
		cam = Camera.main;
	}

	void Update ()
	{
		if (spawningEnabled)
        {
			countdownTime -= Time.deltaTime;

			// Create the booster and start the timer for the next booster.
			if (countdownTime <= 0)
            {
				countdownTime = Random.Range(minDelayBetweenBoosters, maxDelayBetweenBoosters);

				// Spawn a random booster and auto destroy it after 5 sec.
				int boosterIndex = Random.Range(0, boosterTypes.Length);
				Vector3 spawnPoint = cam.ViewportToWorldPoint(new Vector3(Random.Range(minSpawnOffset, maxSpawnOffset), 1, 0));
				spawnPoint.z = 0;

				GameObject spawnedBooster = ObjectPooler.Instance.GetPooledObject(boosterTypes[boosterIndex], 5f);

				if (spawnedBooster != null)
				{
					spawnedBooster.transform.position = spawnPoint;
					spawnedBooster.transform.rotation = Quaternion.identity;
					spawnedBooster.SetActive(true);
				}
			}
		}
	}

	/// <summary>
	/// Update the bomb spawning parameters accoridng to the current wave's difficulty.
	/// </summary>
	public void SetCurrentWaveDifficulty(Wave wave)
	{
		maxDelayBetweenBoosters = wave.boosterMaxDelay;
		minDelayBetweenBoosters = wave.boosterMinDelay;
	}

	/// <summary>
	/// Start spawning boosters again.
	/// </summary>
	public void Resume()
	{
		spawningEnabled = true;
	}

	/// <summary>
	/// Stop spawning boosters, and remove all current active boosters.
	/// </summary>
	public void Pause()
	{
		spawningEnabled = false;
		for (int bombTypeIdx = 0; bombTypeIdx < boosterTypes.Length; bombTypeIdx++)
		{
			ObjectPooler.Instance.ReturnTypeToPool(boosterTypes[bombTypeIdx]);
		}
	}
}
