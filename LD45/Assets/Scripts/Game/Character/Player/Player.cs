using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {
	public Inventory Inventory;
	public Equipment Equipment;
	public PlayerKeyboardMover PlayerKeyboardMover;

	void Awake() {
		GameManager.Instance.Player = this;
		InteractDistSqr = InteractDist * InteractDist;
	}

	public void InterruptAction() {
		if ((Equipment.NeedInterrupt() || OnMoveEndEvent != null) && !GameManager.Instance.IsPaused) {
			StopMove();
			Equipment.InterruptAction();
			Animator.Play("DwarfAnim");
		}
	}
}
