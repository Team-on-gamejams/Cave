using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerKeyboardMover : MonoBehaviour {
	public bool CanMouseMove = true;

	public InputEvent[] Inputs;

	[SerializeField] Player player;

	Vector3 moveToDirection;
	Vector3 moveToPoint;

	const float mouseHoldTimeMax = 0.15f;
	float mouseHoldTime;

	void Awake() {
		moveToPoint = Vector3.zero;
	}

	void Update() {
		if (!GameManager.Instance.IsPaused) {
			WASDMove();
			MouseMove();
		}

		ProcessInputs();
	}

	//TODO: ability to map WASD
	void WASDMove() {
		float v = Input.GetAxisRaw("Vertical"), h = Input.GetAxisRaw("Horizontal");

		if(v != 0 || h != 0) {
			player.InterruptAction();
			player.DirectionMove(v, h);
		}
	}

	void MouseMove() {
		if (Input.GetMouseButton(0))
			mouseHoldTime += Time.fixedDeltaTime;
		else
			mouseHoldTime = 0;

		if (
			CanMouseMove &&
			(Input.GetMouseButtonDown(0) || mouseHoldTime >= mouseHoldTimeMax)
			&& !EventSystem.current.IsPointerOverGameObject()
			&& GameManager.Instance.SelectedOutlineGO == null
		) {
			player.InterruptAction();
			player.MoveTo(GameManager.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition));
		}
	}

	void ProcessInputs() {
		for (byte i = 0; i < Inputs.Length; ++i) {
			InputEvent input = Inputs[i];
			if (GameManager.Instance.IsPaused && !input.IgnorePause)
				continue;

			switch (input.Type) {
				case InputEvent.InputEventType.Key:
					if (Input.GetKeyDown(input.Key)) {
						input.OnButtonPressed?.Invoke();
						if (input.InterruptAnim)
							player.InterruptAction();
					}
					break;
				case InputEvent.InputEventType.MouseWheel:
					var wheelDelta = Input.GetAxis("Mouse ScrollWheel");
					if (wheelDelta > 0f && input.IsWheelUp) {
						input.OnButtonPressed?.Invoke();
						if (input.InterruptAnim)
							player.InterruptAction();
					}
					else if (wheelDelta < 0f && !input.IsWheelUp) {
						input.OnButtonPressed?.Invoke();
						if (input.InterruptAnim)
							player.InterruptAction();
					}
					break;
				case InputEvent.InputEventType.None:
				default:
					break;
			}
		}
	}
}

//TODO: probably need custom inspector
// Кнопки для Hotbar НЕ використовують InterruptAnim, бо всередині Hotbar вона преривається при EquipItem
[Serializable]
public class InputEvent {
	public enum InputEventType : byte { None, Key, MouseWheel};

	public InputEventType Type;
	public KeyCode Key;
	public bool IsWheelUp; //true - up, false - down

	public bool IgnorePause;
	public bool InterruptAnim;

	public UnityEvent OnButtonPressed;
}