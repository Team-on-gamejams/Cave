using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerInputStateBase", menuName = "Player Input/Base", order = 52)]
public class PlayerInputStateBase : ScriptableObject {
	public PlayerInputFlyweight flyweight;

	[SerializeField] InputEvent[] Inputs;

	public virtual void Update() {
		ProcessInputs();
	}

	protected void ProcessInputs() {
		for (byte i = 0; i < Inputs.Length; ++i) {
			InputEvent input = Inputs[i];
			if (
				(GameManager.Instance.IsPaused && !input.IgnorePause) ||
				(input.NeedShift  && !(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))) ||
				(!input.NeedShift &&  (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
			)
				continue;

			switch (input.Type) {
				case InputEvent.InputEventType.Key:
					if (Input.GetKeyDown(input.Key)) {
						if (input.InterruptAnim)
							flyweight.player.InterruptAction();
						input.OnButtonPressed?.Invoke();
					}
					break;
				case InputEvent.InputEventType.MouseWheel:
					var wheelDelta = Input.GetAxis("Mouse ScrollWheel");
					if (wheelDelta > 0f && input.IsWheelUp) {
						if (input.InterruptAnim)
							flyweight.player.InterruptAction();
						input.OnButtonPressed?.Invoke();
					}
					else if (wheelDelta < 0f && !input.IsWheelUp) {
						if (input.InterruptAnim)
							flyweight.player.InterruptAction();
						input.OnButtonPressed?.Invoke();
					}
					break;
				case InputEvent.InputEventType.None:
				default:
					break;
			}
		}
	}

	public void ShowHideInventoryCraft() {
		if(flyweight.inventoryUI.CanChangeShowHide() && flyweight.craftUI.CanChangeShowHide()) {
			flyweight.inventoryUI.ChangeShowHide();
			flyweight.craftUI.ChangeShowHide();
		}
	} 
	public void ShowHideStat() => flyweight.statUI.ChangeShowHide();
	public void ShowHideBigMap() => flyweight.bigMapUI.ChangeShowHide();
	public void ShowHideEquipment() => flyweight.equipmentUI.ChangeShowHide();

	public void SetHotbarSelectionLeft(int slot) => flyweight.hotbar.SetSelection((byte)slot, true);
	public void MoveHotbarSelectionUpLeft() => flyweight.hotbar.MoveSelectionUp(true);
	public void MoveHotbarSelectionDownLeft() => flyweight.hotbar.MoveSelectionDown(true);

	public void SetHotbarSelectionRight(int slot) => flyweight.hotbar.SetSelection((byte)slot, false);
	public void MoveHotbarSelectionUpRight() => flyweight.hotbar.MoveSelectionUp(false);
	public void MoveHotbarSelectionDownRight() => flyweight.hotbar.MoveSelectionDown(false);
}
