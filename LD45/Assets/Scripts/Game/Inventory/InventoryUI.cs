using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour {
	public Inventory Inventory;

	internal bool isDrag;

	[SerializeField] protected bool NeedShowHide = true;
	[SerializeField] float ShowTime = 0.2f;

	protected List<ItemSlot> itemSlots;

	CanvasGroup canvasGroup;

	bool canChange = true;
	bool isShowed = false;

	virtual protected void Awake() {
		if (NeedShowHide) {
			canvasGroup = GetComponent<CanvasGroup>();
			canvasGroup.alpha = 0;
			gameObject.SetActive(false);
		}
		else {
			isShowed = true;
		}
		
		Inventory.OnItemsChanged.AddListener(UpdateUI);

		itemSlots = new List <ItemSlot>(Inventory.MaxSlots);
		ItemSlot[] items = GetComponentsInChildren<ItemSlot>();
		for(byte i = 0; i <items.Length; ++i) {
			itemSlots.Add(items[i]);
			items[i].invId = i;
		}
	}

	virtual protected void OnDestroy() {
		Inventory.OnItemsChanged.RemoveListener(UpdateUI);
	}

	public void ChangeShowHide() {
		if (!canChange || isDrag)
			return;
		if (isShowed)
			Hide();
		else
			Show();
	}

	public void Show() {
		if (isShowed)
			return;
		gameObject.SetActive(true);
		isShowed = true;
		canChange = false;
		UpdateUI();

		LeanTween.cancel(gameObject);
		LeanTween.value(gameObject, canvasGroup.alpha, 1.0f, ShowTime)
			.setOnUpdate((float a) => {
				canvasGroup.alpha = a;
			})
			.setOnComplete(()=> {
				canChange = true;
			});
	}

	public void Hide() {
		if (!isShowed)
			return;
		canChange = false;

		LeanTween.cancel(gameObject);
		LeanTween.value(gameObject, canvasGroup.alpha, 0.0f, ShowTime)
			.setOnUpdate((float a)=> {
				canvasGroup.alpha = a;
			})
			.setOnComplete(() => {
				canChange = true;
				isShowed = false;
				gameObject.SetActive(false);
			});
	}

	public virtual void UpdateUI() {
		if (!isShowed)
			return;

		for(byte i = 0; i < Inventory.Items.Length; ++i) 
			itemSlots[i].SetItem(Inventory.Items[i]);
	}
}
