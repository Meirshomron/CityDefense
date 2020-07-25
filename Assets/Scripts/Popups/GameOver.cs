using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Popup. handle functionality of the game over popup.
/// </summary>
public class GameOver : MonoBehaviour 
{

	[SerializeField] private Text gameOverTxt;

	/// <summary>
	/// Init function called when the popupManager enables this popup.
	/// </summary>
	public void OnEnable()
    {
		print("GameOver: OnEnable");

		SetTxts();
	}

	/// <summary>
	/// Set the dynamic texts.
	/// </summary>
	private void SetTxts()
    {
		print("GameOver: SetTxts");

		gameOverTxt.text = "GOOD GAME! \n\n Score: " + UserModel.Instance.CurrentScore + "\n Bombs Sliced: " + UserModel.Instance.BombsSliced + "\n Bombs Missed: " + UserModel.Instance.BombsHit;
	}

	/// <summary>
	/// Callback called once the 'Restart' button is pressed.
	/// Restart the scene.
	/// </summary>
	public void OnRestartPressed()
    {
		print("GameOver: OnRestartPressed");

		// TODO: post an event for the GameManager to open the game gracefully. 
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	/// <summary>
	/// Callback called once the 'Exit' button was pressed.
	/// Exit the app.
	/// </summary>
	public void OnExitPressed()
	{
		print("GameOver: OnExitPressed");

		// TODO: post an event for the GameManager to close the game gracefully. 
		Application.Quit();
	}
}
