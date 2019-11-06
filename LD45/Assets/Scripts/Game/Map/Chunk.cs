using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {
	public enum Neighbour : byte { Up = 0b1, Right = 0b10, Down = 0b100, Left = 0b1000 }

	public int x, y;
	public bool canMoveUp = true, canMoveRight = true, canMoveDown = true, canMoveLeft = true;
	public Chunk up, right, down, left;

	public List<BaseBuilding> buildings;
	public List<OnGroundItem> onGroundItems;
	public List<ResourcesSource> ResourcesSources;

	private void Awake() {
		WorldGenerator.instance.chunks.Add(this);
	}

	void OnTriggerEnter2D(Collider2D collision) {
		if(collision.tag == "Player") {
			GameManager.Instance.Player.CurrChunk = this;
			GenerateNearbyChunks();
		}
	}

	public void GenerateNearbyChunks() {
		if (canMoveUp && up == null && WorldGenerator.instance.GetChunk(x, y + 1) == null)
			CreateNeighbour(Neighbour.Up);
		if (canMoveRight && right == null && WorldGenerator.instance.GetChunk(x + 1, y) == null)
			CreateNeighbour(Neighbour.Right);
		if (canMoveDown && down == null && WorldGenerator.instance.GetChunk(x, y - 1) == null)
			CreateNeighbour(Neighbour.Down);
		if (canMoveLeft && left == null && WorldGenerator.instance.GetChunk(x - 1, y) == null)
			CreateNeighbour(Neighbour.Left);

		//if (WorldGenerator.instance.GetChunk(x + 1, y + 1) == null)
		//	CreateNeighbour(Neighbour.Right | Neighbour.Up);
		//if (WorldGenerator.instance.GetChunk(x - 1, y + 1) == null)
		//	CreateNeighbour(Neighbour.Left | Neighbour.Up);
		//if (WorldGenerator.instance.GetChunk(x + 1, y - 1) == null)
		//	CreateNeighbour(Neighbour.Right | Neighbour.Down);
		//if (WorldGenerator.instance.GetChunk(x - 1, y - 1) == null)
		//	CreateNeighbour(Neighbour.Left | Neighbour.Down);
	}

	public void CreateNeighbour(Neighbour neighbour) {
		GameObject chunkgo = Instantiate(WorldGenerator.instance.chunkTest);
		Chunk chunk = chunkgo.GetComponent<Chunk>();
		chunk.x = x;
		chunk.y = y;
		chunk.transform.position = transform.position;
		chunk.transform.parent = WorldGenerator.instance.transform;

		if ((neighbour & Neighbour.Up) == Neighbour.Up) {
			++chunk.y;

			chunk.down = this;
			up = chunk;

			chunk.transform.position += new Vector3(0, WorldGenerator.instance.chunkSize);
		}
		if ((neighbour & Neighbour.Right) == Neighbour.Right) {
			++chunk.x;

			chunk.left = this;
			right = chunk;

			chunk.transform.position += new Vector3(WorldGenerator.instance.chunkSize, 0);
		}
		if ((neighbour & Neighbour.Down) == Neighbour.Down) {
			--chunk.y;

			chunk.up = this;
			down = chunk;

			chunk.transform.position -= new Vector3(0, WorldGenerator.instance.chunkSize);
		}
		if ((neighbour & Neighbour.Left) == Neighbour.Left) {
			--chunk.x;

			chunk.right = this;
			left = chunk;

			chunk.transform.position -= new Vector3(WorldGenerator.instance.chunkSize, 0);
		}
	}
}
