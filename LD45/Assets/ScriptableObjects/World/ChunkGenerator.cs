using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class ChunkGenerator : MonoBehaviour {
	public int width = 128;
	public int height = 72;

	public int wallThresholdSize = 25;
	public int roomThresholdSize = 25;
	public int coridorThreshold = 25;
	public int coridorRadius = 2;
	public int borderSize = 5;

	public float squareSize = 1;
	public int smoothBase = 4;
	public float smoothLevel = 5;
	public float smoothLevelAfterPassage = 1;

	[Range(0, 100)]
	public int randomFillPercent;

	Tile[,] map;
	Chunk chunk;

	void Awake() {
		coridorThreshold = coridorThreshold * coridorThreshold;
	}

	public Tile[,] GenerateMap(Chunk _chunk) {
		chunk = _chunk;
		map = new Tile[width, height];
		for (int x = 0; x < width; x++)
			for (int y = 0; y < height; y++)
				map[x, y] = new Tile();

		RandomFillMap();

		for (int i = 0; i < smoothLevel; i++)
			SmoothMap();
		RemoveSmallWalls();
		RemoveSmallRooms();

		ConnectRooms();

		for (int i = 0; i < smoothLevelAfterPassage; i++)
			SmoothMap();
		RemoveSmallWalls();


		Tile[,] borderedMap = GetBorderedMap();

		MeshGenerator meshGen = GetComponent<MeshGenerator>();
		meshGen.GenerateMesh(borderedMap, squareSize);

		return borderedMap;
	}

	void RandomFillMap() {
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if (x == 0 || x == width - 1 || y == 0 || y == height - 1) {
					map[x, y].isSolid = true;
				}
				else {
					map[x, y].isSolid = (Random.Range(0, 100) < randomFillPercent) ? true : false;
				}
			}
		}
	}

	void SmoothMap() {
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				int neighbourWallTiles = GetSurroundingWallCount(x, y);

				if( (map[x, y].isSolid && neighbourWallTiles >= smoothBase) ||
					(!map[x, y].isSolid && neighbourWallTiles >= smoothBase + 1)
					) {
					map[x, y].isSolid = true;
				}
				else {
					map[x, y].isSolid = false;
				}

				//if (neighbourWallTiles > smoothBase)
				//	map[x, y].isSolid = true;
				//else if (neighbourWallTiles < smoothBase)
				//	map[x, y].isSolid = false;

			}
		}
	}

	int GetSurroundingWallCount(int gridX, int gridY) {
		int wallCount = 0;
		for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++) {
			for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++) {
				if (
					0  <= neighbourX && neighbourX < width && 0 <= neighbourY && neighbourY < height
					) {
					if (neighbourX != gridX || neighbourY != gridY) {
						wallCount += map[neighbourX, neighbourY].isSolid? 1 : 0;
					}
				}
				else {
					//wallCount++;
				}
			}
		}

		return wallCount;
	}

	void RemoveSmallWalls() {
		List<List<Coord>> wallRegions = GetRegions(true);
		foreach (List<Coord> wallRegion in wallRegions) {
			if (wallRegion.Count < wallThresholdSize) {
				foreach (Coord tile in wallRegion) {
					map[tile.x, tile.y].isSolid = false;
				}
			}
		}
	}

	void RemoveSmallRooms() {
		List<List<Coord>> roomRegions = GetRegions(false);
		foreach (List<Coord> roomRegion in roomRegions) {
			if (roomRegion.Count < roomThresholdSize) {
				foreach (Coord tile in roomRegion) {
					map[tile.x, tile.y].isSolid = true;
				}
			}
		}
	}

	List<List<Coord>> GetRegions(bool isSolid) {
		List<List<Coord>> regions = new List<List<Coord>>();
		int[,] mapFlags = new int[width, height];

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if (mapFlags[x, y] == 0 && map[x, y].isSolid == isSolid) {
					List<Coord> newRegion = new List<Coord>();

					Queue<Coord> queue = new Queue<Coord>();
					queue.Enqueue(new Coord(x, y));

					while (queue.Count > 0) {
						Coord tile = queue.Dequeue();
						mapFlags[tile.x, tile.y] = 1;
						newRegion.Add(tile);

						FillTile(tile.x - 1, tile.y, queue);
						FillTile(tile.x + 1, tile.y, queue);
						FillTile(tile.x, tile.y - 1, queue);
						FillTile(tile.x, tile.y + 1, queue);
					}

					regions.Add(newRegion);
				}
			}
		}

		return regions;

		void FillTile(int x, int y, Queue<Coord> queue) {
			if (IsInMapRange(x, y) && mapFlags[x, y] == 0 && map[x, y].isSolid == isSolid) {
				mapFlags[x, y] = 1;
				queue.Enqueue(new Coord(x, y));
			}
		}
	}

	Tile[,] GetBorderedMap() {
		Tile[,] borderedMap = new Tile[width + borderSize * 2, height + borderSize * 2];

		for (int x = 0; x < borderedMap.GetLength(0); x++) {
			for (int y = 0; y < borderedMap.GetLength(1); y++) {
				if (
					borderSize <= x && x < width + borderSize &&
					borderSize <= y && y < height + borderSize
				) {
					borderedMap[x, y] = map[x - borderSize, y - borderSize];
				}
				else {

					borderedMap[x, y] = new Tile() {
						isSolid = false,
					};
				}
			}
		}

		return borderedMap;
	}

	void ConnectRooms() {
		List<List<Coord>> roomRegions = GetRegions(false);

		List<Room> rooms = new List<Room>();

		foreach (List<Coord> roomRegion in roomRegions)
			rooms.Add(new Room(roomRegion, map));

		if (rooms.Count <= 1)
			return;

		RoomConnection[,] prices = new RoomConnection[rooms.Count, rooms.Count];

		int bestDistance = int.MaxValue;
		Coord bestA = new Coord(), bestB = new Coord();

		for (int i = 0; i < rooms.Count - 1; ++i) {
			Room roomA = rooms[i];
			prices[i, i] = new RoomConnection() {
				dist = int.MaxValue,
			};

			for (int j = i + 1; j < rooms.Count; ++j) {
				Room roomB = rooms[j];
				bestDistance = int.MaxValue;

				for (int tileIndexA = 0; tileIndexA < roomA.edgeTiles.Count; tileIndexA++) {
					for (int tileIndexB = 0; tileIndexB < roomB.edgeTiles.Count; tileIndexB++) {
						Coord tileA = roomA.edgeTiles[tileIndexA];
						Coord tileB = roomB.edgeTiles[tileIndexB];
						int distanceBetweenRooms = (int)(Mathf.Pow(tileA.x - tileB.x, 2) + Mathf.Pow(tileA.y - tileB.y, 2));

						if (distanceBetweenRooms < bestDistance) {
							bestDistance = distanceBetweenRooms;
							bestA = tileA;
							bestB = tileB;
						}
					}
				}

				prices[i, j].dist = bestDistance;
				prices[i, j].tileA = bestA;
				prices[i, j].tileB = bestB;

				prices[j, i].dist = bestDistance;
				prices[j, i].tileA = bestA;
				prices[j, i].tileB = bestB;
			}
		}

		prices[rooms.Count - 1, rooms.Count - 1] = new RoomConnection() {
			dist = int.MaxValue,
		};

		for (int i = 0; i < rooms.Count - 1; ++i) {
			int minId = i + 1;
			for (int j = i + 2; j < rooms.Count; ++j)
				if (prices[i, minId].dist > prices[i, j].dist)
					minId = j;

			for (int j = 0; j < rooms.Count; ++j)
				if (prices[i, minId].dist >= prices[i, j].dist - coridorThreshold)
					CreatePassage(rooms[i], rooms[j], prices[i, j].tileA, prices[i, j].tileB);
		}
	}

	void CreatePassage(Room roomA, Room roomB, Coord tileA, Coord tileB) {
		Room.ConnectRooms(roomA, roomB);

		List<Coord> line = GetLine(tileA, tileB);
		foreach (Coord c in line)
			DrawCircle(c, coridorRadius);

		Vector3 CoordToWorldPoint(Coord tile) {
			return new Vector3(-width / 2 + .5f + tile.x, 2, -height / 2 + .5f + tile.y);
		}
	}

	void DrawCircle(Coord c, int r) {
		for (int x = -r; x <= r; x++) {
			for (int y = -r; y <= r; y++) {
				if (x * x + y * y <= r * r) {
					int drawX = c.x + x;
					int drawY = c.y + y;
					if (IsInMapRange(drawX, drawY)) {
						map[drawX, drawY].isSolid = false;
					}
				}
			}
		}
	}

	List<Coord> GetLine(Coord from, Coord to) {
		List<Coord> line = new List<Coord>();

		int x = from.x;
		int y = from.y;

		int dx = to.x - from.x;
		int dy = to.y - from.y;

		bool inverted = false;
		int step = Math.Sign(dx);
		int gradientStep = Math.Sign(dy);

		int longest = Mathf.Abs(dx);
		int shortest = Mathf.Abs(dy);

		if (longest < shortest) {
			inverted = true;
			longest = Mathf.Abs(dy);
			shortest = Mathf.Abs(dx);

			step = Math.Sign(dy);
			gradientStep = Math.Sign(dx);
		}

		int gradientAccumulation = longest / 2;
		for (int i = 0; i < longest; i++) {
			line.Add(new Coord(x, y));

			if (inverted)
				y += step;
			else
				x += step;

			gradientAccumulation += shortest;
			if (gradientAccumulation >= longest) {
				if (inverted)
					x += gradientStep;
				else
					y += gradientStep;
				gradientAccumulation -= longest;
			}
		}

		return line;
	}

	bool IsInMapRange(int x, int y) {
		return x >= 0 && x < width && y >= 0 && y < height;
	}

	struct Coord {
		public int x;
		public int y;

		public Coord(int _x, int _y) {
			x = _x;
			y = _y;
		}
	}

	class Room {
		public List<Coord> tiles;
		public List<Coord> edgeTiles;
		public List<Room> connectedRooms;
		public int roomSize;

		public Room() { }

		public Room(List<Coord> roomTiles, Tile[,] map) {
			tiles = roomTiles;
			roomSize = tiles.Count;
			connectedRooms = new List<Room>();

			edgeTiles = new List<Coord>();
			foreach (Coord tile in tiles)
				for (int x = tile.x - 1; x <= tile.x + 1; x++)
					for (int y = tile.y - 1; y <= tile.y + 1; y++)
						if (
							0 <= x && x < map.GetLength(0) && 0 <= y && y < map.GetLength(1) && 
							(x == tile.x || y == tile.y) && map[x, y].isSolid
						)
							edgeTiles.Add(tile);
		}

		public static void ConnectRooms(Room roomA, Room roomB) {
			roomA.connectedRooms.Add(roomB);
			roomB.connectedRooms.Add(roomA);
		}

		public bool IsConnected(Room otherRoom) {
			return connectedRooms.Contains(otherRoom);
		}
	}

	struct RoomConnection {
		public Coord tileA;
		public Coord tileB;
		public int dist;
	}
}