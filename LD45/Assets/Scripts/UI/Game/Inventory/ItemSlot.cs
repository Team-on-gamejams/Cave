using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

//TODO: переробити на людський поліморфізм, а не чекати типи
public class ItemSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler {
	static protected ItemSlot draggingSlot;

	[NonSerialized] public int invId;

	[SerializeField] Image ItemImage;
	[SerializeField] TextMeshProUGUI CountText;

	protected InventoryUI InventoryUI;
	protected ItemSO item;

	virtual protected void Awake() {
		CountText.gameObject.SetActive(false);
		ItemImage.gameObject.SetActive(false);

		InventoryUI = transform.parent.GetComponent<InventoryUI>();
	}

	public virtual void SetItem(ItemSO _item) {
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
	}

	public void OnBeginDrag(PointerEventData eventData) {
		if (GameManager.Instance.IsPaused || item == null)
			return;

		draggingSlot = this;
		PlayerInputFlyweight.CanMouseMove = false;
		InventoryUI.isDrag = true;

		ItemImage.transform.SetParent(InventoryUI.ParentForDraggedSlot.transform, true);
		CountText.transform.SetParent(InventoryUI.ParentForDraggedSlot.transform, true);
		ItemImage.raycastTarget = false;
		CountText.raycastTarget = false;

		ItemImage.transform.position += (Vector3)eventData.delta;
		CountText.transform.position += (Vector3)eventData.delta;
	}

	public void OnDrag(PointerEventData eventData) {
		if (GameManager.Instance.IsPaused || item == null)
			return;

		ItemImage.transform.position += (Vector3)eventData.delta;
		CountText.transform.position += (Vector3)eventData.delta;
	}

	public void OnEndDrag(PointerEventData eventData) {
		if (GameManager.Instance.IsPaused || item == null)
			return;
		PlayerInputFlyweight.CanMouseMove = true;

		ReInit();

		if (eventData.hovered.Count != 0)
			return;

		Vector3 newItemPos = GameManager.Instance.MainCamera.ScreenToWorldPoint(eventData.position);
		GameManager.Instance.Player.InterruptAction();
		GameManager.Instance.Player.Equipment.GOLinkedAnim = gameObject;

		if (GameManager.Instance.Player.CanInteract(newItemPos)) {
			DropItemOnGround(newItemPos);
		}
		else {
			GameManager.Instance.Player.MoveTo(newItemPos);
			GameManager.Instance.Player.OnMoveEndEvent += () => DropItemOnGround(newItemPos);
		}
	}

	public void OnDrop(PointerEventData eventData) {
		PlayerInputFlyweight.CanMouseMove = true;

		if (!CanDrop(draggingSlot?.item))
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
	}

	public virtual bool CanDrop(ItemSO item) {
		return draggingSlot != this && item != null;
	}

	void DropItemOnGround(Vector3 newItemPos) {
		OnGroundItem.CreateOnGround(item, newItemPos, GameManager.Instance.CollectorItems.transform);
		InventoryUI.Inventory.Items[invId] = null;
		InventoryUI.UpdateUI();
	}

	void ReInit() {
		InventoryUI.isDrag = false;
		ItemImage.transform.SetParent(transform, true);
		CountText.transform.SetParent(transform, true);
		ItemImage.transform.localPosition = Vector3.zero;
		CountText.rectTransform.anchoredPosition = Vector3.zero;
		ItemImage.raycastTarget = true;
		CountText.raycastTarget = true;
	}

	public static void OnPause() {
		draggingSlot?.ReInit();
		draggingSlot = null;
	}
}
