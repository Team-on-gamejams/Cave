using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: fix by split sprite onto 2 parts. 
public class SpriteOrderSorterStatic : MonoBehaviour {
	[SerializeField] int Correction = 0;

	void Awake() {
		GetComponent<SpriteRenderer>().sortingOrder = Mathf.FloorToInt(-transform.position.y) + Correction;
		Destroy(this);
	}
}
