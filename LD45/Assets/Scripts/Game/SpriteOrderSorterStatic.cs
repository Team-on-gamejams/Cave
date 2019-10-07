using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: fix by split sprite onto 2 parts. 
public class SpriteOrderSorterStatic : MonoBehaviour {
	[SerializeField] int Correction = 0;

	void Awake() {
		//TOOD: міряти по нижній грані, а не центрі. І позбутися Correction
		GetComponent<SpriteRenderer>().sortingOrder =  Mathf.FloorToInt(-transform.position.y) + Correction;
		Destroy(this);
	}
}
