using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour {
	public UnityEvent OnItemsChanged;

	public byte MaxSlots;
	public ItemSO[] Items;

	public bool AddItem(ItemSO item) {
		bool addAllItems = item.Count == 0;
		ushort startCount = item.Count;

		for (byte i = 0; i < Items.Length; ++i) {
			if(Items[i] != null && Items[i].Type == item.Type) {
				if(Items[i].Count != Items[i].MaxCount) {
					Items[i].Count += item.Count;
					item.Count = 0;
					if (Items[i].Count > Items[i].MaxCount) {
						item.Count = (byte)(Items[i].Count - Items[i].MaxCount);
						Items[i].Count = Items[i].MaxCount;
					}
				}

				if(item.Count == 0)
					addAllItems = true;
			}
		}

		if (!addAllItems) {
			short firstFreePos = -1;
			for (byte i = 0; i < Items.Length; ++i) {
				if(Items[i] == null) {
					firstFreePos = i;
					break;
				}
			}

			if(firstFreePos != -1) {
				Items[firstFreePos] = item;
				addAllItems = true;
				startCount = 0;
			}
		}

		if (startCount != item.Count)
			OnItemsChanged?.Invoke();

		return addAllItems;
	}

	public void RemoveItem(ItemSO item) {
		//TODO: Remove to Count
		//Items.Remove(item);
	}
}
