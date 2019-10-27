using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[Serializable]
public class PlayerInputFlyweight {
	static public bool CanMouseMove = true;

	public Player player;

	public InventoryUI inventoryUI;
	public CraftUI craftUI;
	public StatUI statUI;
	public EquipmentUI equipmentUI;
	public InGameMenu inGameMenu;
	public Hotbar hotbar;
}
