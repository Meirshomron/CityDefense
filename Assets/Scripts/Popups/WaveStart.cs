using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Popup. handle functionality of the wave start popup.
/// </summary>
public class WaveStart : MonoBehaviour 
{
	[SerializeField] private Text waveTxt;

	/// <summary>
	/// Init function called when the popupManager enables this popup.
	/// </summary>
	public void OnEnable()
	{
		SetTxts();
	}

	/// <summary>
	/// Set the dynamic texts.
	/// </summary>
	private void SetTxts()
	{
		print("WaveStart: SetTxts");

		waveTxt.text = "Starting Wave " + UserModel.Instance.CurrentWaveIndex;
	}
}
