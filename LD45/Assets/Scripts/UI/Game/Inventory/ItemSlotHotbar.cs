using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemSlotHotbar: ItemSlot, IPointerClickHandler {
	[SerializeField] Image SelectedFrame;

	Hotbar hotbar;

	protected override void Awake() {
		base.Awake();
		hotbar = InventoryUI.Inventory as Hotbar;

		SelectedFrame.gameObject.SetActive(false);
	}

	public override void SetItem(ItemSO _item) {
		base.SetItem(_item);

		if (hotbar.SelectedSlotId == invId) {
			if(item != null)
				GameManager.Instance.Player.Equipment.EquipItem(item, true);
			else
				GameManager.Instance.Player.Equipment.UnequipItem(ItemSO.ItemSlot.Hands);
		}
	}

	public void OnPointerClick(PointerEventData eventData) {
		if(!GameManager.Instance.IsPaused)
			hotbar.SetSelection((byte)invId);
	}

	public void SetSelectedFrame() {
		SelectedFrame.gameObject.SetActive(true);
	}

	public void RemoveSelectedFrame() {
		SelectedFrame.gameObject.SetActive(false);
	}
}
