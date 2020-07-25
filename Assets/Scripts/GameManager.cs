using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	[SerializeField] private Text scoreTxt;
	[SerializeField] private WavesManager wavesManager;

	private UnityAction<string> AwardEventListener;
	private UnityAction<string> CityDestroyedEventListener;

	void Start ()
	{
		UserModel.Instance.CurrentScore = 0;
		SetScoreUI();

		AddListeners();

		wavesManager.Init();
	}

    private void AddListeners()
    {
		print("GM: AddListeners");

		AwardEventListener = new UnityAction<string>(onAwardReceived);
		CityDestroyedEventListener = new UnityAction<string>(onCityDestroyed);
		EventManager.StartListening("AWARD_DATA_EVENT", AwardEventListener);
		EventManager.StartListening("CITY_DESTROYED_EVENT", CityDestroyedEventListener);
	}

	/// <summary>
	/// Callback called when a city is destroyed.
	/// </summary>
	/// <param name="jsonVars"> City destroyed params. </param>
	private void onCityDestroyed(string jsonVars)
    {
		print("GM: onCityDestroyed");
		GameOver();
	}

	private void GameOver()
    {
		// Stop the game activities.
		wavesManager.Pause();

		// Show the GameOver popup.
		PopupsManager.Instance.ShowPopup("GameOver");
	}

	/// <summary>
	/// Callback called for every award given.
	/// </summary>
	/// <param name="jsonVars"> Award data. </param>
	private void onAwardReceived(string jsonVars)
    {
		print("GM:onAwardReceived");

		AwardData objVars = ScriptableObject.CreateInstance<AwardData>();
		JsonUtility.FromJsonOverwrite(jsonVars, objVars);

		// Hanlde score awards only.
		if ((objVars.type == "bomb") || (objVars.type == "booster" && objVars.subType == "score"))
        {
			UserModel.Instance.CurrentScore += objVars.award;
			SetScoreUI();
		}
	}
	private void SetScoreUI()
    {
		scoreTxt.text = "Score: " + UserModel.Instance.CurrentScore;
	}
}
