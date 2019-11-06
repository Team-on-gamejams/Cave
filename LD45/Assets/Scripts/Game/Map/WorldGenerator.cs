using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour {
	public static WorldGenerator instance {
		get {
			if (_instance == null)
				_instance = FindObjectOfType<WorldGenerator>();
			return _instance;
		}
	}
	public static WorldGenerator _instance;

	public float chunkSize;
	public List<Chunk> chunks;
	[SerializeField] Chunk startingChunk;

	public GameObject[] marchingSquares;

	public GameObject chunkTest;


	public Chunk GetChunk(int x, int y) {
		foreach (var chunk in chunks)
			if (chunk.x == x && chunk.y == y)
				return chunk;
		return null;
	}

	public Chunk GetChunkFromWorldPos(Vector3 pos) {
		foreach (var chunk in chunks) {
			if (
				chunk.x * chunkSize - chunkSize / 2 <= pos.x && pos.x <= chunk.x * chunkSize + chunkSize / 2 &&
				chunk.y * chunkSize - chunkSize / 2 <= pos.y && pos.y <= chunk.y * chunkSize + chunkSize / 2
			)
				return chunk;
		}
		return null;
	}
}
