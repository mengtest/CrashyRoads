using UnityEngine;
using System.Collections;

public class RoadTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Car") {
			GetComponentInParent<CarSpawning> ().SpawnCar ();
			Destroy (other.gameObject);
		}
	}
}
