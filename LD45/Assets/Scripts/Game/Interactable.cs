using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {
	public GameObject OutlineGO;
	public float InteractDist;
	public System.Action OnMouseClick;

	[SerializeField] float outlineSize;

	SpriteOutline outline;
	float InteractDistSqr;

	protected virtual void Awake() {
		InteractDistSqr = InteractDist * InteractDist;
		//TODO: fix outline for forest
		//if (OutlineGO == null)
			OutlineGO = gameObject;
	}

	void OnMouseEnter() {
		if (outline == null) {
			outline = OutlineGO.AddComponent<SpriteOutline>();
			outline._outlineSize = outlineSize;
			outline.color = Color.yellow;
			outline.UpdateOutline(outline._outlineSize);
		}
		outline.enabled = true;
	}

	void OnMouseDown() {
		if (CanInteract()) {
			OnMouseClick?.Invoke();
		}
		else {
			GameManager.Instance.Player.PlayerKeyboardMover.OnMouseMoveEnd = null;
			GameManager.Instance.Player.PlayerKeyboardMover.OnMouseMoveEnd += ()=> OnMouseClick?.Invoke();
		}
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
