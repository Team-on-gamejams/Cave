using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemSlotEquipment : ItemSlot {
	[SerializeField] ItemSO.ItemSlot slot;

	protected override void Awake() {
		base.Awake();

		InventoryUI = transform.parent.parent.GetComponent<InventoryUI>();
	}

	public override void SetItem(ItemSO _item) {
		base.SetItem(_item);

		if (item != null)
			GameManager.Instance.Player.Equipment.EquipItem(item);
		else
			GameManager.Instance.Player.Equipment.UnequipItem(slot);
	}

	public override bool CanDrop(ItemSO item) {
		return base.CanDrop(item) && item.Slot == slot;
	}
}
