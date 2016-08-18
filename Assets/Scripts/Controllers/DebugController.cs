using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;

/// <summary>
/// Debug controller.
/// </summary>
public class DebugController : Singleton<DebugController> {

	public Dropdown playerSelector;
	public InputField actionPointField;
	public Text fpsText;
	public float fpsUpdateTime;
	private float deltaTime;

	/// <summary>
	/// Restarts the game application.
	/// </summary>
	public void DebugGameRestart() {
		RuntimeController.SwitchScene ("Main Menu");
	}

	/// <summary>
	/// Gives action points to each player.
	/// </summary>
	/// <param name="ap">Ap.</param>
	public void DebugGiveActionPoints() {
		int ap;
		if (int.TryParse(actionPointField.text, out ap)) {
			if (playerSelector.value == 0) {
				foreach (PlayerController pc in GameController.Instance.players) {
					pc.ActionPoints += ap;
				}
			} else {
				GameObject go = GameObject.Find(playerSelector.options[playerSelector.value].text);
				PlayerController pc = go.GetComponent<PlayerController>();
				pc.ActionPoints += ap;
			}
		} else {
			Debug.LogError("[DebugController] Could not parse AP field from string to int.");
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="DebugController"/> class.
	/// </summary>
	protected DebugController() {}

	protected void Start() {
		playerSelector.options.Clear ();
		playerSelector.options.Add (new Dropdown.OptionData ("Any"));
		foreach (PlayerController p in FindObjectsOfType(typeof(PlayerController)))
			playerSelector.options.Add (new Dropdown.OptionData(p.name));
		StartCoroutine (UpdateFPS ());
	}

	protected void OnLevelWasLoaded() {
		playerSelector.options.Clear ();
		playerSelector.options.Add (new Dropdown.OptionData ("Any"));
		foreach (PlayerController p in FindObjectsOfType(typeof(PlayerController)))
			playerSelector.options.Add (new Dropdown.OptionData(p.name));
	}

	protected void Update() {
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
	}

	protected IEnumerator UpdateFPS() {
		fpsText.text = "FPS: " + Mathf.RoundToInt(1.0f / deltaTime).ToString();
		yield return new WaitForSeconds(fpsUpdateTime);
		StartCoroutine (UpdateFPS ());
	}
}
