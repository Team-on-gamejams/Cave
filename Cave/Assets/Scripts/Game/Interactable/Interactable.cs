using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


// Всі класи, що наслідуються мають всередині виконувати   
// GameManager.Instance.Player.Equipment.GOLinkedAnim = null;
// Це треба для анимашок, щоб не викликати одне і те ж саме, якщо спамиш кликами
public class Interactable : MonoBehaviour {
	public float InteractDist;
	public Action OnMouseClick;

	[SerializeField] protected string tip;
	protected bool isInteractLMB;

	[SerializeField] bool interactPosOnCenter;
	[SerializeField] float outlineScale = 1;
	[SerializeField] float outlineSize = 1;

	SpriteRenderer spriteRenderer;

	Vector3 interactPos;
	float InteractDistSqr;

	protected virtual void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();

		InteractDistSqr = InteractDist * InteractDist;
    }

	virtual protected void Start() {
		RecalcInteractPos();
	}

	// OnMouseEnter та OnMouseExit викликаються завжди послідовно. Якщо курсор буде над 2 обєктами, то буде: OnMouseEnter -> OnMouseExit -> OnMouseEnter
	// Якщо буде дуже важно мати правильну обводку, то треба буде в OnMouseOver або в OnMouseEnter робити рейкасти, і чекати найвищий спрайт
	void OnMouseEnter() {
		if (GameManager.Instance.IsPaused/* || EventSystem.current.IsPointerOverGameObject()*/)
			return;
		ShowOutline();
	}

    private void OnMouseOver() {
		if (GameManager.Instance.IsPaused)
			return;

		if (Input.GetMouseButtonDown(0)) {
			isInteractLMB = true;
			ProcessMouseDown();
		}
		else if (Input.GetMouseButtonDown(1)){
			isInteractLMB = false;
			ProcessMouseDown();
		}

		EventData eventData = new EventData("OnPopUpShow");
        eventData["tipText"] = tip;
        EventManager.CallOnMouseOverTip(eventData);
    }

	void OnMouseExit() {
		if (GameManager.Instance.IsPaused)
			return;

		HideOutline();
	}

	public void RecalcInteractPos() {
		interactPos = spriteRenderer.bounds.center;
		if (!interactPosOnCenter)
			interactPos += Vector3.down * spriteRenderer.bounds.size.y / 2;
	}

	public void SimulateMouseClick() {
		isInteractLMB = true;
		ProcessMouseDown();
	}

	public virtual bool CanInteract() => true;

	public bool IsInRange() {
		return GameManager.Instance.Player.CanInteract(transform.position, InteractDistSqr);
	}

	void ProcessMouseDown() {
		if (GameManager.Instance.IsPaused ||GameManager.Instance.Player.Equipment.GOLinkedAnim == gameObject || !CanInteract())
			return;

		GameManager.Instance.Player.InterruptAction();
		GameManager.Instance.Player.Equipment.GOLinkedAnim = gameObject;
		if (IsInRange()) {
			OnMouseClick?.Invoke();
		}
		else {
			GameManager.Instance.Player.MoveTo(interactPos);
			GameManager.Instance.Player.OnMoveEndEvent += OnMouseClick;
		}
	}

	void ShowOutline() {

	}

	void HideOutline() {

	}

	public static void OnPause() {

	}
}
