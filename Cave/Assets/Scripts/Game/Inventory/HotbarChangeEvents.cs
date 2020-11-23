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
			EventData eventData = new EventData("OnBuildingCreation");
			eventData["Building"] = item;
			EventManager.CallOnBuildingCreation(eventData);

			//GameObject building = Instantiate(BuildingsList.instance.GetItemPrefab(item),
			//GameManager.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity, gameObject.transform);
			//building.transform.position = new Vector3(building.transform.position.x, building.transform.position.y, 0);
			//building.name = "CreatedBuilding";
		}
	}
}
