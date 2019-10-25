using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarChangeEvents : MonoBehaviour //Todo rename behaviour
{
	void Awake() {
		EventManager.OnEquipmentChange += OnEquipmentChange;
	}

	void OnDestroy() {
		EventManager.OnEquipmentChange += OnEquipmentChange;
	}

	private void OnEquipmentChange(EventData data) {		
		var item = data["ItemSlotType"] as ItemSO;
		if (item is null == false && item.MataType is ItemSO.ItemMetaType.Building) {

			//GameObject building = ItemSO.
			//building = Instantiate(BuildingsList.instance.GetItemPrefab(item), Input.mousePosition, Quaternion.identity, GameManager.Instance.CollectorBuilding.transform);
			//building.GetComponent<BoxCollider2D>().enabled = false;

			//Test.GetComponent<Renderer>().material.color = Color.;
		}
	}
}
