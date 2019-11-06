using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: make it abstract after cream done hotbar & inventory
public class BaseBuilding : Interactable {
	public ItemSO Item;

	protected override void Start() {
		base.Start();
		Chunk chunk = WorldGenerator.instance.GetChunkFromWorldPos(transform.position);
		transform.parent = chunk.transform;
		chunk.buildings.Add(this);
	}

	private void OnDestroy() {
		WorldGenerator.instance.GetChunkFromWorldPos(transform.position).buildings.Remove(this);
	}

	void Update() {

	}
}
