using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseStatUI : MonoBehaviour {
	[SerializeField] Image filledImg;
	[SerializeField] BaseStat stat;

	void Awake() {
		//stat.OnValueChange += UpdateFill;
	}

	void Start() {
		//UpdateFill();
	}

	void OnDestroy() {
		//stat.OnValueChange -= UpdateFill;
	}

	public void UpdateFill() {
		//filledImg.fillAmount = stat.Value / stat.MaxValue;
	}
}
