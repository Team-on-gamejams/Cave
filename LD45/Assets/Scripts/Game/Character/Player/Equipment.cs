using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour {
	public SpriteRenderer ItemInHand;

	public GameObject GOLinkedAnim;

	public ItemSO hands;
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

	public void OnUseHandAnimEnd() {
		if(OnUseHandAnimEndEvent != null) {
			OnUseHandAnimEndEvent();
			OnUseHandAnimEndEvent = null;
			ItemInHand.enabled = true;
		}
	}

	public void EquipItem(ItemSO item) {
		//TODO: also add equip for armor
		GameManager.Instance.Player.InterruptAction();
		hands = item;
		if(hands == null) {
			ItemInHand.enabled = false;
		}
		else {
			ItemInHand.transform.localScale = new Vector3(hands.ScaleInHand, hands.ScaleInHand, 1.0f);
			ItemInHand.sprite = hands.Sprite;
			ItemInHand.enabled = true;
			CallEvent(item);
		}
	}

	private void CallEvent(ItemSO item) {
		EventData eventData = new EventData("OnItemSlotChange");
		eventData["ItemSlotType"] = item.Type;
		GameManager.Instance.EventManager.CallOnEquipmentChange(eventData);
	}

	public bool NeedInterrupt() {
		return OnUseHandAnimEndEvent != null;
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
