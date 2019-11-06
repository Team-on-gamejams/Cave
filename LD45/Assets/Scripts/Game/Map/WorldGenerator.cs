using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour {
	public static WorldGenerator instance;

	[SerializeField] Chunk startingChunk;
	public float chunkSize;
	public GameObject chunkTest;
	public List<Chunk> chunks;

	void Awake() {
		instance = this;
	}

	void Start() {
		LeanTween.delayedCall(1.0f, () => startingChunk.GenerateNearbyChunks());
	}

	void OnDestroy() {
		instance = null;
	}

	public Chunk GetChunk(int x, int y) {
		foreach (var chunk in chunks)
			if (chunk.x == x && chunk.y == y)
				return chunk;
		return null;
	}
}
