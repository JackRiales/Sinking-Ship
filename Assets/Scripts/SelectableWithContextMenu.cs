using UnityEngine;
using System.Collections;

public class SelectableWithContextMenu : Selectable {

	public GameObject contextMenuGUI;

	/// <summary>
	/// Toggles selection. Selects or deselects.
	/// </summary>
	public override void ToggleSelect() {
		base.ToggleSelect();
		contextMenuGUI.SetActive(!contextMenuGUI.activeSelf);
	}

	/// <summary>
	/// Select this instance.
	/// </summary>
	public override void Select() {
		base.Select();
		contextMenuGUI.SetActive(true);
	}

	/// <summary>
	/// Deselect this instance.
	/// </summary>
	public override void Deselect() {
		base.Deselect();
		contextMenuGUI.SetActive(false);
	}
}
