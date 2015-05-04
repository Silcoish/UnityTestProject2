using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PerlinNoise : MonoBehaviour {

	[SerializeField] int size = 10;
	[SerializeField] GameObject cube;
	[SerializeField] public float scale = 5f;
	[SerializeField] public float heightModifyer = 1f;
	[SerializeField] public float xScroll = 0f;
	[SerializeField] public float zScroll = 0f;
	[SerializeField] public bool move = false;
	[SerializeField] public bool smooth = false;
	[SerializeField] public bool mapColour = true;
	[SerializeField] Color[] heightColors;

	void Start () {

		SetUp();
		RemoveColor();

		xScroll = Random.Range(0f, 1000f);
		zScroll = Random.Range(0f, 1000f);

		SetPosition();
	}

	void SetUp()
	{
		for(int i = 0; i < size; i++)
		{
			for(int j = 0; j < size; j++)
			{
				GameObject temp = (GameObject) Instantiate(cube, new Vector3(transform.position.x + i, 0, transform.position.z + j), Quaternion.identity);
				temp.transform.parent = gameObject.transform;
			}
		}
	}

	void RemoveColor()
	{
		foreach(Transform child in transform)
		{
			float h = Mathf.PerlinNoise((child.transform.position.x + /*Mathf.RoundToInt(xScroll)*/ xScroll) / scale, (child.transform.position.z + /*Mathf.RoundToInt(xScroll)*/ zScroll) / scale);
			child.GetComponent<Renderer>().material.color = new Color(h,h,h,h);
		}
	}

	void SetPosition()
	{
		foreach(Transform child in transform)
		{
			float h = Mathf.PerlinNoise((child.transform.position.x + /*Mathf.RoundToInt(xScroll)*/ xScroll) / scale, (child.transform.position.z + /*Mathf.RoundToInt(xScroll)*/ zScroll) / scale);
			if(!smooth)
			{
				child.transform.position = new Vector3(child.transform.position.x, (Mathf.RoundToInt(h * heightModifyer)), child.transform.position.z);
				child.transform.localScale = new Vector3(child.transform.localScale.x, 1.0f + (Mathf.RoundToInt(h * heightModifyer)), child.transform.localScale.z);
			}
			else
			{
				child.transform.position = new Vector3(child.transform.position.x, (h * heightModifyer), child.transform.position.z);
				child.transform.localScale = new Vector3(child.transform.localScale.x, 1.0f + (h * heightModifyer), child.transform.localScale.z);
			}

			for(int i = 0; i < heightColors.Length; i++)
			{
				//print((float)(heightModifyer * (i * (1 / (float)heightColors.Length))));
				if(child.transform.position.y >= (heightModifyer * (i * (1 / (float)heightColors.Length))))
				{
					child.GetComponent<Renderer>().material.color = heightColors[heightColors.Length - 1 - i];
				}
			}
		}
	}

	public void NewXScroll()
	{
		xScroll = Random.Range(0f, 1000f);
		SetPosition();
	}

	public void NewZScroll()
	{
		xScroll = Random.Range(0f, 1000f);
		SetPosition();
	}

	public void ToggleSmooth()
	{
		smooth = !smooth;
		SetPosition();
	}

	public void ToggleMapColour()
	{
		mapColour = !mapColour;
		if(!mapColour)
			RemoveColor();
		else
			SetPosition();
	}








}
