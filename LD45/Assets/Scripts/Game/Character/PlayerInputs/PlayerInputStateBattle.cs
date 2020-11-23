using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerInputStateBattle", menuName = "Player Input/Battle", order = 52)]
public class PlayerInputStateBattle : PlayerInputStateDefault {
	public override void Update() {
		if (!GameManager.Instance.IsPaused)
			WASDMove();

		ProcessInputs();
	}
}
