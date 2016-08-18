using UnityEngine;
using System.Collections;

/// <summary>
/// Defines an object that is able to be selected on a board.
/// </summary>
public class Selectable : MonoBehaviour {

	/// <summary>
	/// The selector object.
	/// </summary>
	public GameObject selector;

	/// <summary>
	/// Determines if the object is currently selected
	/// </summary>
	protected bool isSelected = false;

	/// <summary>
	/// Determines if the object was selected this frame so that they may not
	/// be unselected on the same frame.
	/// </summary>
	protected bool selectedThisFrame = false;

	/// <summary>
	/// Switches the object to selected or deselected based on given boolean.
	/// </summary>
	public virtual void Switch(bool select) {
		if (select == false)
			Deselect ();
		else
			Select ();
	}

	/// <summary>
	/// Toggles selection. Selects or deselects.
	/// </summary>
	public virtual void ToggleSelect() {
		selector.SetActive(!selector.activeSelf);
		isSelected = selector.activeSelf;

		if (isSelected)
			selectedThisFrame = true;
		}

	/// <summary>
	/// Select this instance.
	/// </summary>
	public virtual void Select() {
		selector.SetActive(true);
		isSelected = true;

		selectedThisFrame = true;
	}

	/// <summary>
	/// Deselect this instance.
	/// </summary>
	public virtual void Deselect() {
		selector.SetActive(false);
		isSelected = false;
	}

	/// <summary>
	/// Gets a value indicating whether this instance is selected.
	/// </summary>
	/// <value><c>true</c> if this instance is selected; otherwise, <c>false</c>.</value>
	public virtual bool IsSelected {
		get {
			return isSelected;
		}
	}

	/// <summary>
	/// Ends the current cycle by resetting 'selectedThisFrame'
	/// </summary>
	protected virtual void LateUpdate() {
		// Ensure selector visibility
		if (isSelected && !selector.activeSelf)
			selector.SetActive(true);
		if (!isSelected && selector.activeSelf)
			selector.SetActive(false);

		selectedThisFrame = false;
	}
}
