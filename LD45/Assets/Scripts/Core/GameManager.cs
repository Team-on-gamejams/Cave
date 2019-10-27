using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager> {
	// guarantee this will be always a singleton only - can't use the constructor!
	protected GameManager() { }

	public bool IsPaused {
		set {
			_IsPaused = value;
			EventManager.CallOnPauseChanged();
		}
		get {
			return _IsPaused;
		}
	}
	private bool _IsPaused;

	public float GameSpeed {
		set {
			_GameSpeed = value;
			EventManager.CallOnGameSpeedChanged();
		}
		get {
			return _GameSpeed;
		}
	}
	private float _GameSpeed = 1.0f;

	public Camera MainCamera;
	public GameObject CollectorItems;
	public GameObject CollectorBuilding;
	public Interactable SelectedOutlineGO;

	public Player Player;
	public EventManager EventManager;

	void Awake() {
		MainCamera = Camera.main;
		EventManager = new EventManager();
		Input.multiTouchEnabled = false;
		LeanTween.init(800);

		_IsPaused = false;

		EventManager.OnSceneLoadEnd += OnSceneLoadEnd;
	}

	new void OnDestroy() {
		EventManager.OnSceneLoadEnd -= OnSceneLoadEnd;

		base.OnDestroy();
	}

	void OnSceneLoadEnd(EventData data) {
		MainCamera = Camera.main;
		IsPaused = false;
	}
}