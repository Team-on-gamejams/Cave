using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerInputStateBigmap", menuName = "Player Input/Bigmap", order = 56)]
public class PlayerInputStateBigmap : PlayerInputStateBase {
	public override void Update() {

		ProcessInputs();
	}
}
