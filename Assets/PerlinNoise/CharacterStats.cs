using UnityEngine;
using System.Collections;

public class CharacterStats : MonoBehaviour {

	[SerializeField] PerlinNoise noise;

	void Start () {

	}

	void Update () {

		MapMovement();
	}

	void MapMovement()
	{
		noise.xScroll += transform.forward.x * Time.deltaTime;
		noise.zScroll += transform.forward.z * Time.deltaTime;
	}
}
