using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PolygonGenerator : MonoBehaviour {

	public List<Vector3> newVerticies = new List<Vector3>();
	public List<int> newTriangles = new List<int>();
	public List<Vector2> newUV = new List<Vector2>();
	private int squareCount;

	public List<Vector3> colVerticies = new List<Vector3>();
	public List<int> colTriangles = new List<int>();
	private int colCount;

	private float tUnit = 0.25f;
	private Vector2 tStone = new Vector2(0, 0);
	private Vector2 tGrass = new Vector2(0, 1);

	private Mesh mesh;
	private MeshCollider col;

	private byte[,] blocks;

	void Start()
	{
		mesh = GetComponent<MeshFilter>().mesh;
		col = GetComponent<MeshCollider>();

		GenTerrain();
		BuildMesh();
		UpdateMesh();
	}

	void GenSquare(int x, int y, Vector2 texture)
	{
		newVerticies.Add(new Vector3(x, y, 0));
		newVerticies.Add(new Vector3(x + 1, y, 0));
		newVerticies.Add(new Vector3(x + 1, y - 1, 0));
		newVerticies.Add(new Vector3(x, y - 1, 0));

		newTriangles.Add(squareCount*4);
		newTriangles.Add(squareCount * 4 + 1);
		newTriangles.Add(squareCount * 4 + 3);
		newTriangles.Add(squareCount * 4 + 1);
		newTriangles.Add(squareCount * 4 + 2);
		newTriangles.Add(squareCount * 4 + 3);

		newUV.Add(new Vector2(tUnit * texture.x, tUnit * texture.y + tUnit));
		newUV.Add(new Vector2(tUnit * texture.x + tUnit, tUnit * texture.y + tUnit));
		newUV.Add(new Vector2(tUnit * texture.x + tUnit, tUnit * texture.y));
		newUV.Add(new Vector2(tUnit * texture.x, tUnit * texture.y));

		squareCount++;
	}

	void UpdateMesh()
	{
		mesh.Clear();
		mesh.vertices = newVerticies.ToArray();
		mesh.triangles = newTriangles.ToArray();
		mesh.uv = newUV.ToArray();
		mesh.Optimize();
		mesh.RecalculateNormals();

		//Collider
		Mesh newMesh = new Mesh();
		newMesh.vertices = colVerticies.ToArray();
		newMesh.triangles = colTriangles.ToArray();
		col.sharedMesh = newMesh;

		colVerticies.Clear();
		colTriangles.Clear();
		colCount = 0;
		//End Collider

		squareCount = 0;
		newVerticies.Clear();
		newTriangles.Clear();
		newUV.Clear();
	}

	void GenTerrain()
	{
		blocks = new byte[10, 10];

		for(int px = 0; px < blocks.GetLength(0); px++)
		{
			for (int py = 0; py < blocks.GetLength(1); py++)
			{
				if(px == 5)
				{

				}
				else if (py == 5)
				{
					blocks[px, py] = 2;
				}
				else if (py < 5)
				{
					blocks[px, py] = 1;
				}
			}
		}
	}

	void BuildMesh()
	{
		for (int px = 0; px < blocks.GetLength(0); px++)
		{
			for (int py = 0; py < blocks.GetLength(1); py++)
			{
				if (blocks[px, py] != 0)
					GenCollider(px, py);

				if (blocks[px, py] == 1)
					GenSquare(px, py, tStone);
				else if (blocks[px, py] == 2)
					GenSquare(px, py, tGrass);
			}
		}
	}

	byte Block(int x, int y)
	{
		if(x == -1 || x == blocks.GetLength(0) || y == -1 || y == blocks.GetLength(1))
			return (byte)1;

		return blocks[x, y];
	}
	void GenCollider(int x, int y)
	{
		//top
		if (Block(x, y + 1) == 0)
		{
			colVerticies.Add(new Vector3(x, y, 1));
			colVerticies.Add(new Vector3(x + 1, y, 1));
			colVerticies.Add(new Vector3(x + 1, y, 0));
			colVerticies.Add(new Vector3(x, y, 0));

			ColliderTriangles();
			colCount++;
		}

		//bottom
		if (Block(x, y - 1) == 0)
		{
			colVerticies.Add(new Vector3(x, y - 1, 0));
			colVerticies.Add(new Vector3(x + 1, y - 1, 0));
			colVerticies.Add(new Vector3(x + 1, y - 1, 1));
			colVerticies.Add(new Vector3(x, y - 1, 1));

			ColliderTriangles();
			colCount++;
		}

		//left
		if (Block(x - 1, y) == 0)
		{
			colVerticies.Add(new Vector3(x, y - 1, 1));
			colVerticies.Add(new Vector3(x, y, 1));
			colVerticies.Add(new Vector3(x, y, 0));
			colVerticies.Add(new Vector3(x, y - 1, 0));

			ColliderTriangles();
			colCount++;
		}

		//right
		if (Block(x + 1, y) == 0)
		{
			colVerticies.Add(new Vector3(x + 1, y, 1));
			colVerticies.Add(new Vector3(x + 1, y - 1, 1));
			colVerticies.Add(new Vector3(x + 1, y - 1, 0));
			colVerticies.Add(new Vector3(x + 1, y, 0));

			ColliderTriangles();
			colCount++;
		}

	}

	void ColliderTriangles()
	{
		colTriangles.Add(colCount * 4);
		colTriangles.Add(colCount * 4 + 1);
		colTriangles.Add(colCount * 4 + 3);
		colTriangles.Add(colCount * 4 + 1);
		colTriangles.Add(colCount * 4 + 2);
		colTriangles.Add(colCount * 4 + 3);
	}
}
