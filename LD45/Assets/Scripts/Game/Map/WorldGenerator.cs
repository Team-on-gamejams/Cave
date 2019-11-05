using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour {
	public static WorldGenerator instance;

	[SerializeField] Chunk startingChunk;

	void Awake() {
		instance = this;
	}

	void Start() {
		startingChunk.GenerateNearbyChunks();
	}

	void OnDestroy() {
		instance = null;
	}
}
