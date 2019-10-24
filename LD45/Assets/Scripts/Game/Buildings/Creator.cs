using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creator : MonoBehaviour //Todo rename behaviour
{
	void Awake() {
		EventManager.OnEquipmentChange += OnEquipmentChange;
	}

	void OnDestroy() {
		EventManager.OnEquipmentChange += OnEquipmentChange;
	}


	//Todo buildings
	//1) Fix bug with don's set enable to false (invisible object let fors for Player)
	//2) Fix bug with no visibility of creating GameObjec
	private void OnEquipmentChange(EventData data) {
		Debug.Log("123");
		
		Debug.Log((data["ItemSlotType"]).GetType());
		var item = data["ItemSlotType"] as ItemSO;
		if (item is null == false && item.MataType is ItemSO.ItemMetaType.Building) {
			Debug.Log($"{item.Description}");
			Debug.Log($"{item.name}");
			//GameObject building = ItemSO.
			//building = Instantiate(BuildingsList.instance.GetItemPrefab(item), Input.mousePosition, Quaternion.identity, GameManager.Instance.CollectorBuilding.transform);
			//building.GetComponent<BoxCollider2D>().enabled = false;

			//Test.GetComponent<Renderer>().material.color = Color.;
		}
	}

	private void SetPosition(GameObject @object) { //Need to be like a updete with turn on and turn off
		var pos = GameManager.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
		@object.transform.position = pos;
	}
}
