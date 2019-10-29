using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemSlotHotbar: ItemSlot, IPointerClickHandler {
	[SerializeField] Image SelectedFrameLeft;
	[SerializeField] Image SelectedFrameRight;
	[SerializeField] Image SelectedFrameDual;

	Hotbar hotbar;

	protected override void Awake() {
		base.Awake();
		hotbar = InventoryUI.Inventory as Hotbar;

		SelectedFrameLeft.gameObject.SetActive(false);
		SelectedFrameRight.gameObject.SetActive(false);
		SelectedFrameDual.gameObject.SetActive(false);
	}

	public override void SetItem(ItemSO _item) {
		base.SetItem(_item);

		if (hotbar.IsSelectedSlot(invId)) {
			if (item != null)
				GameManager.Instance.Player.Equipment.EquipItem(item, true, hotbar.SelectedSlotIdLeft == invId);
			else
				GameManager.Instance.Player.Equipment.UnequipItem(hotbar.SelectedSlotIdLeft == invId ? ItemSO.ItemSlot.HandLeft : ItemSO.ItemSlot.HandRight);
		}
	}

	public void OnPointerClick(PointerEventData eventData) {
		if (!GameManager.Instance.IsPaused) {
			if(eventData.button == PointerEventData.InputButton.Left)
				hotbar.SetSelection(invId, true);
			else if (eventData.button == PointerEventData.InputButton.Right)
				hotbar.SetSelection(invId, false);
			else if (eventData.button == PointerEventData.InputButton.Middle) {
				hotbar.SetSelection(invId, true);
				hotbar.SetSelection(invId, false);
			}
		}
	}

	public void SetSelectedFrame(bool isLeftHand) {
		if(
			(isLeftHand && SelectedFrameRight.gameObject.activeSelf) ||
			(!isLeftHand && SelectedFrameLeft.gameObject.activeSelf)
		) {
			(!isLeftHand ? SelectedFrameLeft : SelectedFrameRight).gameObject.SetActive(false);
			SelectedFrameDual.gameObject.SetActive(true);
		}
		else {
			(isLeftHand ? SelectedFrameLeft : SelectedFrameRight).gameObject.SetActive(true);
		}
	}

	public void RemoveSelectedFrame(bool isLeftHand) {
		if(SelectedFrameDual.gameObject.activeSelf) {
			SelectedFrameDual.gameObject.SetActive(false);
			(!isLeftHand ? SelectedFrameLeft : SelectedFrameRight).gameObject.SetActive(true);
		}

		(isLeftHand ? SelectedFrameLeft : SelectedFrameRight).gameObject.SetActive(false);
	}
}
