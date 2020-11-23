using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundItemsList : MonoBehaviour {
	static public OnGroundItemsList instance;

	[SerializeField] GameObject[] OnGroundItemsPrefabs;
	ItemSO[] ItemsData;

	void Awake() {
		instance = this;

		ItemsData = new ItemSO[OnGroundItemsPrefabs.Length];
		for (ushort i = 0; i < OnGroundItemsPrefabs.Length; ++i)
			ItemsData[i] = OnGroundItemsPrefabs[i].GetComponent<OnGroundItem>().Item;
	}

	public GameObject GetItemPrefab(ItemSO item) {
		for(ushort i = 0; i < OnGroundItemsPrefabs.Length; ++i) {
			if (ItemsData[i].Type == item.Type)
				return OnGroundItemsPrefabs[i];
		}
		return null;
	}

}