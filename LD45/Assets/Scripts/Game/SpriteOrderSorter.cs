﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: fix by split sprite onto 2 parts. 
public class SpriteOrderSorter : MonoBehaviour {
	SpriteRenderer spriteRenderer;

	void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void Update() {
		//TOOD: міряти по нижній грані, а не центрі.
		spriteRenderer.sortingOrder = Mathf.FloorToInt(-transform.position.y);
	}
}
