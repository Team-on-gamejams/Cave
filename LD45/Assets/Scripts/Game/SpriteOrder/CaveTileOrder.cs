using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class CaveTileOrder : MonoBehaviour {
	public int sortingOrder;

	protected void Start() {
		StartCoroutine(SetOrder());
	}

	IEnumerator SetOrder() {
		yield return null;

		MeshRenderer renderer = GetComponent<MeshRenderer>();
		renderer.sortingLayerName = "Background";
		renderer.sortingOrder = sortingOrder;
		yield return null;

		Destroy(this);
	}
}