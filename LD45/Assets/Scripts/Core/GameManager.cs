using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager> {
	// guarantee this will be always a singleton only - can't use the constructor!
	protected GameManager() { }

	public EventManager EventManager;

	public void Awake() {
		EventManager = new EventManager();
		Input.multiTouchEnabled = false;
		LeanTween.init(800);
	}
}