using UnityEngine;
using System.Collections;

public class ForceHandler : MonoBehaviour {

	public float torque = 1f;
	public Rigidbody rb;
	public bool isTriggerStay = false;

	void Start() {
		rb = GetComponent<Rigidbody>();
//		isTriggerStay = true;
	}

	void FixedUpdate() {
//		float turn = Input.GetAxis("Horizontal");

		if (!isTriggerStay)
			return;

		rb.AddTorque(new Vector3(0f, torque * Time.deltaTime, 0f));
	}

	void OnTriggerStay(Collider other) {		
		if (other.tag == "Player") {
			Debug.Log ("OnTriggerStay :: " + other.name);
			isTriggerStay = true;
		}
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log ("OnTriggerEnter :: " + other.name);
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "Player") {
			Debug.Log ("OnTriggerExit :: " + other.name);
			isTriggerStay = false;
		}
	}
}
