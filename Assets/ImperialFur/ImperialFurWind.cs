using UnityEngine;
using System.Collections;

public class ImperialFurWind : MonoBehaviour {
	public float windDamping = 0.5f;
	public float minWindForce = 0;
	public float maxWindForce = 2;

	[HideInInspector]
	public static Vector3 windForce = Vector3.zero;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float windMagnitude = (maxWindForce - minWindForce) * 2;
		float x = maxWindForce - Random.Range(0, windMagnitude);
		float y = maxWindForce - Random.Range(0, windMagnitude);
		float z = maxWindForce - Random.Range(0, windMagnitude);
		windForce = Vector3.Lerp(windForce, new Vector3(x, y, z), Time.deltaTime * windDamping);
	}
}
