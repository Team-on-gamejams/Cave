using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class BaseStatUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	[SerializeField] Image filledImg;
	[SerializeField] BaseStat stat;
	[SerializeField] TextMeshProUGUI statText;

	void Awake() {
		stat.OnValueChange += UpdateFill;
	}

	void Start() {
		statText.gameObject.SetActive(false);
		statText.text = $"{stat.Value} / {stat.MaxValue}";
		UpdateFill();
	}

	void OnDestroy() {
		stat.OnValueChange -= UpdateFill;
	}

	public void OnPointerEnter(PointerEventData eventData) {
		statText.gameObject.SetActive(true);
	}

	public void OnPointerExit(PointerEventData eventData) {
		statText.gameObject.SetActive(false);
	}

	public void UpdateFill() {
		filledImg.fillAmount = stat.Value / stat.MaxValue;
		if (statText.gameObject.activeSelf)
			statText.text = $"{stat.Value} / {stat.MaxValue}";
	}
}