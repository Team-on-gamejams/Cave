using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hotbar : Inventory {
	public byte SelectedSlotIdLeft;
	public byte SelectedSlotIdRight;
	public UnityEvent OnSelectionChange;

	void Awake() {
		SelectedSlotIdLeft = 0;
		SelectedSlotIdRight =(byte)(Items.Length - 1);
	}

	void Start() {
		OnSelectionChange?.Invoke();
	}

	public override bool AddItem(ItemSO item) {
		bool rez = base.AddItem(item);
		SetSelection(SelectedSlotIdLeft, true);
		SetSelection(SelectedSlotIdRight, false);
		return rez;
	}

	public override void RemoveItem(ItemSO item) {
		base.RemoveItem(item);
		SetSelection(SelectedSlotIdLeft, true);
		SetSelection(SelectedSlotIdRight, false);
	}

	public void SetSelection(byte id, bool isLeftHand) {
		if(isLeftHand)
			SelectedSlotIdLeft = (byte)id;
		else
			SelectedSlotIdRight = (byte)id;

		if (Items[id] != null)
			GameManager.Instance.Player.Equipment.EquipItem(Items[id], true, isLeftHand);
		else
			GameManager.Instance.Player.Equipment.UnequipItem(isLeftHand ? ItemSO.ItemSlot.HandLeft : ItemSO.ItemSlot.HandRight);
		OnSelectionChange?.Invoke();
	}

	public void MoveSelectionUp(bool isLeftHand) {
		byte id = isLeftHand ? SelectedSlotIdLeft : SelectedSlotIdRight;
		++id;
		if (id == Items.Length)
			id = 0;
		if (isLeftHand)
			SelectedSlotIdLeft = id;
		else
			SelectedSlotIdRight = id;

		if (Items[id] != null)
			GameManager.Instance.Player.Equipment.EquipItem(Items[id], true, isLeftHand);
		else
			GameManager.Instance.Player.Equipment.UnequipItem(isLeftHand ? ItemSO.ItemSlot.HandLeft : ItemSO.ItemSlot.HandRight);
		OnSelectionChange?.Invoke();
	}

	public void MoveSelectionDown(bool isLeftHand) {
		byte id = isLeftHand ? SelectedSlotIdLeft : SelectedSlotIdRight;
		--id;
		if (id == byte.MaxValue)
			id = (byte)(Items.Length - 1);
		if (isLeftHand)
			SelectedSlotIdLeft = id;
		else
			SelectedSlotIdRight = id;

		if (Items[id] != null)
			GameManager.Instance.Player.Equipment.EquipItem(Items[id], true, isLeftHand);
		else
			GameManager.Instance.Player.Equipment.UnequipItem(isLeftHand ? ItemSO.ItemSlot.HandLeft : ItemSO.ItemSlot.HandRight);
		OnSelectionChange?.Invoke();
	}

	public bool IsSelectedSlot(byte id) {
		return id == SelectedSlotIdLeft || id == SelectedSlotIdRight;
	}
}
