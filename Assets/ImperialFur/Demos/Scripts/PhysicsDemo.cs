using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PhysicsDemo : MonoBehaviour {
	public GameObject[] spheres;
	public Text shellText;
	public Text shaderText;

	const int shell10 = 0;
	const int shell20 = 1;
	const int shell40 = 2;

	const int typeMain = 0;
	const int typeSimple = 1;

	int shellCount;
	int type;

	// Use this for initialization
	void Start () {
#if UNITY_IOS || UNITY_ANDROID
		type = typeSimple;
		shellCount = shell10;
		SetShaders();
#else 
		type = typeMain;
		shellCount = shell20;
		SetShaders();
#endif
	}

	public void ToggleShellCount() {
		shellCount++;

		if (shellCount > shell40)
			shellCount = shell10;

		SetShaders();
	}

	public void ToggleShader() {
		type++;
		if (type > typeSimple)
			type = typeMain;
				
		SetShaders();
	}

	void SetShaders() {
		string shaderName = "";

		switch(type) {
		case typeMain:
			shaderText.text = "Shader: Main";
			shaderName = "Imperial Fur/Main/Specular/";
			break;
		case typeSimple:
			shaderText.text = "Shader: Simple";
			shaderName = "Imperial Fur/Simple/Specular Skin/";
			break;
		}

		switch(shellCount) {
		case shell10:
			shellText.text = "Shells: 10";
			shaderName += "10 Shell";
			break;
		case shell20:
			shellText.text = "Shells: 20";
			shaderName += "20 Shell";
			break;
		case shell40:
			shellText.text = "Shells: 40";
			shaderName += "40 Shell";
			break;
		}
		Debug.Log(shaderName);
		for (int i = 0; i < 6; i++) {
			Material material = spheres[i].GetComponent<Renderer>().material;
			material.shader = Shader.Find(shaderName);
		}
	}

	public void HandleBack() {
		Application.LoadLevel("FurDemo");
	}
}
