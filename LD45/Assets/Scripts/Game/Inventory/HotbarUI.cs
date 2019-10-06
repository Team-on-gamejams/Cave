using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarUI : InventoryUI {
	List<ItemSlotHotbar> itemSlotsHotbar = new List<ItemSlotHotbar>();
	Hotbar hotbar;
	byte lastSelection;

	protected override void Awake() {
		base.Awake();
		hotbar = Inventory as Hotbar;
		hotbar.OnSelectionChange.AddListener(UpdateSelectionFrame);

		foreach (var i in itemSlots) 
			itemSlotsHotbar.Add(i as ItemSlotHotbar);
		itemSlotsHotbar[(lastSelection = hotbar.SelectedSlotId)].SetSelectedFrame();
	}

	protected override void OnDestroy() {
		base.OnDestroy();
		hotbar.OnSelectionChange.RemoveListener(UpdateSelectionFrame);
	}

	void UpdateSelectionFrame() {
		itemSlotsHotbar[lastSelection].RemoveSelectedFrame();
		itemSlotsHotbar[(lastSelection = hotbar.SelectedSlotId)].SetSelectedFrame();
	}
}
