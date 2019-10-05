using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {
	public float InteractDist;
	public System.Action OnMouseClick;

	[SerializeField] float outlineSize;

	SpriteOutline outline;
	float InteractDistSqr;

	protected virtual void Awake() {
		InteractDistSqr = InteractDist * InteractDist;
	}

	void OnMouseEnter() {
		if (outline == null) {
			outline = gameObject.AddComponent<SpriteOutline>();
			outline._outlineSize = outlineSize;
			outline.color = Color.yellow;
			outline.UpdateOutline(outline._outlineSize);
		}
		outline.enabled = true;
	}

	void OnMouseDown() {
		OnMouseClick?.Invoke();
	}

	void OnMouseExit() {
		outline.enabled = false;
	}

	public void SimulateMouseClick() {
		OnMouseDown();
	}

	public bool CanInteract() {
		return GameManager.Instance.Player.CanInteract(transform.position, InteractDistSqr);
	}
}
