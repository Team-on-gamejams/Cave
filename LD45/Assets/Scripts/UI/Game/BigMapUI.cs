using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigMapUI : BaseUI {
	void Start() {
		HideAfterStart();
	}

	protected override void BeforeShow() {
		GameManager.Instance.EventManager.CallOnBigMapShow();
	}

	protected override void BeforeHide() {
		GameManager.Instance.EventManager.CallOnBigMapHide();
	}

	public override bool CanChangeShowHide() {
		return !GameManager.Instance.IsPaused;
	}
}
