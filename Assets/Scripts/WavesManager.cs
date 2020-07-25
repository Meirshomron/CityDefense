using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngineInternal;

[System.Serializable]
public class Wave
{
	public float bombMinDelay;
	public float bombMaxDelay;
	public float boosterMinDelay;
	public float boosterMaxDelay;
}

/// <summary>
/// Manage the game waves.
/// </summary>
public class WavesManager : MonoBehaviour 
{
	// Main prefabs to manage.
	[SerializeField] private GameObject bombSpawnerPrefab;
	[SerializeField] private GameObject boostersSpawnerPrefab;
	[SerializeField] private GameObject bladePrefab;

	// Wave params.
	[SerializeField] private float waveDuration;
	[SerializeField] private float waveStartPopupDuration;
	[SerializeField] private float thresholdForNextDifficulty;
	[SerializeField] private float thresholdForPreviousDifficulty;
	[SerializeField] private Slider waveSlider;

	[SerializeField] private List<Wave> WavesDifficultyList;

	// Private fields.
	private BombSpawner bombSpawner;
	private BoostersSpawner boostersSpawner;
	private Blade blade;
	private float currentCountdown;
	private bool waveActive;

	private int waveBombsHit;
	private int waveBombsSliced;
	private int waveDifficultyIdx;

	public bool WaveActive
    {
        get
        {
            return waveActive;
        }

        set
        {
            waveActive = value;
        }
    }

	/// <summary>
	/// Initialize all the spawners and fields.
	/// Show the first wave popup.
	/// </summary>
    public void Init()
    {
		print("WavesManager: Init");

		currentCountdown = waveDuration;
		waveSlider.maxValue = waveDuration;
		waveSlider.minValue = 0f;
		waveSlider.value = waveSlider.maxValue;

		UserModel.Instance.CurrentWaveIndex = 1;

		bombSpawner = Instantiate(bombSpawnerPrefab).GetComponent<BombSpawner>();
		boostersSpawner = Instantiate(boostersSpawnerPrefab).GetComponent<BoostersSpawner>();
		blade = Instantiate(bladePrefab).GetComponent<Blade>();
		blade.gameObject.SetActive(false);

		waveBombsHit = 0;
		waveBombsSliced = 0;
		waveDifficultyIdx = 0;

		// Show the first wave start popup.
		ShowWavePopup();
	}

	void Update () 
	{
		// Wave countdown.
		if (waveActive)
        {
			currentCountdown -= Time.deltaTime;
			if (currentCountdown >= waveSlider.minValue)
            {
				waveSlider.value = currentCountdown;
			}
			else
            {
				OnWaveEnded();
			}
		}
	}

	/// <summary>
	/// Wave countdown ended, show the wave start popup of the next wave.
	/// </summary>
	private void OnWaveEnded()
    {
		print("WavesManager: OnWaveEnded");

		// Stop the game activities.
		Pause();

		// Show the next wave's start popup.
		UserModel.Instance.CurrentWaveIndex++;
		ShowWavePopup();
	}

	private void ShowWavePopup()
    {
		// Show the WaveStart popup.
		PopupsManager.Instance.ShowPopup("WaveStart", waveStartPopupDuration);

		StartCoroutine(OnWaveStartPopupClosed(waveStartPopupDuration));
	}

	/// <summary>
	/// Prepare the data of the next wave and resume the game activities.
	/// </summary>
	/// <param name="popupDuration"> Duration of the WAveStart popup. </param>
	IEnumerator OnWaveStartPopupClosed(float popupDuration)
	{
		yield return new WaitForSeconds(popupDuration);
		currentCountdown = waveSlider.maxValue;

		// Data of the last completed wave.
		waveBombsHit = UserModel.Instance.BombsHit - waveBombsHit;
		waveBombsSliced = UserModel.Instance.BombsSliced - waveBombsSliced;

		// Set the starting wave's difficulty.
		int waveDifficultyIdx = CalcCurrentWaveDifficulty();
		bombSpawner.SetCurrentWaveDifficulty(WavesDifficultyList[waveDifficultyIdx]);
		boostersSpawner.SetCurrentWaveDifficulty(WavesDifficultyList[waveDifficultyIdx]);

		Resume();
	}

	/// <summary>
	/// Calculate the level of difficulty of the current wave starting, according to the level of success in the previously completed wave.
	/// </summary>
	/// <returns> Index in the WavesDifficultyList of the difficulty level for the current wave. </returns>
	private int CalcCurrentWaveDifficulty()
    {
		int waveTotalBombs = waveBombsHit + waveBombsSliced;
		if (waveTotalBombs > 0)
        {
			float waveSuccessRate = ((float)waveBombsHit) / waveTotalBombs;

			if (waveSuccessRate < thresholdForNextDifficulty)
            {
				waveDifficultyIdx++;
			}
			else if (waveSuccessRate > thresholdForPreviousDifficulty)
            {
				waveDifficultyIdx--;
			}

			waveDifficultyIdx = Mathf.Clamp(waveDifficultyIdx, 0, (WavesDifficultyList.Count - 1));
		}

		print("waveDifficultyIdx = " + waveDifficultyIdx);
		return waveDifficultyIdx;
	}

	/// <summary>
	/// Stop game activities.
	/// </summary>
	public void Pause()
    {
		print("WavesManager: Pause");

		waveActive = false;
		bombSpawner.Pause();
		boostersSpawner.Pause();
		blade.gameObject.SetActive(false);
	}

	/// <summary>
	/// Resume the game activities.
	/// </summary>
	public void Resume()
	{
		print("WavesManager: Resume");

		waveActive = true;
		bombSpawner.Resume();
		boostersSpawner.Resume();
		blade.gameObject.SetActive(true);
	}
}
