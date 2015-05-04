using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChunkMananger : MonoBehaviour {

	public GameObject chunk;
	List<GameObject> chunks;
	[SerializeField] int renderDistance = 2;
	[SerializeField] int chunkWidth = 30;

	void Start () {
		chunks = new List<GameObject>();
		for(int i = 0; i < renderDistance; i++)
		{
			for(int j = 0; j < renderDistance; j++)
			{
				GameObject temp = (GameObject) Instantiate(chunk, new Vector3(transform.position.x + i * chunkWidth, 0f, transform.position.z + j * chunkWidth) , Quaternion.identity);
				chunks.Add(temp);
			}
		}
	}
	
	void NewXScroll()
	{
		foreach(GameObject c in chunks)
			c.GetComponent<PerlinNoise>().NewXScroll();
	}

	void NewZScroll()
	{
		foreach(GameObject c in chunks)
			c.GetComponent<PerlinNoise>().NewZScroll();
	}
}
