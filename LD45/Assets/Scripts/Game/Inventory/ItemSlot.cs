using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

//TODO: переробити на людський поліморфізм, а не чекати типи
public class ItemSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler {
	static ItemSlot draggingSlot;

	internal int invId;

	[SerializeField] Image ItemImage;
	[SerializeField] TextMeshProUGUI CountText;

	GameObject canvas;
	InventoryUI InventoryUI;

	ItemSO item;

	virtual protected void Awake() {
		CountText.gameObject.SetActive(false);
		ItemImage.gameObject.SetActive(false);

		//TODO: нормально передвавати канвас
		canvas = gameObject.transform.parent.parent.gameObject;

		InventoryUI = gameObject.transform.parent.gameObject.GetComponent<InventoryUI>();
	}

	public void SetItem(ItemSO _item) {
		item = _item;
		if (item == null) {
			CountText.gameObject.SetActive(false);
			ItemImage.gameObject.SetActive(false);
			return;
		}

		ItemImage.sprite = item.Sprite;
		ItemImage.gameObject.SetActive(true);

		if (item.MaxCount != 1) {
			CountText.text = item.Count.ToString(); ;
			CountText.gameObject.SetActive(true);
		}
		else {
			CountText.gameObject.SetActive(false);
		}

		if (InventoryUI.Inventory is Hotbar) {
			if ((InventoryUI.Inventory as Hotbar).SelectedSlotId == invId)
				GameManager.Instance.Player.Equipment.EquipItem(InventoryUI.Inventory.Items[invId]);
		}
	}

	public void OnBeginDrag(PointerEventData eventData) {
		draggingSlot = this;
		GameManager.Instance.Player.PlayerKeyboardMover.CanMouseMove = false;
		InventoryUI.isDrag = true;

		if (item == null)
			return;

		ItemImage.transform.SetParent(canvas.transform, true);
		CountText.transform.SetParent(canvas.transform, true);
		ItemImage.raycastTarget = false;

		ItemImage.transform.position += (Vector3)eventData.delta;
		CountText.transform.position += (Vector3)eventData.delta;
	}

	public void OnDrag(PointerEventData eventData) {
		if (item == null)
			return;

		ItemImage.transform.position += (Vector3)eventData.delta;
		CountText.transform.position += (Vector3)eventData.delta;
	}

	public void OnEndDrag(PointerEventData eventData) {
		GameManager.Instance.Player.PlayerKeyboardMover.CanMouseMove = true;

		if (item == null || eventData.hovered.Count != 0)
			return;

		OnGroundItem.CreateOnGround(item, GameManager.Instance.MainCamera.ScreenToWorldPoint(eventData.position), GameManager.Instance.CollectorItems.transform);

		ReInit();
		InventoryUI.Inventory.Items[invId] = null;
		InventoryUI.UpdateUI();
		if (InventoryUI.Inventory is Hotbar) {
			if ((InventoryUI.Inventory as Hotbar).SelectedSlotId == invId)
				GameManager.Instance.Player.Equipment.EquipItem(null);
		}
	}

	public void OnDrop(PointerEventData eventData) {
		GameManager.Instance.Player.PlayerKeyboardMover.CanMouseMove = true;

		if (draggingSlot == this || draggingSlot.item == null)
			return;

		if (item == null || (item.Type != draggingSlot.item.Type) || item.IsMaxStack() || draggingSlot.item.IsMaxStack()) {
			ItemSO prevItem = InventoryUI.Inventory.Items[invId];
			InventoryUI.Inventory.Items[invId] = draggingSlot.InventoryUI.Inventory.Items[draggingSlot.invId];
			draggingSlot.InventoryUI.Inventory.Items[draggingSlot.invId] = prevItem;
		}
		else if(item.Type == draggingSlot.item.Type) {
			ItemSO slotItem = InventoryUI.Inventory.Items[invId];
			ItemSO dragItem = draggingSlot.InventoryUI.Inventory.Items[draggingSlot.invId];

			slotItem.Count += dragItem.Count;
			if (slotItem.Count > slotItem.MaxCount) {
				dragItem.Count = (ushort)(slotItem.Count - slotItem.MaxCount);
				slotItem.Count = slotItem.MaxCount;
			}
			else {
				draggingSlot.InventoryUI.Inventory.Items[draggingSlot.invId] = null;
			}
		}

		ReInit();
		draggingSlot.ReInit();
		InventoryUI.UpdateUI();
		draggingSlot.InventoryUI.UpdateUI();

		if (InventoryUI.Inventory is Hotbar) {
			if((InventoryUI.Inventory as Hotbar).SelectedSlotId == invId)
				GameManager.Instance.Player.Equipment.EquipItem(InventoryUI.Inventory.Items[invId]);
		}
		if (draggingSlot.InventoryUI.Inventory is Hotbar) {
			if ((draggingSlot.InventoryUI.Inventory as Hotbar).SelectedSlotId == draggingSlot.invId)
				GameManager.Instance.Player.Equipment.EquipItem(null);
		}
	}

	void ReInit() {
		InventoryUI.isDrag = false;
		ItemImage.transform.SetParent(transform, true);
		CountText.transform.SetParent(transform, true);
		ItemImage.transform.localPosition = Vector3.zero;
		CountText.rectTransform.anchoredPosition = Vector3.zero;
		ItemImage.raycastTarget = true;
	}
}
