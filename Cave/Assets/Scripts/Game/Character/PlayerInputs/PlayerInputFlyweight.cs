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
	public BigMapUI bigMapUI;
	public EquipmentUI equipmentUI;
	public Hotbar hotbar;
}
