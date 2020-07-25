using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class City : MonoBehaviour
{
	[SerializeField] private CityDestroyedData cityDestroyedData;
	[SerializeField] private int currentHealth;
	[SerializeField] private Slider healthSlider;

	private UnityAction<string> BombHitEventListener;
	private UnityAction<string> AwardEventListener;

	private void Start()
    {
        SetHealth((int)healthSlider.maxValue);
        AddListeners();
    }

    private void AddListeners()
    {
        BombHitEventListener = new UnityAction<string>(OnCityHit);
        EventManager.StartListening("BOMB_HIT_DATA_EVENT", BombHitEventListener);
        AwardEventListener = new UnityAction<string>(onAwardReceived);
        EventManager.StartListening("AWARD_DATA_EVENT", AwardEventListener);
    }

	/// <summary>
	/// Callback called every time the city is hit.
	/// </summary>
	/// <param name="jsonVars"> Blade hit data.</param>
	private void OnCityHit(string jsonVars)
	{
		print("City: OnCityHit");

		BombHitData objVars = ScriptableObject.CreateInstance<BombHitData>();
		JsonUtility.FromJsonOverwrite(jsonVars, objVars);

		SetHealth(objVars.damage);

		UserModel.Instance.BombsHit++;
	}

	/// <summary>
	/// Callback called for every award given.
	/// </summary>
	/// <param name="jsonVars"> Award data, for example extra health.</param>
	private void onAwardReceived(string jsonVars)
	{
		print("City: onAwardReceived");

		AwardData objVars = ScriptableObject.CreateInstance<AwardData>();
		JsonUtility.FromJsonOverwrite(jsonVars, objVars);

		// Hanlde health awards only.
		if (objVars.subType == "health")
		{
			SetHealth(objVars.award);
		}
	}

	/// <summary>
	/// Update the health of the city.
	/// </summary>
	/// <param name="addAmount"> The amount of added health - can be negative in case of a hit or positve in case of a health booster.</param>
	private void SetHealth(int addAmount)
	{
		int newHealth = currentHealth + addAmount;
		newHealth = (int)Mathf.Clamp(newHealth, healthSlider.minValue, healthSlider.maxValue);
		currentHealth = newHealth;
		healthSlider.value = currentHealth;

		if (currentHealth <= healthSlider.minValue)
        {
			OnCityDestroyed();
        }
	}

	/// <summary>
	/// Handle the city being destroyed - health reaching its minimum value.
	/// </summary>
	private void OnCityDestroyed()
    {
		// Stop listening to "BOMB_HIT_DATA_EVENT".
		EventManager.StopListening("BOMB_HIT_DATA_EVENT", BombHitEventListener);

		// Stop listening to "AWARD_DATA_EVENT".
		EventManager.StopListening("AWARD_DATA_EVENT", AwardEventListener);

		string jsonVars = JsonUtility.ToJson(cityDestroyedData);

		// Dispatch an event for the GameManager "CITY_DESTROYED_EVENT".
		EventManager.TriggerEvent("CITY_DESTROYED_EVENT", jsonVars);
	}
}