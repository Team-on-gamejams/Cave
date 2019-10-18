using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundItem : Interactable {
	public ItemSO Item;

	protected override void Awake() {
		base.Awake();
		OnMouseClick += CollectItem;
		//TODO: not sure that it shoud be there
		Item = Instantiate(Item);
	}

	void CollectItem() {
		//TODO: add fly animation
		//TODO: or show special frame around new items
		//TODO: Play pickup anim

		GameManager.Instance.Player.Equipment.GOLinkedAnim = null;
		if (GameManager.Instance.Player.Inventory.AddItem(Item))
			Destroy(gameObject);
	}

	static public OnGroundItem CreateOnGround(ItemSO item, Vector3 pos, Transform parent) {
		pos.z = 0;

		GameObject go = new GameObject(item.Name, new Type[] { typeof(OnGroundItem) });
		go.transform.parent = parent;
		go.transform.position = pos;
		OnGroundItem ogi = go.GetComponent<OnGroundItem>();
		ogi.Item = item;
		return ogi;
	}
}
