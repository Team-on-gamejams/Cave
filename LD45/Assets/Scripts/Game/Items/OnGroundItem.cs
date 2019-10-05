using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundItem : Interactable {
	public ItemSO Item;

	void OnMouseDown() {
		if (!CanInteract())
			return;

		//TODO: add fly animation
		GameManager.Instance.Player.Inventory.AddItem(Item);

		Destroy(gameObject);
	}
}
