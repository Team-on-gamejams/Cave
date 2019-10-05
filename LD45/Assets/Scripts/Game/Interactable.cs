using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {
	public GameObject AdditionalOutlineGO;
	public float InteractDist;
	public System.Action OnMouseClick;

	[SerializeField] float outlineSize;

	SpriteOutline outlineAdditional;
	SpriteOutline outline;
	float InteractDistSqr;

	protected virtual void Awake() {
		InteractDistSqr = InteractDist * InteractDist;
	}

	void OnMouseEnter() {
		if (outline == null) {
			outline = CreateOutline(gameObject);
			if (AdditionalOutlineGO) {
				outlineAdditional = CreateOutline(AdditionalOutlineGO);
			}
		}
		outline.enabled = true;
		if (AdditionalOutlineGO)
			outlineAdditional.enabled = true;
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
		if (AdditionalOutlineGO)
			outlineAdditional.enabled = false;
	}

	public void SimulateMouseClick() {
		OnMouseDown();
	}

	public bool CanInteract() {
		return GameManager.Instance.Player.CanInteract(transform.position, InteractDistSqr);
	}

	SpriteOutline CreateOutline(GameObject gameObject) {
		var outline = gameObject.AddComponent<SpriteOutline>();
		outline._outlineSize = outlineSize;
		outline.color = Color.yellow;
		outline.UpdateOutline(outline._outlineSize);
		return outline;
	}
}
