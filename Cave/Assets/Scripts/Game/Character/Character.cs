using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour {
	public Action OnMoveEndEvent;

	public HPStat HitPoints;
	public StaminaStat StaminaPoints;
	public ManaStat ManaPoints;
	public SleepStat SleepPoints;
	public HungerStat HungerPoints;
	public ExperienceStat ExperiencePoints;

	public float speed;

	public float InteractDist;
	[NonSerialized] public float InteractDistSqr;

	public Animator Animator;

	[SerializeField] protected Rigidbody2D Rigidbody;

	Vector3 moveToDirection, moveToPoint;

	void FixedUpdate() {
		if (GameManager.Instance.IsPaused)
			return;

		ProcessMove();
	}

	public void DirectionMove(float v, float h) {
		moveToDirection.x = h;
		moveToDirection.y = v;
		moveToDirection = moveToDirection.normalized;

		if (moveToPoint != Vector3.zero) {
			moveToPoint = Vector3.zero;
			OnMoveEndEvent = null;
		}

		RotateSprite();
	}

	public void MoveTo(Vector3 position) {
		moveToPoint = position;
		moveToPoint.z = 0;
		moveToDirection = (moveToPoint - transform.position).normalized;

		RotateSprite();
	}

	public void StopMove() {
		OnMoveEndEvent = null;
		moveToDirection = moveToPoint = Vector3.zero;
	}

	public bool CanInteract(Vector3 pos) {
		return (transform.position - pos).sqrMagnitude < InteractDistSqr;
	}

	public bool CanInteract(Vector3 pos, float InteractDistSqr) {
		return (transform.position - pos).sqrMagnitude < InteractDistSqr;
	}

	void RotateSprite() {
		if ((moveToDirection.x > 0 && transform.localScale.x < 0) || (moveToDirection.x < 0 && transform.localScale.x > 0))
			transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
	}

	void ProcessMove() {
		bool wasMoved = false;
		if (moveToPoint != Vector3.zero) {
			if ((transform.position - moveToPoint).sqrMagnitude > InteractDistSqr) {
				Rigidbody.MovePosition(transform.localPosition + moveToDirection * speed * Time.deltaTime);
				wasMoved = true;
			}
			else {
				moveToDirection = moveToPoint = Vector3.zero;
				if (OnMoveEndEvent != null) {
					OnMoveEndEvent.Invoke();
					OnMoveEndEvent = null;
				}
			}
		}
		else if (moveToDirection != Vector3.zero) {
			Rigidbody.MovePosition(transform.localPosition + moveToDirection * speed * Time.deltaTime);
			wasMoved = true;
			moveToDirection = Vector3.zero;
		}

		Animator.SetBool("IsMoving", wasMoved);
	}
}
