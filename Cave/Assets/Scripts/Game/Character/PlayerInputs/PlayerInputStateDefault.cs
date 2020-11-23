using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "New PlayerInputStateDefault", menuName = "Player Input/Default", order = 52)]
public class PlayerInputStateDefault : PlayerInputStateBase {
	[SerializeField] float mouseHoldTimeMax = 0.15f;
	float mouseHoldTime;

	public override void Update() {
		if (!GameManager.Instance.IsPaused) {
			WASDMove();
			MouseMove();
		}

		ProcessInputs();
	}

	//TODO: ability to map WASD
	protected void WASDMove() {
		float v = Input.GetAxisRaw("Vertical"), h = Input.GetAxisRaw("Horizontal");

		if (v != 0 || h != 0) {
			flyweight.player.InterruptAction();
			flyweight.player.DirectionMove(v, h);
		}
	}

	protected void MouseMove() {
		if (Input.GetMouseButton(0))
			mouseHoldTime += Time.fixedDeltaTime;
		else
			mouseHoldTime = 0;

		if (
			PlayerInputFlyweight.CanMouseMove &&
			(Input.GetMouseButtonDown(0) || mouseHoldTime >= mouseHoldTimeMax)
			&& !EventSystem.current.IsPointerOverGameObject()
		) {
			flyweight.player.InterruptAction();
			flyweight.player.MoveTo(GameManager.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition));
		}
	}
}
