using UnityEngine;
using System.Collections;

public class PerlinNoisePlane : MonoBehaviour {

	[SerializeField] float scale = 1f;
	[SerializeField] float heightModifyer = 1f;

	void Update () {
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		Vector3[] verticies = mesh.vertices;
		int i = 0;
		while(i < verticies.Length)
		{
			float h = Mathf.PerlinNoise(verticies[i].x / scale, verticies[i].z / scale);
			verticies[i] = new Vector3(verticies[i].x, h * heightModifyer, verticies[i].z);
			i++;
		}

		mesh.vertices = verticies;
		mesh.RecalculateBounds();
		GetComponent<MeshCollider>().sharedMesh = mesh;
	}
}
