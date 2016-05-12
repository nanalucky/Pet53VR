using UnityEngine;
using System.Collections;

public class WallForce : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionStay (Collision col)
	{
		Vector3 dir = transform.up + new Vector3(0.1f, 0.1f, 0.1f);
		col.rigidbody.AddForce(dir * 125);
	}
}
