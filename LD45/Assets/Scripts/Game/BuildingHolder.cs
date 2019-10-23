using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHolder : MonoBehaviour {
	void Start() {
		GameManager.Instance.CollectorBuilding = gameObject;
	}
}
