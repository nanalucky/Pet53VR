using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tips : MonoBehaviour {
	public Text[] tip;
	public Material[] mat;
	public Renderer torus;
	public ImperialFurPhysics wind;

	private int currTip;

	// Use this for initialization
	void Start () {
		currTip = 0;
		ShowTip(currTip);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("space")) {
			currTip++;
			ShowTip(currTip);
		}
	}

	void ShowTip(int index) {
		if (index >= tip.Length)
			return;

		for (int i = 0; i < tip.Length; i++)
			tip[i].gameObject.SetActive(false);

		tip[index].gameObject.SetActive(true);
		torus.material = mat[index];
		wind.UpdateMaterial();
	}
}
