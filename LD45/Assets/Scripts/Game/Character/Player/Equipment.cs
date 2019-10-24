using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour {
	public SpriteRenderer ItemInHand;

	[NonSerialized] public GameObject GOLinkedAnim;
	[NonSerialized] public ItemSO hands;
	public Action OnUseHandAnimEndEvent;

	Animator animator;

    //private bool IsBuildingEquip;
    //private GameObject Test;

	private void Start() {
		animator = GameManager.Instance.Player.Animator;
	}

 //   private void Update() {
 //       if(IsBuildingEquip) {
	//		var pos = GameManager.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
 //           Test.transform.position = pos;
	//		Debug.Log("X on canvas : " + Test.transform.position.x + " Y on canvas : " + Test.transform.position.y);
	//		Debug.Log("X : " + pos.x + " Y : " + pos.y);

	//	}
	//}

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
            //CheckForBuilding(item);
        }
	}

	public bool NeedInterrupt() {
		return OnUseHandAnimEndEvent != null;
	}

 //   private void CheckForBuilding(ItemSO item)
 //   {
	//	//Todo buildings
	//	//1) Fix bug with don's set enable to false (invisible object let fors for Player)
	//	//2) Fix bug with no visibility of creating GameObjec
	//	if (item.MataType is ItemSO.ItemMetaType.Building && hands != null) {
	//		IsBuildingEquip = true;
	//		Test = Instantiate(BuildingsList.instance.GetItemPrefab(item), Input.mousePosition, Quaternion.identity, GameManager.Instance.CollectorBuilding.transform);
	//		Test.GetComponent<BoxCollider2D>().enabled = false;
			
	//		//Test.GetComponent<Renderer>().material.color = Color.;
	//	}
	//	else {
	//		Debug.Log("IsBuildingEquip: " + IsBuildingEquip + " in else");
	//		IsBuildingEquip = false;
	//	}
	//}

	public void InterruptAction() {
		OnUseHandAnimEndEvent = null;
		ItemInHand.enabled = true;
		GOLinkedAnim = null;
	}

	void OnStartAnim() {
		ItemInHand.enabled = false;
	}
}
