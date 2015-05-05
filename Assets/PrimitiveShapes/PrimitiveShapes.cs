using UnityEngine;
using UnityEditor;
using System.Collections;

public class PrimitiveShapes : MonoBehaviour {

	[MenuItem("GameObject/Shapes/HexagonalPrism")]
	private static void HexagonalPrism()
	{
		CreateHexagonalPrism();
	}
	
	private static void CreateHexagonalPrism()
	{
		GameObject hexagonalPrism = new GameObject("HexagonalPrism");

		Vector3[] verticies = 
		{
			//Bottom vertices
			new Vector3(0, 0, 0),
			new Vector3(0.25f, 0, -0.5f),
			new Vector3(0.5f, 0, 0),
			new Vector3(0.75f, 0, -0.5f),
			new Vector3(1, 0, 0),
			new Vector3(0.75f, 0, 0.5f),
			new Vector3(0.25f, 0, 0.5f),

			//top vertices
			new Vector3(0, 1, 0),
			new Vector3(0.25f, 1, -0.5f),
			new Vector3(0.5f, 1, 0),
			new Vector3(0.75f, 1, -0.5f),
			new Vector3(1, 1, 0),
			new Vector3(0.75f, 1, 0.5f),
			new Vector3(0.25f, 1, 0.5f)
		};

		int[] triangles =
		{
			//bottom
			1, 2, 0, 
			3, 2, 1,
			4, 2, 3,
			5, 2, 4,
			6, 2, 5,
			0, 2, 6,

			//top
			7, 9, 8,
			8, 9, 10,
			10, 9, 11,
			11, 9, 12, 
			12, 9, 13, 
			13, 9, 7,

			//sides
			0, 7, 1, 
			1, 7, 8,
			1, 8, 3,
			3, 8, 10,
			3, 10, 4,
			4, 10, 11,
			4, 11, 5,
			5, 11, 12,
			5, 12, 6,
			6, 12, 13,
			6, 13, 0,
			0, 13, 7
		};

		Mesh mesh = new Mesh();
		MeshFilter filter = hexagonalPrism.AddComponent<MeshFilter>();
		hexagonalPrism.AddComponent<MeshRenderer>();

		mesh.Clear();
		mesh.vertices = verticies;
		mesh.triangles = triangles;

		mesh.RecalculateNormals();

		filter.mesh = mesh;

		ApplyMaterial(hexagonalPrism);

		hexagonalPrism.transform.position = Vector3.zero;
	}

	private static void ApplyMaterial(GameObject go)
	{
		Shader shader = Shader.Find("Diffuse");
		Material mat = new Material(shader);
		go.GetComponent<MeshRenderer>().sharedMaterial = mat;
	}
}
