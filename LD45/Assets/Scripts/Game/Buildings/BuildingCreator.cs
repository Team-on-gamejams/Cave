using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCreator : MonoBehaviour
{
	public void SetPosition(GameObject @object) { //Need to be like a updete with turn on and turn off
        Vector3 pos = GameManager.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log(Input.mousePosition);
        pos.y = 0;
		@object.transform.position = pos;
    }
}
