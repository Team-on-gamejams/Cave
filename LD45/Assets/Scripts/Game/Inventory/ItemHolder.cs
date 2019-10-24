using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour {
	void Start() {
		GameManager.Instance.CollectorItems = gameObject;
	}
}
