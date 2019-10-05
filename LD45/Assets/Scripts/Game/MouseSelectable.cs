using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSelectable : MonoBehaviour {
	[SerializeField] float outlineSize;

	SpriteOutline outline;

	private void OnMouseEnter() {
		if(outline == null) {
			outline = gameObject.AddComponent<SpriteOutline>();
			outline._outlineSize = outlineSize;
			outline.color = Color.yellow;
			outline.UpdateOutline(outline._outlineSize);
		}
		outline.enabled = true;
	}

	private void OnMouseExit() {
		outline.enabled = false;
	}
}
