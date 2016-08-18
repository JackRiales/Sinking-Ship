using UnityEngine;
using System.Collections;

/// <summary>
/// Camera facing billboard.
/// </summary>
public class CameraFacingBillboard : MonoBehaviour {

	/// <summary>
	/// The camera to face.
	/// </summary>
	public Camera cameraToFace;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	protected void Awake() {
		if (cameraToFace == null)
			cameraToFace = Camera.main;
	}
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	protected void Update () {
		this.transform.LookAt (cameraToFace.transform.position, Vector3.up);
	}
}
