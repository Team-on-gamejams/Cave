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
		canvasGroup = GetComponent<CanvasGroup>();
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
		if (!CanChangeShowHide())
			return;

		BeforeShow();
		isShowed = true;
		gameObject.SetActive(true);

		LeanTween.cancel(gameObject);
		LeanTween.value(gameObject, canvasGroup.alpha, 1.0f, ShowTime)
			.setOnUpdate((float a) => {
				canvasGroup.alpha = a;
				AfterShow();
			});
	}

	public void Hide() {
		if (!CanChangeShowHide())
			return;

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

	protected void HideAfterStart() {
		if (NeedShowHide) {
			if (!isShowed) {
				canvasGroup.alpha = 0;
				gameObject.SetActive(false);
			}
		}
		else {
			isShowed = true;
		}
	}

	virtual public bool CanChangeShowHide() => true;

	virtual protected void BeforeShow() { }
	virtual protected void AfterShow() { }
	virtual protected void BeforeHide() { }
	virtual protected void AfterHide() { }
}
