using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BigMapUI : BaseUI, IDragHandler {
	[SerializeField] Camera MapCamera;
	[SerializeField] float minZoom = 1;
	[SerializeField] float maxZoom = 20;

	const float dragMultDiv = 500;
	float dragMult;

	void Start() {
		dragMult = MapCamera.orthographicSize / dragMultDiv;

		HideAfterStart();
	}

	void Update() {
		var wheelDelta = Input.GetAxis("Mouse ScrollWheel");
		if(wheelDelta > 0 && maxZoom > MapCamera.orthographicSize) {
			MapCamera.orthographicSize += 1;
			dragMult = MapCamera.orthographicSize / dragMultDiv;
		}
		else if (wheelDelta < 0 && minZoom < MapCamera.orthographicSize) {
			MapCamera.orthographicSize -= 1;
			dragMult = MapCamera.orthographicSize / dragMultDiv;
		}
	}

	public void OnDrag(PointerEventData eventData) {
		MapCamera.transform.position += (Vector3)(-eventData.delta * dragMult);
	}

	protected override void BeforeShow() {
		GameManager.Instance.EventManager.CallOnBigMapShow();
	}

	protected override void BeforeHide() {
		GameManager.Instance.EventManager.CallOnBigMapHide();
	}

	public override bool CanChangeShowHide() {
		return !GameManager.Instance.IsPaused;
	}
}
