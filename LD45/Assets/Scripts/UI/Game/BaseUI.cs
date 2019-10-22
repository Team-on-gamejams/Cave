using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUI : MonoBehaviour {
	[SerializeField] protected bool NeedShowHide = true;
	[SerializeField] float ShowTime = 0.2f;

	CanvasGroup canvasGroup;

	protected bool IsShowed => isShowed;
	bool isShowed = false;

	virtual protected void Awake() {
		if (NeedShowHide) {
			canvasGroup = GetComponent<CanvasGroup>();
			canvasGroup.alpha = 0;
			gameObject.SetActive(false);
		}
		else {
			isShowed = true;
		}
		
	}

	public void ChangeShowHide() {
		if (!CanChangeShowHide())
			return;
		if (isShowed)
			Hide();
		else
			Show();
	}

	public void Show() {
		BeforeShow();
		gameObject.SetActive(true);
		isShowed = true;

		LeanTween.cancel(gameObject);
		LeanTween.value(gameObject, canvasGroup.alpha, 1.0f, ShowTime)
			.setOnUpdate((float a) => {
				canvasGroup.alpha = a;
				AfterShow();
			});
	}

	public void Hide() {
		BeforeHide();
		LeanTween.cancel(gameObject);
		LeanTween.value(gameObject, canvasGroup.alpha, 0.0f, ShowTime)
			.setOnUpdate((float a)=> {
				canvasGroup.alpha = a;
			})
			.setOnComplete(() => {
				isShowed = false;
				gameObject.SetActive(false);
				AfterHide();
			});
	}

	virtual protected bool CanChangeShowHide() => true;

	virtual protected void BeforeShow() { }
	virtual protected void AfterShow() { }
	virtual protected void BeforeHide() { }
	virtual protected void AfterHide() { }
}
