using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class Character : MonoBehaviour {
	[Header("Stats"), Space]
	public HPStat hitPoints;
	public StaminaStat staminaPoints;
	public ManaStat manaPoints;
	public SleepStat sleepPoints;
	public HungerStat hungerPoints;
	public ExperienceStat experiencePoints;

	[Header("Moving values"), Space]
	[SerializeField] float acceleration = 4;
	[SerializeField] float maxSpeed = 5;
	float currSpeed;

	[Header("Refs"), Space]
	[SerializeField] protected Animator animator;
	[SerializeField] protected Rigidbody2D rigidbody;

	Vector3 moveDirection;

#if UNITY_EDITOR
	private void OnValidate() {
		if (!hitPoints)
			hitPoints = GetComponentInChildren<HPStat>();
		if (!staminaPoints)
			staminaPoints = GetComponentInChildren<StaminaStat>();
		if (!manaPoints)
			manaPoints = GetComponentInChildren<ManaStat>();
		if (!sleepPoints)
			sleepPoints = GetComponentInChildren<SleepStat>();
		if (!hungerPoints)
			hungerPoints = GetComponentInChildren<HungerStat>();
		if (!experiencePoints)
			experiencePoints = GetComponentInChildren<ExperienceStat>();

		if (!animator)
			animator = GetComponent<Animator>();
		if (!rigidbody)
			rigidbody = GetComponent<Rigidbody2D>();
	}
#endif

	void FixedUpdate() {
		ProcessMove();
	}

	public void DirectionMove(float x, float y) {
		DirectionMove(new Vector2(x, y));
	}

	public void DirectionMove(Vector2 vector) {
		moveDirection = vector.normalized;
		RotateSprite();
	}

	void RotateSprite() {
		if ((moveDirection.x > 0 && transform.localScale.x < 0) || (moveDirection.x < 0 && transform.localScale.x > 0))
			transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
	}

	void ProcessMove() {
		if (moveDirection != Vector3.zero) {
			if(currSpeed != maxSpeed) {
				currSpeed += acceleration * Time.deltaTime * Time.deltaTime * rigidbody.mass;
				if (currSpeed > maxSpeed)
					currSpeed = maxSpeed;
			}

			rigidbody.velocity = moveDirection * currSpeed;

			animator.SetBool("IsMoving", true);
		}
		else {
			currSpeed = 0;

			animator.SetBool("IsMoving", false);
		}
	}
}
