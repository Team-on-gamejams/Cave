using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundItem : Interactable {
	public ItemSO Item;

	protected override void Awake() {
		base.Awake();
		//TODO: not sure that it shoud be there
		Item = Instantiate(Item);
        tip = $"Pickup {Item.Name}";

		OnMouseClick += CollectItem;
	}

	protected override void Start() {
		base.Start();
		Chunk chunk = WorldGenerator.instance.GetChunkFromWorldPos(transform.position);
		transform.parent = chunk.transform;
		chunk.onGroundItems.Add(this);
	}

	private void OnDestroy() {
		WorldGenerator.instance.GetChunkFromWorldPos(transform.position).onGroundItems.Remove(this);
	}

	void CollectItem() {
		//TODO: add fly animation
		//TODO: or show special frame around new items
		//TODO: Play pickup anim

		GameManager.Instance.Player.Equipment.GOLinkedAnim = null;
		if (GameManager.Instance.Player.Inventory.AddItem(Item))
			Destroy(gameObject);
	}

	static public OnGroundItem CreateOnGround(ItemSO item, Vector3 pos) {
		pos.z = 0;
		GameObject go = Instantiate(OnGroundItemsList.instance.GetItemPrefab(item), pos, Quaternion.identity);
		OnGroundItem newItem = go.GetComponent<OnGroundItem>();
		newItem.Item.Count = item.Count;
		return newItem;
	}
}
