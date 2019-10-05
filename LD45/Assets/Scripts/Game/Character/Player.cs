using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {
	public Inventory Inventory;
	public Equipment Equipment;

	void Awake() {
		GameManager.Instance.Player = this;
	}
}
