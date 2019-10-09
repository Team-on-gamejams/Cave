﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerKeyboardMover : MonoBehaviour {
	public bool CanMouseMove = true;
	public Action OnMouseMoveEnd;

	public KeyCode[] Keys;
	public UnityEvent[] OnButtonPressed;

	[SerializeField] SpriteRenderer spriteRenderer;
	[SerializeField] Rigidbody2D rigidbody;
	[SerializeField] Player player;
	Animator animator;

	Vector3 moveToDirection;
	Vector3 moveToPoint;

	void Awake() {
		moveToPoint = Vector3.zero;
	}

	void Start() {
		animator = player.Animator;
	}

	//Split to different methods
	void FixedUpdate() {
		//TODO: початок руху з прискоренням вплоть до макс. швидкості. Зупинка моментальна
		if (!player.IsPlayingBlockerAnimation) {
			float v = Input.GetAxisRaw("Vertical");
			float h = Input.GetAxisRaw("Horizontal");
			bool wasMoved = false;

			if (v != 0 || h != 0) {
				if (moveToPoint != Vector3.zero) {
					moveToPoint = Vector3.zero;
					OnMouseMoveEnd = null;
				}
				rigidbody.MovePosition(transform.localPosition + new Vector3(h, v).normalized * player.speed * Time.deltaTime);
				wasMoved = true;
				//if (h > 0 && spriteRenderer.flipX)
				//	spriteRenderer.flipX = false;
				//if (h < 0 && !spriteRenderer.flipX)
				//	spriteRenderer.flipX = true;

				if ((h > 0 && transform.localScale.x < 0) || (h < 0 && transform.localScale.x > 0))
					transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z); ;
			}

			if (CanMouseMove && Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject()) {
				moveToPoint = GameManager.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
				moveToPoint.z = 0;
				moveToDirection = moveToPoint - transform.position;

				if ((moveToDirection.x > 0 && transform.localScale.x < 0) || (moveToDirection.x < 0 && transform.localScale.x > 0))
					transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z); ;
			}

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
		
			for (byte i = 1; i < Keys.Length; ++i)
				if (Input.GetKeyDown(Keys[i]))
					OnButtonPressed[i]?.Invoke();
		}

		//TODO: Самий справжній костиль
		//Переробити на інпут в якого буде буль - чи можна робити під час анімації
		//Бо щас під час анімацій можна лише відкривати інвентарь
		//Того щас такий костиль
		if (Input.GetKeyDown(Keys[0]))
			OnButtonPressed[0]?.Invoke();
	}
}