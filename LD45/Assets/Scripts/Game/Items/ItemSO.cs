using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ItemSO", menuName = "ItemSO", order = 51)]
public class ItemSO : ScriptableObject {
	public enum ItemSlot : byte { None, Hands, Head, Armor }

	public enum ItemMetaType : byte {
		None,
		Hands,		//1 <= ItemType < 1000		Can be hold in hand. Used for battle & harvester
		//Head,
		//Armor,
		Resource,   //1000 <= ItemType < 2000	Used only for crafting.
		Building,   //2000 <= ItemType < 3000	Can be build on map
	}
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
		Torch,		//TODO: need smth special, cuz can be hold both in hand & building
	}

	public ItemSlot Slot;
	public ItemMetaType MataType {
		get {
			if (1 <= (int)Type && (int)Type < 1000)
				return ItemMetaType.Hands;
			if (1000 <= (int)Type && (int)Type < 2000)
				return ItemMetaType.Resource;
			if (2000 <= (int)Type && (int)Type < 3000)
				return ItemMetaType.Building;

			return ItemMetaType.None;
		}
	}
	public ItemType Type;

	public string Name;
	public string Description;
	public ushort Count;
	public ushort MaxCount;
	public Sprite Sprite;
}
