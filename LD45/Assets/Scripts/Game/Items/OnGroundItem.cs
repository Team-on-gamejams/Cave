using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundItem : MonoBehaviour {
	public ItemSO Item;

	void OnMouseDown() {
		//TODO: add fly animation
		GameManager.Instance.Player.Inventory.AddItem(Item);

		Destroy(gameObject);
	}
}
