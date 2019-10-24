using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenu : BaseWindow {
	public void ChangeShowHide() {
		if (isShowed)
			Hide(false);
		else
			Show(false);
	}

	protected override void BeforeShow() {
		base.BeforeShow();
		GameManager.Instance.IsPaused = true;
	}

	protected override void AfterHide() {
		base.AfterHide();
		GameManager.Instance.IsPaused = false;
	}
}
