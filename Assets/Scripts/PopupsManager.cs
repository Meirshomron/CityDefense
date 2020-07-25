using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup
{
	public string name;
	public GameObject popupObj;
	public float autoClose = -1;
}

/// <summary>
/// Singleton to manage the showing of popups.
/// </summary>
public class PopupsManager : MonoBehaviour
{
	[SerializeField] private Canvas popupsUI;

	// Currently being shown popup.
	private Popup currentPopup;

	// Queue of all the popups waiting to be shown.
	private Queue<Popup> popupsQueue;

	private static PopupsManager _instance;

	public static PopupsManager Instance
	{
		get { return _instance; }
	}

	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
			return;
		}

		_instance = this;
	}

	void Start ()
	{
		popupsUI.gameObject.SetActive(false);
		popupsQueue = new Queue<Popup>();
		currentPopup = null;
	}

	/// <summary>
	/// Inner actual function to show a popup.
	/// </summary>
	/// <param name="popup"> The popup to show, expected to be the front of the queue. </param>
	private void ShowNextPopup(Popup popup)
	{
		print("ShowNextPopup");

		currentPopup = popup;
		Popup frontPopup = popupsQueue.Dequeue();

		if (frontPopup != currentPopup)
        {
			Debug.LogError("Trying to actually show a popup that isn't the front of the queue.");
        }

		popupsUI.gameObject.SetActive(true);
		popup.popupObj.SetActive(true);
		if (popup.autoClose > 0)
        {
			StartCoroutine(AutoClose(popup));
		}
	}

	/// <summary>
	/// The interface function for showing a popup.
	/// </summary>
	/// <param name="popupName"> The name of the popup, this must be also the name of the gameObject under the popupsUI. </param>
	/// <param name="autoClose"> The time till auto closing the popup, by default it's -1 and wont auto close. </param>
	public void ShowPopup(string popupName, float autoClose = -1)
	{
		print("ShowPopup");

		Popup popup = CreatePopup(popupName, autoClose);
		popupsQueue.Enqueue(popup);

		// If no popup is currently showing - show this popup, otherwise it'll be the popupsQueue till it's his turn.
		if (currentPopup == null)
        {
			ShowNextPopup(popup);
		}
	}

	/// <summary>
	/// Creates a popup.
	/// </summary>
	/// <param name="popupName"> The name of the popup, this must be also the name of the gameObject under the popupsUI. </param>
	/// <param name="autoClose"> The time till auto closing the popup, by default it's -1. </param>
	/// <returns></returns>
	private Popup CreatePopup(string popupName, float autoClose)
    {
        Popup popup = new Popup
        {
            popupObj = popupsUI.transform.Find(popupName).gameObject,
            name = popupName,
			autoClose = autoClose
	};

        return popup;
	}

	/// <summary>
	/// Auto close the current popup.
	/// </summary>
	IEnumerator AutoClose(Popup popup)
	{
		yield return new WaitForSeconds(popup.autoClose);

		ClosePopup(popup);
	}

	/// <summary>
	/// Every popup that is closed must call this function.
	/// Closes the given popup and shows then next popup.
	/// </summary>
	/// <param name="popup"> The given current popup that was shown and now closed. </param>
	public void ClosePopup(Popup popup)
    {
		print("ClosePopup");

		popup.popupObj.SetActive(false);
		currentPopup = null;

		// No more popups.
		if (popupsQueue.Count == 0)
        {
			popupsUI.gameObject.SetActive(false);
			currentPopup = null;
		}
		// Show next popup.
		else
		{
			ShowNextPopup(popupsQueue.Peek());
		}
	}
}
