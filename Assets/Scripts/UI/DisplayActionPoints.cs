using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Displays the action points.
/// </summary>
[RequireComponent(typeof(Text))]
public class DisplayActionPoints : MonoBehaviour {

	/// <summary>
	/// The attached player.
	/// </summary>
	public PlayerController player;

	/// <summary>
	/// The header. Comes before the numeric display.
	/// </summary>
	public string header;

	/// <summary>
	/// The text.
	/// </summary>
	private Text text;

	protected void Awake() {
		text = GetComponent<Text>();
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	protected void Update() {
		text.text = 
			header + player.ActionPoints.ToString();
	}

}
