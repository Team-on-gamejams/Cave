using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemSlotHotbar: ItemSlot {
	[SerializeField] Image SelectedFrame;

	protected override void Awake() {
		base.Awake();
		SelectedFrame.gameObject.SetActive(false);
	}

	public void SetSelectedFrame() {
		SelectedFrame.gameObject.SetActive(true);
	}

	public void RemoveSelectedFrame() {
		SelectedFrame.gameObject.SetActive(false);
	}
}
