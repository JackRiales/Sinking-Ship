using UnityEngine;
using System.Collections;

/// <summary>
/// Runtime controller test.
/// Tested working correctly on 110515@3:28PM
/// </summary>
public class RuntimeControllerTest : MonoBehaviour {

	/// <summary>
	/// The level to move to.
	/// </summary>
	public string levelToMoveTo;

	/// <summary>
	/// Raises the GUI event.
	/// </summary>
	protected void OnGUI() {
		GUI.TextArea (new Rect (0, 0, 300, 25), "Press any button to move to the next area.");
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	protected void Update() {
		if (Input.anyKeyDown)
			RuntimeController.SwitchScene (levelToMoveTo);
	}
}
