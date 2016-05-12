using UnityEngine;
using System.Collections;

public class LODSphere : MonoBehaviour {
	private ImperialFurLOD lodScript;
	private LODDemo demoScript;
	private bool lodEnabled = true;
	private Material material;
	private string shaderBase;


	IEnumerator Start () {
		lodScript = GetComponent<ImperialFurLOD>();
		demoScript = GameObject.Find("LODDemo").GetComponent<LODDemo>();

		material = gameObject.GetComponent<Renderer>().material;
		int index = material.shader.name.LastIndexOf('/');
		shaderBase = material.shader.name.Substring(0, index+1);

		Vector3 pointA = transform.position;
		Vector3 pointB = transform.position - new Vector3(0, 0, 10);
		while (true) {
			yield return StartCoroutine(MoveObject(transform, pointA, pointB, 3.0f));
			yield return StartCoroutine(MoveObject(transform, pointB, pointA, 3.0f));
		}

	}

	void Update() {
		if (demoScript.lodOn && !lodEnabled) {
			lodEnabled = true;
			lodScript.enabled = true;
			lodScript.Reset();
		}

		if (!demoScript.lodOn && lodEnabled) {
			lodEnabled = false;
			lodScript.enabled = false;
#if UNITY_IOS || UNITY_ANDROID
			material.shader = Shader.Find(shaderBase + "20 Shell");
#else
			material.shader = Shader.Find(shaderBase + "40 Shell");
#endif
		}
	}

	IEnumerator MoveObject (Transform thisTransform, Vector3 startPos, Vector3 endPos, float time) {
		float i = 0.0f;
		float rate = 0.5f / time;
		while (i < 1.0f) {
			i += Time.deltaTime * rate;
			thisTransform.position = Vector3.Lerp(startPos, endPos, i);
			yield return null; 
		}
	}
}
