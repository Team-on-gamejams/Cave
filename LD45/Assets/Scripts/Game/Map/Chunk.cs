using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {
	public int x, y;
	public Chunk up, right, down, left;

	public void GenerateNearbyChunks() {
		GameObject chunkgo = new GameObject("Chunk");
		Chunk chunk = chunkgo.AddComponent<Chunk>();
	}
}
