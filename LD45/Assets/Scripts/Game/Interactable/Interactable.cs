using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Всі класи, що наслідуються мають всередині виконувати   
// GameManager.Instance.Player.Equipment.GOLinkedAnim = null;
// Це треба для анимашок, щоб не викликати одне і те ж саме, якщо спамиш кликами
public class Interactable : MonoBehaviour {
	public GameObject AdditionalOutlineGO;
	public float OutlineScale = 1;
	public float InteractDist;
	public System.Action OnMouseClick;

	[SerializeField] bool interactPosOnCenter;
	[SerializeField] float outlineSize;

	SpriteRenderer spriteRenderer;

	Vector3 interactPos;

	SpriteOutline outlineAdditional;
	SpriteOutline outline;
	float InteractDistSqr;

	protected virtual void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();

		InteractDistSqr = InteractDist * InteractDist;

		RecalcInteractPos();
	}

	void OnMouseEnter() {
		if (GameManager.Instance.SelectedOutlineGO != null)
			return;

		GameManager.Instance.SelectedOutlineGO = this;
		if (outline == null) {
			outline = CreateOutline(gameObject, true);
			if (AdditionalOutlineGO) {
				outlineAdditional = CreateOutline(AdditionalOutlineGO, false);
			}
		}
		outline.gameObject.SetActive(true);
		if (outlineAdditional)
			outlineAdditional.gameObject.SetActive(true);
	}

	void OnMouseDown() {
		if (GameManager.Instance.SelectedOutlineGO != this || GameManager.Instance.Player.Equipment.GOLinkedAnim == gameObject)
			return;

		GameManager.Instance.Player.Equipment.GOLinkedAnim = gameObject;
		GameManager.Instance.Player.InterruptAction();
		if (CanInteract()) {
			OnMouseClick?.Invoke();
		}
		else {
			GameManager.Instance.Player.PlayerKeyboardMover.MoveTo(interactPos);
			GameManager.Instance.Player.PlayerKeyboardMover.OnMouseMoveEnd += OnMouseClick;
		}
	}

	void OnMouseExit() {
		if (GameManager.Instance.SelectedOutlineGO != this)
			return;

		GameManager.Instance.SelectedOutlineGO = null;
		outline.gameObject.SetActive(false);
		if (outlineAdditional)
			outlineAdditional.gameObject.SetActive(false);
	}

	public void RecalcInteractPos() {
		interactPos = spriteRenderer.bounds.center;
		if (!interactPosOnCenter)
			interactPos += Vector3.down * spriteRenderer.bounds.size.y / 2;
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
