using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseStat : MonoBehaviour {
	public float MaxValue {
		get {
			return _maxValue;
		}
		set {
			_maxValue = value;
			OnValueChange?.Invoke();
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
			OnValueChange?.Invoke();
		}
	}

	public Action OnValueChange;

	[SerializeField] float _value;
	[SerializeField] float _maxValue;

	public void ChangeBy(float val) {
		Value += val;
	}

	public float GetFill() {
		return Value / MaxValue;
	}
}
