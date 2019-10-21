using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour {
	void Awake() {
		EventManager.OnPauseChanged += OnPauseChanged;
	}

	void OnDestroy() {
		EventManager.OnPauseChanged -= OnPauseChanged;
	}

	void OnPauseChanged(EventData ed) {
		if (GameManager.Instance.IsPaused) {
			Interactable.OnPause();
			ItemSlot.OnPause();
		}
	}
}
