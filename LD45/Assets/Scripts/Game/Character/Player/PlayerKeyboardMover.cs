using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerKeyboardMover : MonoBehaviour {
	public bool CanMouseMove = true;
	public Action OnMouseMoveEnd;

	public InputEvent[] Inputs;

	[SerializeField] SpriteRenderer spriteRenderer;
	[SerializeField] Rigidbody2D rigidbody;
	[SerializeField] Player player;
	Animator animator;

	Vector3 moveToDirection;
	Vector3 moveToPoint;

	const float mouseHoldTimeMax = 0.15f;
	float mouseHoldTime;

	void Awake() {
		moveToPoint = Vector3.zero;
	}

	void Start() {
		animator = player.Animator;
	}

	void Update() {
		for (byte i = 0; i < Inputs.Length; ++i) {
			if (Input.GetKeyDown(Inputs[i].Key)) {
				Inputs[i].OnButtonPressed?.Invoke();
				if (Inputs[i].InterruptAnim)
					player.InterruptAction();
			}
		}
	}

	void FixedUpdate() {
		float v = Input.GetAxisRaw("Vertical");
		float h = Input.GetAxisRaw("Horizontal");
		bool wasMoved = false;

		if (v != 0 || h != 0)
			WASDMove(v, h, ref wasMoved);

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
			MoveTo(GameManager.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition));
		}

		ProcessMove(ref wasMoved);

	}

	public bool NeedInterrupt() {
		return OnMouseMoveEnd != null;
	}

	public void InterruptAction() {
		StopMove();
	}

	public void MoveTo(Vector3 position) {
		moveToPoint = position;
		moveToPoint.z = 0;
		moveToDirection = moveToPoint - transform.position;

		if ((moveToDirection.x > 0 && transform.localScale.x < 0) || (moveToDirection.x < 0 && transform.localScale.x > 0))
			transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
	}

	void StopMove() {
		OnMouseMoveEnd = null;
		moveToPoint = moveToPoint = Vector3.zero;
	}

	void WASDMove(float v, float h, ref bool wasMoved) {
		player.InterruptAction();
		if (moveToPoint != Vector3.zero) {
			moveToPoint = Vector3.zero;
			OnMouseMoveEnd = null;
		}
		rigidbody.MovePosition(transform.localPosition + new Vector3(h, v).normalized * player.speed * Time.deltaTime);
		wasMoved = true;

		if ((h > 0 && transform.localScale.x < 0) || (h < 0 && transform.localScale.x > 0))
			transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z); ;
	}

	void ProcessMove(ref bool wasMoved) {
		if (moveToPoint != Vector3.zero) {
			if ((transform.position - moveToPoint).sqrMagnitude > GameManager.Instance.Player.InteractDistSqr) {
				rigidbody.MovePosition(transform.localPosition + moveToDirection.normalized * player.speed * Time.deltaTime);
				wasMoved = true;
			}
			else {
				moveToPoint = Vector3.zero;
				if (OnMouseMoveEnd != null) {
					OnMouseMoveEnd.Invoke();
					OnMouseMoveEnd = null;
				}
			}
		}

		animator.SetBool("IsMoving", wasMoved);
	}
}

[Serializable]
public class InputEvent {
	public KeyCode Key;
	public bool InterruptAnim;
	public UnityEvent OnButtonPressed;
}