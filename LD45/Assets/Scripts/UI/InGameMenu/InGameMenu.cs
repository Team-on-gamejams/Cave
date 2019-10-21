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
}
