using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWindow : MonoBehaviour {
	[SerializeField] LeanTweenType Ease = LeanTweenType.easeOutBack;
	[SerializeField] float MoveTime = 0.5f;
	[SerializeField] Vector2 HidePos = new Vector2(0, 1000);
	[SerializeField] Vector2 ShowPos = new Vector2(0, 0);

	protected bool isShowed = false;

	protected void Awake() {
		transform.localPosition = HidePos;
	}

	public virtual void Show(bool isForce) {
		isShowed = true;
		BeforeShow();
		Move(isForce, ShowPos, AfterShow);
	}

	public virtual void Hide(bool isForce) {
		isShowed = false;
		BeforeHide();
		Move(isForce, HidePos, AfterHide);
	}

	void Move(bool isForce, Vector2 pos, Action onEnd) {
		if (isForce) {
			transform.position = HidePos;
		}
		else {
			LeanTween.cancel(gameObject, false);
			LeanTween.moveLocal(gameObject, pos, MoveTime)
				.setEase(Ease)
				.setOnComplete(onEnd);
		}
	}

	protected virtual void BeforeShow() { }
	protected virtual void AfterShow() { }
	protected virtual void BeforeHide() { }
	protected virtual void AfterHide() { }
}
