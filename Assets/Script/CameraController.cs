using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Runtime.InteropServices;		// required for DllImport


public class CameraController : MonoBehaviour {

	public Canvas canvas;

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("empty"))
		{
			canvas.gameObject.SetActive(true);
			GetComponent<Animator>().enabled = false;
			this.enabled = false;
		}
	}
}
