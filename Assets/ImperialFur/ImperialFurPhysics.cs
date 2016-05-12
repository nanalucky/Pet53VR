using UnityEngine;
using System.Collections;

public class ImperialFurPhysics : MonoBehaviour {
	public bool useRigidbody = true;
	public bool usePhysicsGravity = true;
	public bool physicsEnabled = true;
	public bool windEnabled = false;
	public Vector3 AdditionalGravity;
	public float forceScale = 1f;
	public float gravityScale = 0.25f;
	public float forceDamping = 3;

	private Material material;
	private Rigidbody rigidBody;
	private Transform thisTransform;
	private Vector3 oldPosition;
	private Vector3 forceSmooth = Vector3.zero;

	void Start () {
//		Time.timeScale = 0f;
		rigidBody = gameObject.GetComponent<Rigidbody>();
		material = gameObject.GetComponent<Renderer>().material;
		thisTransform = transform;
		oldPosition = thisTransform.position;

		if (rigidBody == null && useRigidbody) {
			Debug.LogWarning("No Rigidbody attached to fur object. Defaulting to non-Rigidbody simulation");
			useRigidbody = false;
		}
	}

	void Update() {
		if (useRigidbody || Time.timeScale == 0)
			return;

		Vector3 force = Vector3.zero;
		if (physicsEnabled && !useRigidbody) {
			Vector3 movement = oldPosition - thisTransform.position;
			force = movement / Time.deltaTime;
			oldPosition = thisTransform.position;
			force *= forceScale;
		}

		CalculateAdditionalForce(force);
	}

	void FixedUpdate () {
		if (!useRigidbody)
			return;

		Vector3 force = Vector3.zero;
		if (physicsEnabled) {
			force = -rigidBody.velocity;
			force *= forceScale;
		}

		CalculateAdditionalForce(force);
	}

	void CalculateAdditionalForce(Vector3 force) {
		if (usePhysicsGravity)
			force += Physics.gravity * gravityScale; 
		force += AdditionalGravity;

		if (windEnabled) {
			force += ImperialFurWind.windForce;
		}

		force = Vector3.ClampMagnitude(force, 1.0f);
	
		forceSmooth = Vector3.Lerp(forceSmooth, force, Time.deltaTime * forceDamping);
		material.SetVector("Displacement", transform.InverseTransformDirection(forceSmooth));
	}

	// Used by the LOD script to update the material when the shader changes
	public void UpdatePhysics() {
		if (enabled)
			material.SetVector("Displacement", transform.InverseTransformDirection(forceSmooth));
	}

	
	public void UpdateMaterial() {
		material = gameObject.GetComponent<Renderer>().material;
	}

}
