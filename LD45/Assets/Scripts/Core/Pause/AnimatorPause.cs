using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorPause : MonoBehaviour {
	Animator animator;

	void Awake() {
		animator = GetComponent<Animator>();

		EventManager.OnPauseChanged += OnPauseChanged;
	}

	void OnDestroy() {
		EventManager.OnPauseChanged -= OnPauseChanged;
	}

	void OnPauseChanged(EventData ed) {
		animator.enabled = !GameManager.Instance.IsPaused;
	}
}
