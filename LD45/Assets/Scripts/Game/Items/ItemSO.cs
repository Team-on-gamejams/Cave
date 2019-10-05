using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ItemSO", menuName = "ItemSO", order = 51)]
public class ItemSO : ScriptableObject {
	public enum ItemSlot : byte { None, Hands }
	public enum ItemType : uint {
		None = 0,
		Axe = 1,
		Pickaxe,
		Spear,
		Wood = 1000,
		Stone,
		Chest = 2000, 
		Tent, 
		Bonfire, 
		Torch, 
	}

	public ItemSlot Slot;
	public ItemType Type;

	public string Name;
	public string Description;
	public byte Count;
	public byte MaxCount;
	public Texture2D Texture;
}
