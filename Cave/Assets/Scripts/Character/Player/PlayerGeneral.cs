using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class PlayerGeneral : MonoBehaviour {
	[Header("Refs"), Space]
	[ReadOnly] public PlayerInputHandler inputHandler;
	[ReadOnly] public Player character;

#if UNITY_EDITOR
	private void OnValidate() {
		if (!inputHandler)
			inputHandler = GetComponentInChildren<PlayerInputHandler>();
		if (!character)
			character = GetComponentInChildren<Player>();
	}
#endif

	private void Awake() {
		GameManager.Instance.player = this;
	}

	private void OnDestroy() {
		GameManager.Instance.player = null;
	}
}
