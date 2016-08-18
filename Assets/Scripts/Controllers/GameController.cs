using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Game controller.
/// Responsible for handling the actual game and ensuring 
/// everything is updating and running smoothly.
/// </summary>
public class GameController : Singleton<GameController> {

	#region Attributes and Properties
	/// <summary>
	/// The scene to generate the actual game objects on
	/// </summary>
	public string generateOnScene = "Game";

	/// <summary>
	/// The default action points.
	/// </summary>
	public int defaultActionPoints = 3;

	/// <summary>
	/// The tile offset.
	/// </summary>
	public float tileOffset;

	/// <summary>
	/// The size of the board.
	/// </summary>
	public int boardSize;

	/// <summary>
	/// The board parent.
	/// </summary>
	public Transform boardParent;

	/// <summary>
	/// The players.
	/// </summary>
	public PlayerController[] players;

	/// <summary>
	/// The board tile prefabs.
	/// </summary>
	public BoardTile[] boardTilePrefabs;

	/// <summary>
	/// The board.
	/// </summary>
	private Board board = new Board();
	#endregion

	#region Public Methods
	/// <summary>
	/// Generates the board and spawns the players; all procedural.
	/// </summary>
	public void GenerateGame() {
		board.Refresh ();
		board.Generate(boardTilePrefabs, boardSize, tileOffset, transform.position, boardParent);

		// for now
		players[0].CurrentTile = board[0,0];
		players[0].transform.position = players[0].CurrentTile.transform.position;
		players[0].gameObject.SetActive(true);
	}

	public Board Board {
		get {
			return board;
		}
	}

	#endregion

	#region Protected Methods
	/// <summary>
	/// Initializes a new instance of the <see cref="GameController"/> class.
	/// </summary>
	protected GameController() {}

	/// <summary>
	/// Start this instance.
	/// </summary>
	protected void Start() {
		if (Application.loadedLevelName == generateOnScene)
			GenerateGame();
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	protected void Update() {
	}

	/// <summary>
	/// Raises the level was loaded event.
	/// </summary>
	protected void OnLevelWasLoaded() {
		if (Application.loadedLevelName == generateOnScene)
			GenerateGame ();
		else {
			foreach(PlayerController p in players)
				p.gameObject.SetActive(false);
			board.Refresh();
		}
	}
	#endregion
}
