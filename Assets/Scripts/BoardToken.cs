using UnityEngine;
using System.Collections;

/// <summary>
/// Board token. Defines an object with a space on the board that
/// can be selected and has a context menu with dynamically
/// defined functions.
/// </summary>
public class BoardToken : SelectableWithContextMenu {

	/// <summary>
	/// The current tile.
	/// </summary>
	protected BoardTile currentTile;

	/// <summary>
	/// The time it takes to move if the currentTile is changed
	/// </summary>
	public float moveTime = 0.3f;

	/// <summary>
	/// The velocity used by smoothDamp
	/// </summary>
	private Vector3 velocity = Vector3.zero;

	/// <summary>
	/// Raises the mouse down event.
	/// NOTICE: Comes before update. See http://docs.unity3d.com/Manual/ExecutionOrder.html
	/// </summary>
	protected void OnMouseDown() {
		Debug.Log ("Clicked token [" + this.name + "].");
		
		// Toggle the selector and gui context menu
		ToggleSelect ();
	}
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	protected void Update() {
		// Transform is always on the current tile
		transform.position = Vector3.SmoothDamp(transform.position, currentTile.transform.position, ref velocity, moveTime);

		// OnMouseDown is not registered on Wii U. Manually check for it here.
		#if UNITY_WIIU && !(UNITY_EDITOR)
		if (WiiUtilities.ColliderTouched(GetComponent<Collider>())) {
			OnMouseDown();
		}
		#endif

		// Deactivate the selection
		if (!selectedThisFrame && SelectionTools.ClickOutsideGUI()) {
			Deselect();
		}
	}

	#region Accessors and Mutators
	public BoardTile CurrentTile {
		get {
			return currentTile;
		}
		set {
			currentTile = value;
		}
	}
	#endregion
}
