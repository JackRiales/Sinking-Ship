using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board {

	private BoardTile[,] _boardMap;

	public void Generate (BoardTile[] prefabs, int size, float offset, Vector3 origin, Transform parentTransform = null) {
		_boardMap = new BoardTile[size, size];

		for (int z = 0; z < size; z++) {
			for (int x = 0; x < size; x++) {

				// Create the object at the given index
				GameObject btgo = (GameObject) GameObject.Instantiate(
					prefabs[Random.Range(0, prefabs.Length - 1)].gameObject, 
				   	new Vector3(origin.x + x * offset, origin.y, origin.z + z * offset), 
				    Quaternion.identity);

				// Change the name
				btgo.name = "Space[" + x + "," + z + "]";

				// Set their parent if it exists
				if (parentTransform != null)
					btgo.transform.parent = parentTransform;

				// Get the component
				_boardMap[x,z] = btgo.GetComponent<BoardTile>();

				// Set each adjacent relationship
				if (x != 0 && _boardMap[x-1,z] != null) {
					_boardMap[x-1,z].adjacent.Add(_boardMap[x,z]);
					_boardMap[x,z].adjacent.Add (_boardMap[x-1,z]);
				}
				
				if (z != 0 && _boardMap[x,z-1] != null) {
					_boardMap[x,z-1].adjacent.Add(_boardMap[x,z]);
					_boardMap[x,z].adjacent.Add (_boardMap[x,z-1]);
				}
			}
		}
	}

	public void Refresh() {
		if (_boardMap == null)
			return;

		foreach (BoardTile b in _boardMap) {
			GameObject.Destroy(b.gameObject);
		}
		if (_boardMap != null)
			_boardMap = null;
	}

	public void SelectAllTiles() {
		foreach(BoardTile b in _boardMap)
			b.Select ();
	}

	public void DeselectAllTiles() {
		foreach(BoardTile b in _boardMap)
			b.Deselect();
	}

	public BoardTile[] GetSelectedTiles() {
		List<BoardTile> tiles = new List<BoardTile>();
		foreach(BoardTile b in _boardMap) {
			if (b.IsSelected) {
				tiles.Add(b);
			}
		}
		return tiles.ToArray();
	}

	public BoardTile this[int i, int i2] {
		get { return _boardMap[i,i2]; }
		set { _boardMap[i,i2] = value; }
	}
}
