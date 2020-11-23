using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenu : BaseWindow {
	[SerializeField] SettingsWindow SettingsWindow;

	public void ChangeShowHide() {
		if (CanChangeShowHide()) {
			if (isShowed)
				Hide(false);
			else
				Show(false);
		}
	}

	protected bool CanChangeShowHide() {
		return !SettingsWindow.IsShowed;
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
