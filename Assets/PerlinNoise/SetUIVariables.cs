using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SetUIVariables : MonoBehaviour {
	
	public ChunkMananger chunkMananger;
	public PerlinNoise map;

	public Toggle move;
	public Toggle smooth;
	public Slider heightMod;
	public Slider scaleMod;

	void Start () {
		map = chunkMananger.chunk.GetComponent<PerlinNoise>();
		move.isOn = map.mapColour;
		smooth.isOn = map.smooth;
		heightMod.value = Mathf.RoundToInt(map.heightModifyer);
		scaleMod.value = Mathf.RoundToInt(map.scale);
	}

	void Update()
	{
		map.heightModifyer = heightMod.value;
		map.scale = scaleMod.value;
	}
}
