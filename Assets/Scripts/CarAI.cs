using UnityEngine;
using System.Collections;

public class CarAI : MonoBehaviour {

	float power;
	Rigidbody rb;
	int behaviour;
	public bool foundPlayer = false;
	int direction;
	public bool alive = true;
	float timeSinceFound = 0f;
	public float topSpeed;

	void Start () {
		direction = Mathf.Round (Random.value)==1?1:-1;
		rb = GetComponent<Rigidbody>();
		rb.mass = 1500f;
		power = rb.mass * topSpeed * 3;
		behaviour = Random.Range (1, 4);
	}

	void FixedUpdate() {
		if (alive) {
			if (foundPlayer) {
				timeSinceFound += Time.fixedDeltaTime;
				switch (behaviour) {
				case 1:
					power = -Mathf.Abs (power);
					power *= (1f - Time.fixedDeltaTime*2f);
					if (rb.velocity.magnitude <= 1f) {
						power = 0;
					}
					break;
				case 2:
					if (timeSinceFound <= 1f) {
						if (power < rb.mass * topSpeed * 1.3f) {
							power += timeSinceFound * timeSinceFound * 50000000f;
						}
					} else {
						power = 0f;
					}
					if (timeSinceFound <= 0.5f) {
						rb.AddRelativeTorque (direction * Vector3.up * topSpeed * 550f);
					}
					break;
				case 3:
					if (timeSinceFound <= 2f) {
						if (power >  rb.mass * topSpeed * -1.3f) {
							power -= timeSinceFound * timeSinceFound * 50000000f;
						}
					} else {
						power = 0f;
					}
					if (timeSinceFound <= 1f) {
						rb.AddRelativeTorque (direction * Vector3.up * topSpeed * 450f);
					}
					break;
				}
			}
			Vector3 force = transform.right * (power / (rb.velocity.magnitude + 0.5f));
			rb.AddForce (force);
		}
	}

	void SlowDown() {
		rb.freezeRotation = false;
		foundPlayer = true;
	}

	void OnCollisionEnter(Collision col) {
		if (col.impulse.magnitude > 1000f && (col.gameObject.tag != "Ground")) {
			print ("dead " + col.gameObject.name);
			alive = false;
			GameObject.FindObjectOfType<GameManager> ().KilledPeople (Random.Range (1, 6));
		}
		if (col.gameObject.tag == "Player") {
			col.gameObject.GetComponent<PlayerMovement> ().Hit ();
			SlowDown ();
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject != this.gameObject) {
			if (other.gameObject.tag == "Player") {
				SlowDown ();
			}
			if (other.gameObject.tag == "Car") {
				SlowDown ();
				behaviour = 1;
			}
		}
	}
}