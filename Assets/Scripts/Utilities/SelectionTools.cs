using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

/// <summary>
/// Selection tools.
/// </summary>
public static class SelectionTools {

	/// <summary>
	/// Tells if the user has clicked this frame and if that click was not
	/// registered onto the UI event system.
	/// </summary>
	/// <returns><c>true</c>, if outside GUI was clicked, <c>false</c> otherwise.</returns>
	public static bool ClickOutsideGUI() {
		//return (Input.GetMouseButtonDown (0) && !EventSystem.current.IsPointerOverGameObject ());
		return (Input.GetMouseButtonDown (0) && EventSystem.current.currentSelectedGameObject == null);
	}

	/// <summary>
	/// Tells if the user has clicked this frame and if that click was
	/// registered onto the UI event system.
	/// </summary>
	/// <returns><c>true</c>, if inside GUI was clicked, <c>false</c> otherwise.</returns>
	public static bool ClickInsideGUI() {
		//return (Input.GetMouseButtonDown (0) && EventSystem.current.IsPointerOverGameObject ());
		return (Input.GetMouseButtonDown (0) && EventSystem.current.currentSelectedGameObject != null);
	}
}
