using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ItemSO", menuName = "ItemSO", order = 51)]
public class ItemSO : ScriptableObject {
	public string Name;
	public string Description;
	public byte Count;
	public byte MaxCount;
	public Texture2D Texture;
}
