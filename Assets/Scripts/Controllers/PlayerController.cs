using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

/// <summary>
/// Player controller.
/// </summary>
public class PlayerController : BoardToken {

	#region Properties and Attributes
	public enum PlayerClass {
		/// <summary>
		/// The captain.
		/// </summary>
		Captain,

		/// <summary>
		/// The adventurer.
		/// </summary>
		Adventurer,

		/// <summary>
		/// The engineer.
		/// </summary>
		Engineer,

		/// <summary>
		/// The doctor.
		/// </summary>
		Doctor,

		/// <summary>
		/// The entertainer.
		/// </summary>
		Entertainer,

		/// <summary>
		/// The passenger.
		/// </summary>
		Passenger,

		/// <summary>
		/// The count.
		/// </summary>
		Count
	};

	/// <summary>
	/// The player class.
	/// </summary>
	public PlayerClass playerClass;

	/// <summary>
	/// The action points.
	/// </summary>
	private int actionPoints;
	#endregion

	#region Public Methods
	public IEnumerator MoveCharacter() {
		Debug.Log ("[PlayerController] " + name + " moving.");

		// If you don't have enough action points, you can't move
		if (actionPoints <= 0) {
			Debug.Log ("[PlayerController] Not enough action points to move.");
			StopCoroutine(MoveCharacter());
			return false; 
		}
		
		// Deselect the player
		Deselect ();
		
		// Select adjacent tiles
		BoardTile.SelectTileArea (currentTile, 2);
		
		// Get a list of selected tiles
		BoardTile[] selectedTiles = GameController.Instance.Board.GetSelectedTiles();
		
		// Capture a click event on one of them
		bool hasClicked = false;
		BoardTile clickedTile = null;
		while (!hasClicked) {
			foreach(BoardTile b in selectedTiles) {
				if (WiiUtilities.ColliderTouched(b.GetComponent<Collider>())) {
					clickedTile = b;
					hasClicked = true;
				} 
			}
			
			if (Input.GetMouseButtonDown(0) && clickedTile == null)
				break;
			
			yield return null;
		}

		if (clickedTile != null) {
			// Decrease action points for moving
			if (clickedTile != currentTile)
				actionPoints -= 1;

			// Move to the selected tile
			currentTile = clickedTile;

			// Deselect and end the action
			GameController.Instance.Board.DeselectAllTiles();
			StopCoroutine(MoveCharacter());
			return true;
		} else {
			Debug.Log ("[PlayerController] No tile clicked during move.");
			GameController.Instance.Board.DeselectAllTiles();
			StopCoroutine(MoveCharacter());
			return false;
		}
	}

	public int ActionPoints {
		get {
			return actionPoints;
		}
		set {
			actionPoints = value;
		}
	}

	#region UI Events
	public void EventMove() {
		StartCoroutine(MoveCharacter());
	}
	#endregion
	#endregion

	#region Protected Methods
	protected void Awake() {
		actionPoints = GameController.Instance.defaultActionPoints;
	}
	#endregion
}
