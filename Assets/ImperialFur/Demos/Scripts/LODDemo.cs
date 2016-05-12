using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LODDemo : MonoBehaviour {
	public GameObject lodSphere;

	public Text buttonLabel;
	public bool lodOn = true;

	// Use this for initialization
	void Start () {
		float x = -17.5f;
		for (int i = 0; i < 15; i++) {
			float z = 1;
			for (int j = 0; j < 15; j++) {
				Instantiate(lodSphere, new Vector3(x, 1, z), Quaternion.identity);
				z += 5;
			}
			x += 2.5f;
		}

		buttonLabel.text = "LOD On: " + lodOn.ToString();
	}

	public void ToggleLod() {
		lodOn = ! lodOn;
		buttonLabel.text = "LOD On: " + lodOn.ToString();
	}

	public void HandleBack() {
		Application.LoadLevel("FurDemo");
	}
}
