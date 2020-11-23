using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {
	public enum Neighbour : byte { Up = 0b1, Right = 0b10, Down = 0b100, Left = 0b1000 }

	public int x, y;
	public bool canMoveUp = true, canMoveRight = true, canMoveDown = true, canMoveLeft = true;
	public Chunk up, right, down, left;
	public Chunk upRight => up.right;
	public Chunk upLeft => up.left;
	public Chunk downRight => down.right;
	public Chunk downLeft => down.left;

	public List<BaseBuilding> buildings;
	public List<OnGroundItem> onGroundItems;
	public List<ResourcesSource> ResourcesSources;

	public Tile[,] map;

	private void Awake() {
		WorldGenerator.instance.chunks.Add(this);
	}

	void Start() {
		gameObject.name = $"Chunk [{x}, {y}]";	
	}

	void OnTriggerEnter2D(Collider2D collision) {
		if(collision.tag == "Player") {
			GameManager.Instance.Player.CurrChunk.Add(this);
			GenerateNearbyChunks();

			up?.gameObject?.SetActive(true);
			right?.gameObject?.SetActive(true);
			down?.gameObject?.SetActive(true);
			left?.gameObject?.SetActive(true);

			upLeft?.gameObject?.SetActive(true);
			upRight?.gameObject?.SetActive(true);
			downLeft?.gameObject?.SetActive(true);
			downRight?.gameObject?.SetActive(true);
		}
	}

	void OnTriggerExit2D(Collider2D collision) {
		if (collision.tag == "Player") {
			GameManager.Instance.Player.CurrChunk.Remove(this);
			//bool isMoveUp = GameManager.Instance.Player.CurrChunk.y > y;
			//bool isMoveRight = GameManager.Instance.Player.CurrChunk.x > x;

			//if (up != GameManager.Instance.Player.CurrChunk && GameManager.Instance.Player.CurrChunk.x == x)
			//	up?.gameObject?.SetActive(false);
			//if(right != GameManager.Instance.Player.CurrChunk && GameManager.Instance.Player.CurrChunk.y == y)
			//	right?.gameObject?.SetActive(false);
			//if(down != GameManager.Instance.Player.CurrChunk && GameManager.Instance.Player.CurrChunk.x == x)
			//	down?.gameObject?.SetActive(false);
			//if(left != GameManager.Instance.Player.CurrChunk && GameManager.Instance.Player.CurrChunk.y == y)
			//	left?.gameObject?.SetActive(false);


			//bool upLeftActive = false,
			//	upRightActive = false,
			//	downLeftActive = false,
			//	downRightActive = false;

			//if (isMoveUp) {
			//	upLeftActive = true;
			//	upRightActive = true;
			//}
			//else if(GameManager.Instance.Player.CurrChunk.y != y) {
			//	downLeftActive = true;
			//	downRightActive = true;
			//}

			//if (isMoveRight) {
			//	upRightActive = true;
			//	downRightActive = true;
			//}
			//else if (GameManager.Instance.Player.CurrChunk.x != x) {
			//	upLeftActive = true;
			//	downLeftActive = true;
			//}

			//upLeft?.gameObject?.SetActive(upLeftActive);
			//upRight?.gameObject?.SetActive(upRightActive);
			//downLeft?.gameObject?.SetActive(downLeftActive);
			//downRight?.gameObject?.SetActive(downRightActive);
		}
	}

	public void GenerateChunk() {
		ChunkGenerator generator = GetComponent<ChunkGenerator>();
		if (generator != null)
			map = generator.GenerateMap(this);
	}

	public void GenerateNearbyChunks() {
		if (canMoveRight && right == null && WorldGenerator.instance.GetChunk(x + 1, y) == null)
			CreateNeighbour(Neighbour.Right);

		if (canMoveLeft && left == null && WorldGenerator.instance.GetChunk(x - 1, y) == null)
			CreateNeighbour(Neighbour.Left);

		if (canMoveUp && up == null && WorldGenerator.instance.GetChunk(x, y + 1) == null)
			CreateNeighbour(Neighbour.Up);

		if (canMoveDown && down == null && WorldGenerator.instance.GetChunk(x, y - 1) == null)
			CreateNeighbour(Neighbour.Down);

		if (WorldGenerator.instance.GetChunk(x + 1, y + 1) == null) {
			if (up)
				up.CreateNeighbour(Neighbour.Right);
			else if (right)
				right.CreateNeighbour(Neighbour.Up);
		}

		if (WorldGenerator.instance.GetChunk(x + 1, y - 1) == null) {
			if (right)
				right.CreateNeighbour(Neighbour.Down);
			else if (down)
				down.CreateNeighbour(Neighbour.Right);
		}
		

		if (WorldGenerator.instance.GetChunk(x - 1, y - 1) == null) {
			if (left)
				left.CreateNeighbour(Neighbour.Down);
			else if (down)
				down.CreateNeighbour(Neighbour.Left);
		}

		if (WorldGenerator.instance.GetChunk(x - 1, y + 1) == null) {
			if (left)
				left.CreateNeighbour(Neighbour.Up);
			else if (up)
				up.CreateNeighbour(Neighbour.Left);
		}
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

			SetNeighbour(this, chunk, Neighbour.Up);
			SetNeighbour(chunk, WorldGenerator.instance.GetChunk(chunk.x, chunk.y + 1), Neighbour.Up);
			SetNeighbour(chunk, WorldGenerator.instance.GetChunk(chunk.x + 1, chunk.y), Neighbour.Right);
			SetNeighbour(chunk, WorldGenerator.instance.GetChunk(chunk.x - 1, chunk.y), Neighbour.Left);

			chunk.transform.position += new Vector3(0, WorldGenerator.instance.chunkSize);
		}
		if ((neighbour & Neighbour.Right) == Neighbour.Right) {
			++chunk.x;

			SetNeighbour(this, chunk, Neighbour.Right);
			SetNeighbour(chunk, WorldGenerator.instance.GetChunk(chunk.x, chunk.y + 1), Neighbour.Up);
			SetNeighbour(chunk, WorldGenerator.instance.GetChunk(chunk.x, chunk.y - 1), Neighbour.Down);
			SetNeighbour(chunk, WorldGenerator.instance.GetChunk(chunk.x + 1, chunk.y), Neighbour.Right);
			chunk.transform.position += new Vector3(WorldGenerator.instance.chunkSize, 0);
		}
		if ((neighbour & Neighbour.Down) == Neighbour.Down) {
			--chunk.y;

			SetNeighbour(this, chunk, Neighbour.Down);
			SetNeighbour(chunk, WorldGenerator.instance.GetChunk(chunk.x, chunk.y - 1), Neighbour.Down);
			SetNeighbour(chunk, WorldGenerator.instance.GetChunk(chunk.x + 1, chunk.y), Neighbour.Right);
			SetNeighbour(chunk, WorldGenerator.instance.GetChunk(chunk.x - 1, chunk.y), Neighbour.Left);
			chunk.transform.position -= new Vector3(0, WorldGenerator.instance.chunkSize);
		}
		if ((neighbour & Neighbour.Left) == Neighbour.Left) {
			--chunk.x;

			SetNeighbour(this, chunk, Neighbour.Left);
			SetNeighbour(chunk, WorldGenerator.instance.GetChunk(chunk.x, chunk.y + 1), Neighbour.Up);
			SetNeighbour(chunk, WorldGenerator.instance.GetChunk(chunk.x, chunk.y - 1), Neighbour.Down);
			SetNeighbour(chunk, WorldGenerator.instance.GetChunk(chunk.x - 1, chunk.y), Neighbour.Left);
			chunk.transform.position -= new Vector3(WorldGenerator.instance.chunkSize, 0);
		}

		chunk.GenerateChunk();
	}

	public void SetNeighbour(Chunk a, Chunk b, Neighbour neighbour) {
		if (a == null || b == null)
			return;

		switch (neighbour) {
			case Neighbour.Up:
				a.up = b;
				b.down = a;
				break;
			case Neighbour.Right:
				a.right = b;
				b.left = a;
				break;
			case Neighbour.Down:
				a.down = b;
				b.up = a;
				break;
			case Neighbour.Left:
				a.left = b;
				b.right = a;
				break;
		}
	}
}
