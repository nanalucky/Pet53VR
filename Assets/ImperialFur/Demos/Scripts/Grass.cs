using UnityEngine;
using System.Collections;

public class Grass : MonoBehaviour {
	public Material[] mats;

	private Renderer render;
	private int matIndex = 0;

	// Use this for initialization
	void Start () {
		render = gameObject.GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("space")) {
			matIndex = 1 - matIndex;
			render.material = mats[matIndex];
		}
	}
}
