using UnityEngine;
using System.Collections;

/// <summary>
/// Camera underwater fog.
/// Attach to water object.
/// </summary>
public class CameraUnderwaterFog : MonoBehaviour {

	public Camera affectedCamera;
	public Color fogColor;
	public Color backgroundColor;
	public float fogDensity;

	// Caching current camera properties
	private bool defaultFog;
	private Color defaultFogColor;
	private float defaultFogDensity;
	private Material defaultSkybox;
	private Color defaultBackgroundColor;

	protected void Awake() {
		// Cache all current information
		defaultFog = RenderSettings.fog;
		defaultFogColor = RenderSettings.fogColor;
		defaultFogDensity = RenderSettings.fogDensity;
		defaultSkybox = RenderSettings.skybox;
		defaultBackgroundColor = affectedCamera.backgroundColor;
	}

	protected void Update() {
		if (affectedCamera.transform.position.y < transform.position.y) {
			RenderSettings.fog = true;
			RenderSettings.fogColor = fogColor;
			RenderSettings.fogDensity = fogDensity;
			RenderSettings.skybox = null;
			affectedCamera.backgroundColor = backgroundColor;
		} else {
			RenderSettings.fog = defaultFog;
			RenderSettings.fogColor = defaultFogColor;
			RenderSettings.fogDensity = defaultFogDensity;
			RenderSettings.skybox = defaultSkybox;
			affectedCamera.backgroundColor = defaultBackgroundColor;
		}
	}
}
