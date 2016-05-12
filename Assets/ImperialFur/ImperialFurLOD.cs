using UnityEngine;
using System.Collections;

public class ImperialFurLOD : MonoBehaviour {
	public float from40To20;
	public float from20To10;
	public float from10To5;
	public float from5To2;
	public float from2To1;

	private Material material;
	private string shaderBase;
	private Shader[] shaders;
	private int lodLevel;
	private ImperialFurPhysics physicsScript;

	// Use this for initialization
	void Start () {
		material = gameObject.GetComponent<Renderer>().material;
		int index = material.shader.name.LastIndexOf('/');
		shaderBase = material.shader.name.Substring(0, index+1);

		shaders = new Shader[6];
		shaders[0] = Shader.Find(shaderBase + "40 Shell"); 
		shaders[1] = Shader.Find(shaderBase + "20 Shell"); 
		shaders[2] = Shader.Find(shaderBase + "10 Shell"); 
		shaders[3] = Shader.Find(shaderBase + " 5 Shell"); 
		shaders[4] = Shader.Find(shaderBase + " 2 Shell"); 
		shaders[5] = Shader.Find(shaderBase + " 1 Shell"); 

		lodLevel = -1;

		physicsScript = GetComponent<ImperialFurPhysics>();
	}
	
	// Update is called once per frame
	void Update () {
		// Get camera distance
		Vector3 heading = transform.position - Camera.main.transform.position;
		float distance = Vector3.Dot(heading, Camera.main.transform.forward);

		if (distance > from2To1) {
			if (lodLevel != 5) { 
				lodLevel = 5;
				material.shader = shaders[5];
				if (physicsScript != null)
					physicsScript.UpdatePhysics();
			}
		} else if (distance > from5To2) {
			if (lodLevel != 4) { 
				lodLevel = 4;
				material.shader = shaders[4];
				if (physicsScript != null)
					physicsScript.UpdatePhysics();
			}
		} else if (distance > from10To5) {
			if (lodLevel != 3) { 
				lodLevel = 3;
				material.shader = shaders[3];
				if (physicsScript != null)
					physicsScript.UpdatePhysics();
			}
		} else if (distance > from20To10) {
			if (lodLevel != 2) { 
				lodLevel = 2;
				material.shader = shaders[2];
				if (physicsScript != null)
					physicsScript.UpdatePhysics();
			}
		} else if (distance > from40To20) {
			if (lodLevel != 1) { 
				lodLevel = 1;
				material.shader = shaders[1];
				if (physicsScript != null)
					physicsScript.UpdatePhysics();
			}
		} else if (lodLevel != 0) {
			lodLevel = 0;
			material.shader = shaders[0];
			if (physicsScript != null)
				physicsScript.UpdatePhysics();
		}
	}

	public void Reset() {
		lodLevel = -1;
	}
}
