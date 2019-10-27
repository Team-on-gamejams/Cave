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

	void Awake() {
		DefaultInput.flyweight = Flyweight;
		BattleInput.flyweight = Flyweight;
		PauseInput.flyweight = Flyweight;
		BigmapInput.flyweight = Flyweight;

		currInput = DefaultInput;
	}

	void Update() {
		currInput.Update();
	}
}
