using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class BaseStat : MonoBehaviour {
	public float MaxValue {
		get {
			return _maxValue;
		}
		set {
			_maxValue = value;
		}
	}
	public float Value {
		get {
			return _value;
		}
		set {
			_value = value;
			if (_value > MaxValue)
				_value = MaxValue;
			if (_value < 0)
				_value = 0;
		}
	}

	public Action OnValueChange;

	[SerializeField] float _maxValue;
	[SerializeField] float _value;

	public BaseStat() {
		_value = _maxValue = 0;
	}

	public void ChangeBy(float val) {
		Value += val;
		OnValueChange?.Invoke();
	}
}
