using UnityEngine;
using System.Collections;

/// <summary>
/// Runtime controller.
/// </summary>
public class RuntimeController : Singleton<RuntimeController> {

	#region Attributes and Properties
	/// <summary>
	/// State machine enumeration for the runtime scene events.
	/// </summary>
	public enum SceneState {
		Reset,
		Preload,
		Load,
		Unload,
		Postload,
		Ready,
		Run,
		Count
	};

	/// <summary>
	/// The initial scene to be loaded.
	/// </summary>
	public string initialSceneName;

	/// <summary>
	/// Debug verbosity
	/// </summary>
	public bool logToConsole = true;

	/// <summary>
	/// Definition for the scene update task delegate.
	/// </summary>
	private delegate void UpdateDelegate ();

	/// <summary>
	/// The name of the current scene.
	/// </summary>
	private string currentSceneName;

	/// <summary>
	/// The name of the next scene.
	/// </summary>
	private string nextSceneName;

	/// <summary>
	/// The resource unload task.
	/// </summary>
	private AsyncOperation resourceUnloadTask;

	/// <summary>
	/// The scene load task.
	/// </summary>
	private AsyncOperation sceneLoadTask;

	/// <summary>
	/// The state of the scene.
	/// </summary>
	private SceneState sceneState;

	/// <summary>
	/// The update delegates.
	/// </summary>
	private UpdateDelegate[] updateDelegates;
	#endregion

	#region Public Methods
	/// <summary>
	/// Switches the scene.
	/// </summary>
	/// <param name="nextSceneName">Next scene name.</param>
	public static void SwitchScene(string nextSceneName) {
		if (Instance.logToConsole)
			Debug.Log ("[RuntimeController] is switching the scene to " + 
			           nextSceneName + " from " + Instance.currentSceneName);

		if (Instance != null) {
			if (Instance.currentSceneName != nextSceneName) {
				Instance.nextSceneName = nextSceneName;
			}
		}
	}
	#endregion

	#region Protected Methods
	/// <summary>
	/// Initializes a new instance of the <see cref="RuntimeController"/> class.
	/// Used here to block the default constructor from being used.
	/// </summary>
	protected RuntimeController () {
	}

	/// <summary>
	/// Awake this instance.
	/// </summary>
	protected override void Awake() {
		base.Awake ();

		if (logToConsole)
			Debug.Log ("[RuntimeController] is setting up the update delegates.");

		// Instantiate the updateDelegates array
		updateDelegates = new UpdateDelegate[(int)SceneState.Count];

		// Setup the update delegates
		updateDelegates [(int)SceneState.Reset] = UpdateSceneReset;
		updateDelegates [(int)SceneState.Preload] = UpdateScenePreload;
		updateDelegates [(int)SceneState.Load] = UpdateSceneLoad;
		updateDelegates [(int)SceneState.Unload] = UpdateSceneUnload;
		updateDelegates [(int)SceneState.Postload] = UpdateScenePostload;
		updateDelegates [(int)SceneState.Ready] = UpdateSceneReady;
		updateDelegates [(int)SceneState.Run] = UpdateSceneRun;

		// Set the next scene name
		nextSceneName = initialSceneName;

		// Set the sceneState to the initial reset state for setup
		if (logToConsole)
			Debug.Log ("[RuntimeController] is setting the scene to Reset.");
		sceneState = SceneState.Reset;
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	protected void Update() {
		// Run the current scene state method
		if (updateDelegates [(int)sceneState] != null) {
			updateDelegates[(int)sceneState]();
		}
	}

	/// <summary>
	/// Raises the destroy event.
	/// </summary>
	protected override void OnDestroy() {
		if (logToConsole)
			Debug.Log ("[RuntimeController] is cleaning up.");

		base.OnDestroy ();

		if (updateDelegates != null) {
			for (int i = 0; i < (int)SceneState.Count; i++) {
				updateDelegates[i] = null;
			}
			updateDelegates = null;
		}
	}
	#endregion

	#region Private Methods
	/// <summary>
	/// Scene reset state.
	/// Performs GC collect manually so it doesn't affect game performance later.
	/// </summary>
	private void UpdateSceneReset() {
		if (logToConsole)
			Debug.Log ("[RuntimeController] is running GC collect.");
		System.GC.Collect ();
		sceneState = SceneState.Preload;
	}

	/// <summary>
	/// Scene preload state.
	/// Starts an asyncronous load of the next scene.
	/// </summary>
	private void UpdateScenePreload() {
		if (logToConsole)
			Debug.Log ("[RuntimeController] is beginning the async load of " + nextSceneName);
		sceneLoadTask = Application.LoadLevelAsync (nextSceneName);
		if (sceneLoadTask == null) Debug.LogError ("[RuntimeController] could not load next scene." +
			" You may want to check the string, or add that scene to the build settings.");
		sceneState = SceneState.Load;
	}

	/// <summary>
	/// Scene load state.
	/// Waits for scene to be loaded and does things while not done.
	/// </summary>
	private void UpdateSceneLoad() {
		if (sceneLoadTask != null && sceneLoadTask.isDone == true) {
			if (logToConsole)
				Debug.Log ("[RuntimeController] load completed.");
			sceneState = SceneState.Unload;
		} else {
			// Update scene loading progress here
		}
	}

	/// <summary>
	/// Scene unload state.
	/// Unloads unused assets.
	/// </summary>
	private void UpdateSceneUnload() {
		if (resourceUnloadTask == null) {
			if (logToConsole)
				Debug.Log ("[RuntimeController] is beginning to unload unused assets.");
			resourceUnloadTask = Resources.UnloadUnusedAssets ();
		} else {
			if (resourceUnloadTask.isDone) {
				if (logToConsole)
					Debug.Log ("[RuntimeController] unload complete.");
				resourceUnloadTask = null;
				sceneState = SceneState.Postload;
			}
		}
	}

	/// <summary>
	/// Scene post load state.
	/// Last chance to perform stuff that needs to happen after loading.
	/// </summary>
	private void UpdateScenePostload() {
		if (logToConsole)
			Debug.Log ("[RuntimeController] is running postload.");
		currentSceneName = nextSceneName;
		sceneState = SceneState.Ready;
	}

	/// <summary>
	/// Scene ready state.
	/// Optionally performs GC collect and anything else that needs to happen
	/// right before the scene starts to run.
	/// </summary>
	private void UpdateSceneReady() {
		if (logToConsole)
			Debug.Log ("[RuntimeController] the scene is ready.");
		sceneState = SceneState.Run;
	}

	/// <summary>
	/// Scene run state.
	/// Runs scene until the current scene name is not the same as the next scene name
	/// which can only be done by using <see cref="SwitchScene()"/>
	/// </summary>
	private void UpdateSceneRun() {
		if (currentSceneName != nextSceneName) {
			if (logToConsole)
				Debug.Log ("[RuntimeController] is resetting states.");
			sceneState = SceneState.Reset;
		}
	}
	#endregion
}
