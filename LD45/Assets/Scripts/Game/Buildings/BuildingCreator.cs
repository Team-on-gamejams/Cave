using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCreator : MonoBehaviour
{
	private void SetPosition(GameObject @object) { //Need to be like a updete with turn on and turn off
		var pos = GameManager.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
		@object.transform.position = pos;
	}
}
