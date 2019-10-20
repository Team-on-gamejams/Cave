using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager> {
	// guarantee this will be always a singleton only - can't use the constructor!
	protected GameManager() { }

	public Camera MainCamera;
	public GameObject CollectorItems;

	public Interactable SelectedOutlineGO => SelectedOutlineGOArr[SelectedOutlineGOMax];
	public List<Interactable> SelectedOutlineGOArr;
	public int SelectedOutlineGOMax;

	public Player Player;
	public EventManager EventManager;

	void Awake() {
		EventManager = new EventManager();
		SelectedOutlineGO = new List<Interactable>();
		SelectedOutlineGOMax = 0;

		MainCamera = Camera.main;

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