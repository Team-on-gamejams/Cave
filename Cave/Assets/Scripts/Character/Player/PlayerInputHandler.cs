using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class PlayerInputHandler : MonoBehaviour{
	Player player;

	private void Start() {
		player = GameManager.Instance.player.character;
	}

	public void OnMove(InputAction.CallbackContext context) {
		Vector2 value = context.ReadValue<Vector2>();

		player.DirectionMove(value);
	}

	public void OnLookMouse(InputAction.CallbackContext context) {

	}

	public void OnLookGamepad(InputAction.CallbackContext context) {

	}

	public void OnFire(InputAction.CallbackContext context) {

	}
}
