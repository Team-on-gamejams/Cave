using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
	public List<ItemSO> items;

	void Awake() {
		items = new List<ItemSO>();
	}

	public void AddItem(ItemSO item) {
		//Add to Count
		items.Add(item);
	}

	public void RemoveItem(ItemSO item) {
		//Remove to Count
		items.Remove(item);
	}
}
