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
			if (GameManager.Instance.IsPaused && !input.IgnorePause)
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
	public void ShowHideEquipment() => flyweight.equipmentUI.ChangeShowHide();
	public void ShowHideInGameMenu() => flyweight.inGameMenu.ChangeShowHide();

	public void SetHotbarSelection(int slot) => flyweight.hotbar.SetSelection((byte)slot);
	public void MoveHotbarSelectionUp() => flyweight.hotbar.MoveSelectionUp();
	public void MoveHotbarSelectionDown() => flyweight.hotbar.MoveSelectionDown();
}
