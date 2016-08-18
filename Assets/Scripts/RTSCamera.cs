using UnityEngine;
using System.Collections;

/// <summary>
/// Real-Time-Strategy camera. Able to move cardinal directions using the given
/// axes and zoom in with the given buttons.
/// <remarks>Written by Jack Riales</remarks>
/// </summary>
public class RTSCamera : MonoBehaviour {

	public enum UpDirection {Y, Z}
	public UpDirection upDirection;
	public string horizontalMotionAxis;
	public string verticalMotionAxis;
	public string zoomAxis;
	public string zoomInButton;
	public string zoomOutButton;
	public float motionSpeed;
	public Rect motionBoundingBox;
	public float zoomSpeed;
	public float zoomMin;
	public float zoomMax;
	public bool drawBoundingBox;
	public Color boundingBoxGizmoColor;
	private Camera cameraCache;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	protected void Awake() {
		cameraCache = GetComponent<Camera> ();
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	protected void Update() {
		// Handle translation
		if (upDirection == UpDirection.Y) {
			transform.position = new Vector3 (
				Mathf.Clamp (
					transform.position.x + Input.GetAxis (horizontalMotionAxis) * motionSpeed * Time.deltaTime,
					motionBoundingBox.xMin, 
					motionBoundingBox.xMax + motionBoundingBox.width),
				transform.position.y,
				Mathf.Clamp (
					transform.position.z + Input.GetAxis (verticalMotionAxis) * motionSpeed * Time.deltaTime,
					motionBoundingBox.yMin,
					motionBoundingBox.yMax + motionBoundingBox.height)
			);
		} else {
			transform.position = new Vector3 (
				Mathf.Clamp (
					transform.position.x + Input.GetAxis (horizontalMotionAxis) * motionSpeed * Time.deltaTime,
					motionBoundingBox.xMin, 
					motionBoundingBox.xMax + motionBoundingBox.width),
				Mathf.Clamp (
					transform.position.y + Input.GetAxis (verticalMotionAxis) * motionSpeed * Time.deltaTime,
					motionBoundingBox.yMin,
					motionBoundingBox.yMax + motionBoundingBox.height),
				transform.position.z
				);
		}

		// Handle zoom (currently through fov. probably not the best idea.)
		if (!Input.GetButton (zoomInButton) && !Input.GetButton (zoomOutButton))
			cameraCache.fieldOfView += Input.GetAxis(zoomAxis) * zoomSpeed * Time.deltaTime;

		if (Input.GetAxis (zoomAxis) == 0 && Input.GetButton (zoomInButton))
			cameraCache.fieldOfView -= zoomSpeed * Time.deltaTime;

		else if (Input.GetAxis (zoomAxis) == 0 && Input.GetButton(zoomOutButton))
			cameraCache.fieldOfView += zoomSpeed * Time.deltaTime;

		// Clamp the field of view
		if (cameraCache.fieldOfView > zoomMax)
			cameraCache.fieldOfView = zoomMax;
		if (cameraCache.fieldOfView < zoomMin)
			cameraCache.fieldOfView = zoomMin;
	}

	protected void OnDrawGizmosSelected() {
		if (drawBoundingBox) {
			Gizmos.color = boundingBoxGizmoColor;
			float boxOriginX = motionBoundingBox.xMax;
			float boxOriginY = motionBoundingBox.yMax;
			float boxBoundsX = motionBoundingBox.width * 2;
			float boxBoundsZ = motionBoundingBox.height * 2;
			Vector3 boxOrigin;
			Vector3 boxBounds;
			if (upDirection == UpDirection.Y) {
				boxOrigin = new Vector3(boxOriginX, 0, boxOriginY);
				boxBounds = new Vector3(boxBoundsX, 0, boxBoundsZ);
			} else {
				boxOrigin = new Vector3(boxOriginX, boxOriginY, 0);
				boxBounds = new Vector3(boxBoundsX, boxBoundsZ, 0);
			}

			Gizmos.DrawWireCube (boxOrigin, boxBounds);
		}
	}
}
