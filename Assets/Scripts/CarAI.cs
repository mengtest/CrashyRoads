using UnityEngine;
using System.Collections;

public class CarAI : MonoBehaviour {

	float power = 50000;
	Rigidbody rb;

	void Start () {
		rb = GetComponent<Rigidbody>();
		rb.mass = 1500f;
	}

	void Update() {
		Vector3 force = new Vector3((power/(rb.velocity.magnitude+5f)), 0f, 0f);
		rb.AddForce(force);
	}

}
