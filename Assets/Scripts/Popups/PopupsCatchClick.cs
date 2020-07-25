using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Layer controlled by the popupsManager to disable clicking behind a popup that is being shown.
/// </summary>
public class PopupsCatchClick : MonoBehaviour 
{

	/// <summary>
	/// Catch the clicks over this layer that cover the full screen.
	/// </summary>
	void Update()
	{
		// Check if the left mouse button was clicked
		if (Input.GetMouseButtonDown(0))
		{
			// Check if the mouse was clicked over a UI element
			if (EventSystem.current.IsPointerOverGameObject())
			{
				print("Clicked on the UI");
			}
		}
	}
}
