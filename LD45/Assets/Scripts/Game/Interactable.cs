using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {
	public GameObject AdditionalOutlineGO;
	public float OutlineScale = 1;
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
			outline = CreateOutline(gameObject, true);
			if (AdditionalOutlineGO) {
				outlineAdditional = CreateOutline(AdditionalOutlineGO, false);
			}
		}
		outline.gameObject.SetActive(true);
		if (AdditionalOutlineGO)
			AdditionalOutlineGO.gameObject.SetActive(true);
	}

	void OnMouseDown() {
		if (CanInteract()) {
			OnMouseClick?.Invoke();
		}
		else {
			GameManager.Instance.Player.PlayerKeyboardMover.OnMouseMoveEnd = null;
			GameManager.Instance.Player.PlayerKeyboardMover.OnMouseMoveEnd += () => {
				if (this != null && CanInteract()) {
					OnMouseClick?.Invoke();
				}
				//else {
				//	GameManager.Instance.Player.PlayerKeyboardMover.OnMouseMoveEnd = null;
				//}
			};
		}
	}

	void OnMouseExit() {
		outline.gameObject.SetActive(false);
		if (AdditionalOutlineGO)
			AdditionalOutlineGO.gameObject.SetActive(false);
	}

	public void SimulateMouseClick() {
		OnMouseDown();
	}

	public bool CanInteract() {
		return GameManager.Instance.Player.CanInteract(transform.position, InteractDistSqr);
	}

	SpriteOutline CreateOutline(GameObject parentGO, bool needScale) {
		var gameObject = new GameObject() {
			name = $"{parentGO.name}-outline",
		};
		gameObject.transform.parent = parentGO.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = new Vector3(needScale ? OutlineScale : 1, needScale ? OutlineScale : 1, 1f);

		SpriteRenderer parentsr = parentGO.GetComponent<SpriteRenderer>();
		SpriteRenderer sr = gameObject.AddComponent<SpriteRenderer>();
		sr.sprite = parentsr.sprite;
		sr.sortingOrder = parentsr.sortingOrder - 1;

		SpriteOutline outline = gameObject.AddComponent<SpriteOutline>();
		outline._outlineSize = outlineSize;
		outline.color = Color.yellow;
		outline.UpdateOutline(outline._outlineSize);
		return outline;
	}
}
