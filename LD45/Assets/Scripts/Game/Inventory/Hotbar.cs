﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hotbar : Inventory {
	public byte SelectedSlotId;
	public UnityEvent OnSelectionChange;

	void Awake() {
		SelectedSlotId = 0;
	}

	void Start() {
		OnSelectionChange?.Invoke();
	}

	public override bool AddItem(ItemSO item) {
		bool rez = base.AddItem(item);
		SetSelection(SelectedSlotId);
		return rez;
	}

	public override void RemoveItem(ItemSO item) {
		base.RemoveItem(item);
		SetSelection(SelectedSlotId);
	}

	public void SetSelection(byte id) {
		SelectedSlotId = (byte)id;
		if (Items[SelectedSlotId] != null)
			GameManager.Instance.Player.Equipment.EquipItem(Items[SelectedSlotId], true);
		else
			GameManager.Instance.Player.Equipment.UnequipItem(ItemSO.ItemSlot.Hands);
		OnSelectionChange?.Invoke();
	}

	public void MoveSelectionUp() {
		++SelectedSlotId;
		if (SelectedSlotId == Items.Length)
			SelectedSlotId = 0;
		if (Items[SelectedSlotId] != null)
			GameManager.Instance.Player.Equipment.EquipItem(Items[SelectedSlotId], true);
		else
			GameManager.Instance.Player.Equipment.UnequipItem(ItemSO.ItemSlot.Hands);
		OnSelectionChange?.Invoke();
	}

	public void MoveSelectionDown() {
		--SelectedSlotId;
		if (SelectedSlotId == byte.MaxValue)
			SelectedSlotId = (byte)(Items.Length - 1);
		if (Items[SelectedSlotId] != null)
			GameManager.Instance.Player.Equipment.EquipItem(Items[SelectedSlotId], true);
		else
			GameManager.Instance.Player.Equipment.UnequipItem(ItemSO.ItemSlot.Hands);
		OnSelectionChange?.Invoke();
	}
}
