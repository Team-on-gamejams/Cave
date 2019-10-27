using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerInput : MonoBehaviour {
	[SerializeField] protected PlayerInputFlyweight Flyweight;

	[SerializeField] protected PlayerInputStateBase DefaultInput;
	[SerializeField] protected PlayerInputStateBase BattleInput;
	[SerializeField] protected PlayerInputStateBase PauseInput;
	[SerializeField] protected PlayerInputStateBase BigmapInput;

	protected PlayerInputStateBase currInput;
	protected Stack<PlayerInputStateBase> prevInputs;

	void Awake() {
		DefaultInput.flyweight = Flyweight;
		BattleInput.flyweight = Flyweight;
		PauseInput.flyweight = Flyweight;
		BigmapInput.flyweight = Flyweight;

		prevInputs = new Stack<PlayerInputStateBase>(4);
		currInput = DefaultInput;
		prevInputs.Push(currInput);

		EventManager.OnBigMapShow += OnBigMapShow;
		EventManager.OnBigMapHide += OnBigMapHide;
		EventManager.OnPauseChanged += OnPauseChanged;
	}

	void OnDestroy() {
		EventManager.OnBigMapShow -= OnBigMapShow;
		EventManager.OnBigMapHide -= OnBigMapHide;
		EventManager.OnPauseChanged -= OnPauseChanged;
	}

	void Update() {
		currInput.Update();
	}

	void OnBigMapShow(EventData ed) {
		prevInputs.Push(currInput);
		currInput = BigmapInput;
	}

	void OnBigMapHide(EventData ed) {
		currInput = prevInputs.Pop();
	}

	void OnPauseChanged(EventData ed) {
		if (GameManager.Instance.IsPaused) {
			prevInputs.Push(currInput);
			currInput = PauseInput;
		}
		else {
			currInput = prevInputs.Pop();
		}
	}
}
