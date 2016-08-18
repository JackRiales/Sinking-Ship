using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Board tile.
/// </summary>
public class BoardTile : Selectable {

	#region Properties and Attributes
	//[System.Flags]
	public enum BoardTileState {
		None		= (1<<0),
		Underwater	= (1<<1),
		Abyss		= (1<<2)
	};

	public int index;
	public string tileName;
	public Transform playerSpot;
	public BoardTileState state = BoardTileState.None;
	public List<BoardTile> adjacent = new List<BoardTile>();
	#endregion

	#region Public Methods
	/// <summary>
	/// Selects a tile area. A recursive process that iterates on a certain tile
	/// a number of times equal to size.
	/// <example>
	/// Setting the size to 1 would only select the single tile. 2 would select
	/// its immediate adjacent tiles, and so on.
	/// </example>
	/// </summary>
	/// <param name="center">Center tile to begin process on.</param>
	/// <param name="size">Size of the recursive process.</param>
	public static void SelectTileArea(BoardTile center, int size) {
		if (size == 0 || center == null)
			return;

		// Select the tile
		center.Select ();

		// Select its friends
		foreach (BoardTile bt in center.adjacent) {
			SelectTileArea(bt, size - 1);
		}
	}
	#endregion
}
