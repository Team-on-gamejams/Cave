using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {
	public Inventory Inventory;

	void Awake() {
		GameManager.Instance.Player = this;
	}
}
