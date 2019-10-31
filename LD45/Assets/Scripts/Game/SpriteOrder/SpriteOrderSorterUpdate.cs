using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOrderSorterUpdate : SpriteOrderSorter {
	[SerializeField] float UpdateFrequency = 0.1f;
	float tileLeft;

	void Update() {
		tileLeft -= Time.deltaTime;

		if (tileLeft <= 0) {
			tileLeft = UpdateFrequency;
			UpdateSortOrder();
		}
	}
}
