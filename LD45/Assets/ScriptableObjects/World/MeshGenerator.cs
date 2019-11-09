using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshGenerator : MonoBehaviour {
	public MeshFilter roof;
	public MeshFilter roofMinimap;
	public MeshFilter walls;
	public MeshFilter floor;

	public SquareGrid squareGrid;
	List<Vector3> vertices;
	List<int> triangles;

	Dictionary<int, List<Triangle>> triangleDictionary;
	List<List<int>> outlines;
	HashSet<int> checkedOutlines;

	void Awake() {
		vertices = new List<Vector3>();
		triangles = new List<int>();

		triangleDictionary = new Dictionary<int, List<Triangle>>();
		outlines = new List<List<int>>();
		checkedOutlines = new HashSet<int>();
	}

	public void GenerateMesh(Tile[,] map, float squareSize) {
		squareGrid = new SquareGrid(map, squareSize);

		vertices.Clear();
		CreateRoofMesh();
		Generate2DColliders();
	}

	#region floor
	void CreateFloorMesh() {
		triangles.Clear();
		triangleDictionary.Clear();
		outlines.Clear();
		checkedOutlines.Clear();

		for (int x = 0; x < squareGrid.squares.GetLength(0); x++)
			for (int y = 0; y < squareGrid.squares.GetLength(1); y++)
				TriangulateSquareFloor(squareGrid.squares[x, y]);

		Mesh mesh = new Mesh();
		
		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray();
		mesh.RecalculateNormals();

		floor.mesh = mesh;
		floor.GetComponent<MeshCollider>().sharedMesh = mesh;
	}

	void TriangulateSquareFloor(Square square) {
		switch (15 - square.configuration) {
			case 0:
				break;

			// 1 points:
			case 1:
				MeshFromPoints(square.centreLeft, square.centreBottom, square.bottomLeft);
				break;
			case 2:
				MeshFromPoints(square.bottomRight, square.centreBottom, square.centreRight);
				break;
			case 4:
				MeshFromPoints(square.topRight, square.centreRight, square.centreTop);
				break;
			case 8:
				MeshFromPoints(square.topLeft, square.centreTop, square.centreLeft);
				break;

			// 2 points:
			case 3:
				MeshFromPoints(square.centreRight, square.bottomRight, square.bottomLeft, square.centreLeft);
				break;
			case 6:
				MeshFromPoints(square.centreTop, square.topRight, square.bottomRight, square.centreBottom);
				break;
			case 9:
				MeshFromPoints(square.topLeft, square.centreTop, square.centreBottom, square.bottomLeft);
				break;
			case 12:
				MeshFromPoints(square.topLeft, square.topRight, square.centreRight, square.centreLeft);
				break;
			case 5:
				MeshFromPoints(square.centreTop, square.topRight, square.centreRight, square.centreBottom, square.bottomLeft, square.centreLeft);
				break;
			case 10:
				MeshFromPoints(square.topLeft, square.centreTop, square.centreRight, square.bottomRight, square.centreBottom, square.centreLeft);
				break;

			// 3 point:
			case 7:
				MeshFromPoints(square.centreTop, square.topRight, square.bottomRight, square.bottomLeft, square.centreLeft);
				break;
			case 11:
				MeshFromPoints(square.topLeft, square.centreTop, square.centreRight, square.bottomRight, square.bottomLeft);
				break;
			case 13:
				MeshFromPoints(square.topLeft, square.topRight, square.centreRight, square.centreBottom, square.bottomLeft);
				break;
			case 14:
				MeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.centreBottom, square.centreLeft);
				break;

			// 4 point:
			case 15:
				MeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.bottomLeft);
				break;
		}
	}
	#endregion

	#region roof
	void CreateRoofMesh() {
		triangles.Clear();

		triangleDictionary.Clear();
		outlines.Clear();
		checkedOutlines.Clear();

		for (int x = 0; x < squareGrid.squares.GetLength(0); x++)
			for (int y = 0; y < squareGrid.squares.GetLength(1); y++)
				TriangulateSquareRoof(squareGrid.squares[x, y]);

		Mesh mesh = new Mesh();

		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray();
		mesh.RecalculateNormals();

		roof.mesh = mesh;
		roofMinimap.mesh = mesh;
		//roof.GetComponent<MeshCollider>().sharedMesh = mesh;
	}

	void TriangulateSquareRoof(Square square) {
		switch (square.configuration) {
			case 0:
				break;

			// 1 points:
			case 1:
				MeshFromPoints(square.centreLeft, square.centreBottom, square.bottomLeft);
				break;
			case 2:
				MeshFromPoints(square.bottomRight, square.centreBottom, square.centreRight);
				break;
			case 4:
				MeshFromPoints(square.topRight, square.centreRight, square.centreTop);
				break;
			case 8:
				MeshFromPoints(square.topLeft, square.centreTop, square.centreLeft);
				break;

			// 2 points:
			case 3:
				MeshFromPoints(square.centreRight, square.bottomRight, square.bottomLeft, square.centreLeft);
				break;
			case 6:
				MeshFromPoints(square.centreTop, square.topRight, square.bottomRight, square.centreBottom);
				break;
			case 9:
				MeshFromPoints(square.topLeft, square.centreTop, square.centreBottom, square.bottomLeft);
				break;
			case 12:
				MeshFromPoints(square.topLeft, square.topRight, square.centreRight, square.centreLeft);
				break;
			case 5:
				MeshFromPoints(square.centreTop, square.topRight, square.centreRight, square.centreBottom, square.bottomLeft, square.centreLeft);
				break;
			case 10:
				MeshFromPoints(square.topLeft, square.centreTop, square.centreRight, square.bottomRight, square.centreBottom, square.centreLeft);
				break;

			// 3 point:
			case 7:
				MeshFromPoints(square.centreTop, square.topRight, square.bottomRight, square.bottomLeft, square.centreLeft);
				break;
			case 11:
				MeshFromPoints(square.topLeft, square.centreTop, square.centreRight, square.bottomRight, square.bottomLeft);
				break;
			case 13:
				MeshFromPoints(square.topLeft, square.topRight, square.centreRight, square.centreBottom, square.bottomLeft);
				break;
			case 14:
				MeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.centreBottom, square.centreLeft);
				break;

			// 4 point:
			case 15:
				MeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.bottomLeft);
				checkedOutlines.Add(square.topLeft.vertexIndex);
				checkedOutlines.Add(square.topRight.vertexIndex);
				checkedOutlines.Add(square.bottomRight.vertexIndex);
				checkedOutlines.Add(square.bottomLeft.vertexIndex);
				break;
		}
	}

	void MeshFromPoints(params Node[] points) {
		AssignVertices(points);

		if (points.Length >= 3)
			CreateTriangle(points[0], points[1], points[2]);
		if (points.Length >= 4)
			CreateTriangle(points[0], points[2], points[3]);
		if (points.Length >= 5)
			CreateTriangle(points[0], points[3], points[4]);
		if (points.Length >= 6)
			CreateTriangle(points[0], points[4], points[5]);
	}

	void AssignVertices(Node[] points) {
		for (int i = 0; i < points.Length; i++) {
			if (points[i].vertexIndex == -1) {
				points[i].vertexIndex = vertices.Count;
				vertices.Add(points[i].position);
			}
		}
	}

	void CreateTriangle(Node a, Node b, Node c) {
		triangles.Add(a.vertexIndex);
		triangles.Add(b.vertexIndex);
		triangles.Add(c.vertexIndex);

		Triangle triangle = new Triangle(a.vertexIndex, b.vertexIndex, c.vertexIndex);
		AddTriangleToDict(a, triangle);
		AddTriangleToDict(b, triangle);
		AddTriangleToDict(c, triangle);
	}

	void AddTriangleToDict(Node vertex, Triangle triangle) {
		if (triangleDictionary.ContainsKey(vertex.vertexIndex))
			triangleDictionary[vertex.vertexIndex].Add(triangle);
		else
			triangleDictionary.Add(vertex.vertexIndex, new List<Triangle>() { triangle });
	}
	#endregion

	#region walls
	void CreateWallMesh() {
		List<Vector3> wallVertices = new List<Vector3>();
		List<int> wallTriangles = new List<int>();
		float wallHeight = 5;

		CalculateMeshOutlines();

		foreach (var outline in outlines) {
			for (int i = 0; i < outline.Count - 1; ++i) {
				int startIndex = wallVertices.Count;
				wallVertices.Add(vertices[outline[i]]); //left
				wallVertices.Add(vertices[outline[i + 1]]); //right
				wallVertices.Add(vertices[outline[i]] - Vector3.up * wallHeight);   //bottom left
				wallVertices.Add(vertices[outline[i + 1]] - Vector3.up * wallHeight);   //bottom right

				wallTriangles.Add(startIndex);
				wallTriangles.Add(startIndex + 2);
				wallTriangles.Add(startIndex + 3);

				wallTriangles.Add(startIndex + 3);
				wallTriangles.Add(startIndex + 1);
				wallTriangles.Add(startIndex);
			}
		}

		Mesh mesh = new Mesh();
		
		mesh.vertices = wallVertices.ToArray();
		mesh.triangles = wallTriangles.ToArray();
		mesh.RecalculateNormals();

		walls.mesh = mesh;
		//walls.GetComponent<MeshCollider>().sharedMesh = mesh;
	}

	void CalculateMeshOutlines() {
		for (int vertexIndex = 0; vertexIndex < vertices.Count; ++vertexIndex) {
			if (!checkedOutlines.Contains(vertexIndex)) {
				int newOutlineVert = GetConnectedOutlineVertex(vertexIndex);
				if (newOutlineVert != -1) {
					checkedOutlines.Add(vertexIndex);

					List<int> newOutline = new List<int>();
					newOutline.Add(vertexIndex);
					outlines.Add(newOutline);
					FollowOutline(newOutlineVert, outlines.Count - 1);
					outlines[outlines.Count - 1].Add(vertexIndex);
				}
			}
		}
	}

	void FollowOutline(int vertId, int outlineId) {
		int nextVertId = vertId;

		while (nextVertId != -1) {
			outlines[outlineId].Add(nextVertId);
			checkedOutlines.Add(nextVertId);
			nextVertId = GetConnectedOutlineVertex(nextVertId);
		}
	}

	int GetConnectedOutlineVertex(int vectexIndex) {
		List<Triangle> trianglesContainingVertex = triangleDictionary[vectexIndex];

		for (int i = 0; i < trianglesContainingVertex.Count; i++) {
			Triangle triangle = trianglesContainingVertex[i];

			for (byte j = 0; j < 3; ++j)
				if (vectexIndex != triangle[j] && !checkedOutlines.Contains(triangle[j]) && IsOutlineEdge(vectexIndex, triangle[j]))
					return triangle[j];
		}

		return -1;
	}

	bool IsOutlineEdge(int vertexA, int vertexB) {
		List<Triangle> trianglesContainingVertexA = triangleDictionary[vertexA];
		int sharedTriangleCount = 0;

		for (int i = 0; i < trianglesContainingVertexA.Count; i++)
			if (trianglesContainingVertexA[i].Contains(vertexB))
				if (++sharedTriangleCount > 1)
					break;
		return sharedTriangleCount == 1;
	}

	#endregion

	void Generate2DColliders() {
		EdgeCollider2D[] currentColliders = gameObject.GetComponents<EdgeCollider2D>();
		for (int i = 0; i < currentColliders.Length; i++)
			Destroy(currentColliders[i]);

		CalculateMeshOutlines();

		foreach (List<int> outline in outlines) {
			EdgeCollider2D edgeCollider = gameObject.AddComponent<EdgeCollider2D>();
			Vector2[] edgePoints = new Vector2[outline.Count];

			for (int i = 0; i < outline.Count; i++)
				edgePoints[i] = new Vector2(vertices[outline[i]].x, vertices[outline[i]].z);
			edgeCollider.points = edgePoints;
		}
	}

	struct Triangle {
		public int vertexIndexA;
		public int vertexIndexB;
		public int vertexIndexC;

		public int this[int key] {
			get {
				switch (key) {
					case 0:
						return vertexIndexA;
					case 1:
						return vertexIndexB;
					case 2:
						return vertexIndexC;
					default:
						return -1;
				}
			}
			set {
				switch (key) {
					case 0:
						vertexIndexA = value;
						break;
					case 1:
						vertexIndexB = value;
						break;
					case 2:
						vertexIndexC = value;
						break;
				}
			}
		}

		public Triangle(int a, int b, int c) {
			vertexIndexA = a;
			vertexIndexB = b;
			vertexIndexC = c;
		}

		public bool Contains(int id) {
			return vertexIndexA == id || vertexIndexB == id || vertexIndexC == id;
		}
	}

	public class SquareGrid {
		public Square[,] squares;

		public SquareGrid(Tile[,] map, float squareSize) {
			int nodeCountX = map.GetLength(0);
			int nodeCountY = map.GetLength(1);
			float mapWidth = nodeCountX * squareSize;
			float mapHeight = nodeCountY * squareSize;

			ControlNode[,] controlNodes = new ControlNode[nodeCountX, nodeCountY];

			for (int x = 0; x < nodeCountX; x++) {
				for (int y = 0; y < nodeCountY; y++) {
					Vector3 pos = new Vector3(-mapWidth / 2 + x * squareSize + squareSize / 2, 0, -mapHeight / 2 + y * squareSize + squareSize / 2);
					controlNodes[x, y] = new ControlNode(pos, map[x, y].isSolid, squareSize);
				}
			}

			squares = new Square[nodeCountX - 1, nodeCountY - 1];
			for (int x = 0; x < nodeCountX - 1; x++) {
				for (int y = 0; y < nodeCountY - 1; y++) {
					squares[x, y] = new Square(controlNodes[x, y + 1], controlNodes[x + 1, y + 1], controlNodes[x + 1, y], controlNodes[x, y]);
				}
			}

		}
	}

	public class Square {
		public ControlNode topLeft, topRight, bottomRight, bottomLeft;
		public Node centreTop, centreRight, centreBottom, centreLeft;
		public int configuration;

		public Square(ControlNode _topLeft, ControlNode _topRight, ControlNode _bottomRight, ControlNode _bottomLeft) {
			topLeft = _topLeft;
			topRight = _topRight;
			bottomRight = _bottomRight;
			bottomLeft = _bottomLeft;

			centreTop = topLeft.right;
			centreRight = bottomRight.above;
			centreBottom = bottomLeft.right;
			centreLeft = bottomLeft.above;

			if (topLeft.active)
				configuration += 8;
			if (topRight.active)
				configuration += 4;
			if (bottomRight.active)
				configuration += 2;
			if (bottomLeft.active)
				configuration += 1;
		}
	}

	public class Node {
		public Vector3 position;
		public int vertexIndex = -1;

		public Node(Vector3 _pos) {
			position = _pos;
		}
	}

	public class ControlNode : Node {
		public bool active;
		public Node above, right;

		public ControlNode(Vector3 _pos, bool _active, float squareSize) : base(_pos) {
			active = _active;
			above = new Node(position + Vector3.forward * squareSize / 2f);
			right = new Node(position + Vector3.right * squareSize / 2f);
		}
	}
}