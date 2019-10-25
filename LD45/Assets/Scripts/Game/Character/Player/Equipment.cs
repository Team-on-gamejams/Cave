using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour {
	public SpriteRenderer ItemInHand;

	[NonSerialized] public GameObject GOLinkedAnim;
	[NonSerialized] public ItemSO hands;
	[NonSerialized] public ItemSO head;
	[NonSerialized] public ItemSO armor;
	public Action OnUseHandAnimEndEvent;

	Animator animator;


	private void Start() {
		animator = GameManager.Instance.Player.Animator;
	}

    public void PlayUseHandItemAnim() {
		if(hands != null) {
			switch (hands.Type) {
				case ItemSO.ItemType.Axe:
					OnStartAnim();
					animator.Play("AxePunch");
					break;
				case ItemSO.ItemType.Pickaxe:
					OnStartAnim();
					animator.Play("PickaxePunch");
					break;
				case ItemSO.ItemType.Spear:
					OnStartAnim();
					animator.Play("SpearPunch");
					break;                              
				default:
					//TODO: show that it cant be used
					break;
			}
		}
		//else {
		//	//TODO: Assume that it will use for pick-up anims
		//}
	}

	//Callback for animations
	public void OnUseHandAnimEnd() {
		if(OnUseHandAnimEndEvent != null) {
			OnUseHandAnimEndEvent();
			OnUseHandAnimEndEvent = null;
			ItemInHand.enabled = true;
		}
	}

	public void EquipItem(ItemSO item, bool forceHand = false) {
		GameManager.Instance.Player.InterruptAction();
		if (item == null)
			return;

		if (forceHand) {
			EquipInHand(item);
		}
		else {
			switch (item.Slot) {
				case ItemSO.ItemSlot.None:
				case ItemSO.ItemSlot.Hands:
					EquipInHand(item);
					break;

				case ItemSO.ItemSlot.Head:
					head = item;
					break;

				case ItemSO.ItemSlot.Armor:
					armor = item;
					break;
			}
		}
	}

	public void UnequipItem(ItemSO.ItemSlot slot) {
		switch (slot) {
			case ItemSO.ItemSlot.None:
			case ItemSO.ItemSlot.Hands:
				hands = null;
				ItemInHand.enabled = false;
				break;
			case ItemSO.ItemSlot.Head:
				head = null;
				break;
			case ItemSO.ItemSlot.Armor:
				armor = null;
				break;
		}
	}

	public bool NeedInterrupt() {
		return OnUseHandAnimEndEvent != null;
	}

	void EquipInHand(ItemSO item) {
		hands = item;
		ItemInHand.transform.localScale = new Vector3(hands.ScaleInHand, hands.ScaleInHand, 1.0f);
		ItemInHand.sprite = hands.Sprite;
		ItemInHand.enabled = true;
		
		EventData eventData = new EventData("OnItemSlotChange");
		eventData["ItemSlotType"] = item;
		GameManager.Instance.EventManager.CallOnEquipmentChange(eventData);
	}
	
	public void InterruptAction() {
		OnUseHandAnimEndEvent = null;
		ItemInHand.enabled = true;
		GOLinkedAnim = null;
	}

	void OnStartAnim() {
		ItemInHand.enabled = false;
	}
}
