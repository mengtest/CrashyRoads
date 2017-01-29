using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	bool hasControl;
	Vector2 movement;
	Vector3 startPos;
	float movementTimer;
	float speed = 2f;
	float moveStep = 2f;

	void Start() {
		hasControl = true;
	}

	void Update() {
		if (hasControl) {
			if (movementTimer > 0) {
				Vector3 targetPos = startPos + new Vector3 ((1f - movementTimer) * moveStep * movement.x, Mathf.Sin ((1f - movementTimer) * Mathf.PI) * moveStep, (1f - movementTimer) * moveStep * movement.y);
				movementTimer -= Time.deltaTime * speed;
				transform.position = targetPos;
				if (movementTimer <= 0) {
					transform.position = (Vector3)startPos + new Vector3 (moveStep * movement.x, 0f, moveStep * movement.y);
					targetPos = transform.position;
				}
			} else {
				Vector2 movementVector = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
				if (movementVector.sqrMagnitude > 0) {
					Move (movementVector);
				}
			}
		}
	}

	void Move(Vector2 mv) {
		movement = mv;
		startPos = transform.position;
		movementTimer = 1f;
		GameObject.FindObjectOfType<LevelBuilder> ().PlayerMoved ();
		GameObject.FindObjectOfType<GameManager> ().ScoreIncrease ();
	}

	public void Hit() {
		GetComponent<Rigidbody> ().freezeRotation = false;
		hasControl = false;
		Camera.main.GetComponent<CameraControl> ().playerAlive = false;
	}

}
