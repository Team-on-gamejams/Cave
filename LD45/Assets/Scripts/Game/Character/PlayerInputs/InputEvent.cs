using System;
using UnityEngine;
using UnityEngine.Events;

//TODO: probably need custom inspector
// Кнопки для Hotbar НЕ використовують InterruptAnim, бо всередині Hotbar вона преривається при EquipItem
[Serializable]
public class InputEvent {
	public enum InputEventType : byte { None, Key, MouseWheel };

	public InputEventType Type;
	public KeyCode Key;
	public bool IsWheelUp; //true - up, false - down

	public bool NeedShift;

	public bool IgnorePause;
	public bool InterruptAnim;

	public UnityEvent OnButtonPressed;
}
