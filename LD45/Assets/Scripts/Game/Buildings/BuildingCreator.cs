using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCreator : MonoBehaviour
{
	public void Awake() {
		EventManager.OnBuildingCreation += Creator;
	}

	public void OnDestroy() {
		EventManager.OnBuildingCreation -= Creator;
	}

	public void SetPosition(GameObject @object) { //Need to be like a updete with turn on and turn off
        Vector3 pos = GameManager.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log(Input.mousePosition);
        pos.y = 0;
		@object.transform.position = pos;
    }

	private void Creator(EventData ed) {
		Debug.Log(ed.Data["Building"]);
	}

	public void OnMouseDown() {
		Debug.Log("123");
	}
}
