using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BigMapUI : BaseUI, IDragHandler {
	[SerializeField] Camera MapCamera;
	[SerializeField] float minZoom = 1;
	[SerializeField] float maxZoom = 20;

	[SerializeField] float WASDMoveSpeed = 20;
	[SerializeField] float dragMultDiv = 500;
	float dragMult;

	void Start() {
		dragMult = MapCamera.orthographicSize / dragMultDiv;
		HideAfterStart();
	}

	void Update() {
		if (!GameManager.Instance.IsPaused) {
			ProcessZoom();
			ProcessWASDMove();
		}
	}

	public void OnDrag(PointerEventData eventData) {
		if (!GameManager.Instance.IsPaused) 
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

	void ProcessZoom() {
		var wheelDelta = Input.GetAxis("Mouse ScrollWheel");
		if (wheelDelta > 0 && maxZoom > MapCamera.orthographicSize) {
			MapCamera.orthographicSize += 1;
			dragMult = MapCamera.orthographicSize / dragMultDiv;
		}
		else if (wheelDelta < 0 && minZoom < MapCamera.orthographicSize) {
			MapCamera.orthographicSize -= 1;
			dragMult = MapCamera.orthographicSize / dragMultDiv;
		}
	}

	//TODO: ability to map WASD
	void ProcessWASDMove() {
		float v = Input.GetAxisRaw("Vertical"), h = Input.GetAxisRaw("Horizontal");
		if (v != 0 || h != 0) {
			MapCamera.transform.position += new Vector3(h, v).normalized * WASDMoveSpeed * Time.deltaTime;
		}
	}
}
