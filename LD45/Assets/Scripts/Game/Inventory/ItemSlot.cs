using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler {
	static ItemSlot draggingSlot;

	internal int invId;

	[SerializeField] Image ItemImage;
	[SerializeField] TextMeshProUGUI CountText;

	GameObject canvas;
	InventoryUI InventoryUI;

	ItemSO item;

	Vector3 mouseOffsetImage;
	Vector3 mouseOffsetCount;

	void Awake() {
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
	}

	public void OnBeginDrag(PointerEventData eventData) {
		draggingSlot = this;

		if (item == null)
			return;

		ItemImage.transform.SetParent(canvas.transform, true);
		CountText.transform.SetParent(canvas.transform, true);

		mouseOffsetImage = ItemImage.transform.position - Input.mousePosition;
		mouseOffsetCount = CountText.transform.position - Input.mousePosition;
	}

	public void OnDrag(PointerEventData eventData) {
		if (item == null)
			return;

		ItemImage.transform.position = Input.mousePosition + mouseOffsetImage;
		CountText.transform.position = Input.mousePosition + mouseOffsetCount;

		ItemImage.raycastTarget = false;
	}

	public void OnEndDrag(PointerEventData eventData) {
		if (item == null)
			return;

		ReInit();
	}

	public void OnDrop(PointerEventData eventData) {
		if (draggingSlot == this || draggingSlot.item == null)
			return;

		if (item == null || item.Type != draggingSlot.item.Type) {
			InventoryUI.Inventory.Items[invId] = draggingSlot.InventoryUI.Inventory.Items[draggingSlot.invId];
			draggingSlot.InventoryUI.Inventory.Items[draggingSlot.invId] = null;
		}

		ReInit();
		draggingSlot.ReInit();
		InventoryUI.UpdateUI();
		draggingSlot.InventoryUI.UpdateUI();
	}

	void CheckDrop() {

	}

	void ReInit() {
		ItemImage.transform.SetParent(transform, true);
		CountText.transform.SetParent(transform, true);
		ItemImage.transform.localPosition = Vector3.zero;
		CountText.rectTransform.anchoredPosition = Vector3.zero;
		ItemImage.raycastTarget = true;
	}
}
