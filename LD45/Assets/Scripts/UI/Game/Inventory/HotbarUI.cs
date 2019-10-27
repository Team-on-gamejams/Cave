using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: да тут же оверхед по памяті огромний. В InventoryUI є свої посилання на всі приватні поля, а тут вони же, але іншого типу
public class HotbarUI : InventoryUI {
	List<ItemSlotHotbar> itemSlotsHotbar = new List<ItemSlotHotbar>();
	Hotbar hotbar;
	byte lastSelection;

	protected override void Awake() {
		base.Awake();
		hotbar = Inventory as Hotbar;
		hotbar.OnSelectionChange.AddListener(UpdateSelectionFrame);
	}

	protected override void Start() {
		base.Start();

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
