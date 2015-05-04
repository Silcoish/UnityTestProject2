using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

/* TODO:
 * Allow Materials instead of colours?
 * Delete all terrains, not just one
 * Allow users to supply own shader
 * Make into 1 whole mesh
 * Collision mesh (maybe an option?)
 * Allow window to scroll is needed
 * Different block size?
 * Presets?
 * New name: TerrainWindow -> Voxel Terrain
 */

public class TerrainWindow : EditorWindow {

	GameObject cube;
	GameObject terrain;

	
	int size = 10;
	float scale = 5f;
	float heightModifyer = 1f;
	float xScroll = 0f;
	float zScroll = 0f;
	bool move = false;
	bool smooth = false;
	bool mapColour = true;
	int colourAmounts = 1;
	List<Color> heightColors = new List<Color>();
	List<Material> mats = new List<Material>();

	[MenuItem("Window/Terrain")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(TerrainWindow));
	}

	void OnGUI()
	{
		GUILayout.Label ("Voxel Terrain Generator", EditorStyles.boldLabel);

		//SIZE, SCALE, HEIGHT, SMOOTH
		size = (int)EditorGUILayout.Slider("Terrain Size:", size, 1, 1000);
		scale = (int)EditorGUILayout.Slider("Terrain Scale:", scale, 1, 100);
		heightModifyer = (int)EditorGUILayout.Slider("Terrain HeightModifyer:", heightModifyer, 1, 100);
		EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Smoother Terrain?");
			smooth = EditorGUILayout.Toggle(smooth);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();
		EditorGUILayout.Space();

		//OFFSETS
		EditorGUILayout.BeginHorizontal();
			GUILayout.Label("XOffset: ");
			xScroll = (int)EditorGUILayout.Slider(xScroll, 0, 1000);
			GUILayout.Label("ZOffset: ");	
			zScroll = (int)EditorGUILayout.Slider(zScroll, 0, 1000);
		EditorGUILayout.EndHorizontal();

		if (GUILayout.Button("New Offset"))
		{
			xScroll = Random.Range(0f, 1000f);
			zScroll = Random.Range(0f, 1000f);

			ShowNotification(new GUIContent("New offset created. Click \"generate\" to create a new terrain."));
		}

		EditorGUILayout.Space();
		EditorGUILayout.Space();

		//COLOURS
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Height Colors (highest to lowest):");
		colourAmounts = (int)EditorGUILayout.Slider(colourAmounts, 1, 10);
		EditorGUILayout.EndHorizontal();

		//Checks for increase
		for (int a = 0; a < colourAmounts; a++)
		{
			if (a > (heightColors.Count - 1))
			{
				heightColors.Add(new Color(0, 0, 0, 1));
			}
		}

		//Checks for decrease
		for (int b = 0; b < heightColors.Count; b++)
		{
			if (b >= colourAmounts)
				heightColors.RemoveAt(b);
		}

		for (int i = 0; i < heightColors.Count; i++)
		{
			heightColors[i] = EditorGUILayout.ColorField(heightColors[i]);
		}

		EditorGUILayout.Space();
		EditorGUILayout.Space();

		//MAKE AND DELETE
		if(GUILayout.Button("Generate Terrain"))
		{
			ShowNotification(new GUIContent("Terrain being generated... Please allow a few seconds."));
			SetUp();

			ShowNotification(new GUIContent("New terrain generated."));
		}

		if(GUILayout.Button("Delete Terrain"))
		{
			DeleteTerrain();

			ShowNotification(new GUIContent("Terrain deleted."));
		}

	}

	void DeleteTerrain()
	{
		DestroyImmediate(GameObject.Find("VoxelTerrain"));
		DestroyImmediate(GameObject.Find("Voxel"));
	}

	void CreateCube()
	{
		cube = new GameObject("Voxel");
		cube.AddComponent<MeshFilter>();
		cube.AddComponent<MeshRenderer>();

		Mesh mesh = new Mesh();

		mesh.Clear();
		mesh.name = "Custom Cube";
		mesh.vertices = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0), new Vector3(1, 0, 0), //front
										new Vector3(1, 0, 1), new Vector3(1, 1, 1), new Vector3(0, 1, 1), new Vector3(0, 0, 1), //back
 										new Vector3(0, 1, 0), new Vector3(0, 1, 1), new Vector3(1, 1, 1), new Vector3(1, 1, 0), //top
										new Vector3(0, 0, 1), new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(1, 0, 1), //bottom
										new Vector3(0, 0, 1), new Vector3(0, 1, 1), new Vector3(0, 1, 0), new Vector3(0, 0, 0), //left
										new Vector3(1, 0, 0), new Vector3(1, 1, 0), new Vector3(1, 1, 1), new Vector3(1, 0, 1)}; //right
		mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3, 4, 5, 6, 4, 6, 7, 8, 9, 10, 8, 10, 11, 12, 13, 14, 12, 14, 15, 16, 17, 18, 16, 18, 19, 20, 21, 22, 20, 22, 23};

		cube.GetComponent<MeshFilter>().mesh = mesh;

		//apply material
		Shader shader = Shader.Find("Diffuse");
		Material mat = new Material(shader);
		cube.GetComponent<MeshRenderer>().sharedMaterial = mat;

		cube.GetComponent<MeshFilter>().sharedMesh.RecalculateNormals();
	}

	void SetUp()
	{
		/* TODO: Find all called voxel terrain, not just 1 */

		DestroyImmediate(GameObject.Find("VoxelTerrain"));
		DestroyImmediate(GameObject.Find("Voxel"));
		terrain = new GameObject("VoxelTerrain");
		terrain.transform.position = Vector3.zero;

		CreateCube();
		for (int i = 0; i < size; i++)
		{
			for (int j = 0; j < size; j++)
			{
				GameObject temp = (GameObject)Instantiate(cube, new Vector3(terrain.transform.position.x + i, 0, terrain.transform.position.z + j), Quaternion.identity);
				temp.transform.parent = terrain.transform;
			}
		}

		//Set up colour materials
		mats.Clear();
		Shader shader = Shader.Find("Diffuse");
		for (int i = 0; i < heightColors.Count; i++)
		{
			mats.Add(new Material(shader));
			mats[i].color = heightColors[i];
		}

		SetPosition();
	}
	void SetPosition()
	{
		foreach (Transform child in terrain.transform)
		{
			float h = Mathf.PerlinNoise((child.transform.position.x + xScroll) / scale, (child.transform.position.z + zScroll) / scale);
			if (!smooth)
			{
				child.transform.position = new Vector3(child.transform.position.x, (Mathf.RoundToInt(h * heightModifyer)), child.transform.position.z);
				child.transform.localScale = new Vector3(child.transform.localScale.x, 1.0f + (Mathf.RoundToInt(h * heightModifyer)), child.transform.localScale.z);
			}
			else
			{
				child.transform.position = new Vector3(child.transform.position.x, (h * heightModifyer), child.transform.position.z);
				child.transform.localScale = new Vector3(child.transform.localScale.x, 1.0f + (h * heightModifyer), child.transform.localScale.z);
			}

			for (int i = 0; i < heightColors.Count; i++)
			{
				//Debug.Log("Y: " + child.transform.position.y + ">=" + (heightModifyer * (i * (1 / (float)heightColors.Count))));
				if (child.transform.position.y >= (heightModifyer * (i * (1 / (float)heightColors.Count))))
				{
					//Debug.Log("Drawing colour?");
					child.GetComponent<Renderer>().material = mats[heightColors.Count - 1 - i];//heightColors[heightColors.Count - 1 - i];
				}
			}
		}

		DestroyImmediate(GameObject.Find("Voxel"));
	}

	
}
