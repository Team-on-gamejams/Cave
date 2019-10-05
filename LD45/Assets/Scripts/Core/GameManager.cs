using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager> {
	// guarantee this will be always a singleton only - can't use the constructor!
	protected GameManager() { }

	public Camera MainCamera;

	public Player Player;
	public EventManager EventManager;

	void Awake() {
		MainCamera = Camera.main;
		EventManager = new EventManager();
		Input.multiTouchEnabled = false;
		LeanTween.init(800);

		EventManager.OnSceneLoadEnd += OnSceneLoadEnd;
	}

	new void OnDestroy() {
		EventManager.OnSceneLoadEnd -= OnSceneLoadEnd;

		base.OnDestroy();
	}

	void OnSceneLoadEnd(EventData data) {
		MainCamera = Camera.main;
	}
}