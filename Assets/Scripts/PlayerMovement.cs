using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	bool hasControl;
	bool isMoving;
	Vector2 movement;
	Vector3 startPos;
	Vector3 targetPos;
	float walkTime;
	float speed = 2f;
	float moveStep = 1f;

	void Start() {
		hasControl = true;
		isMoving = false;
	}

	void Update() {
		if (isMoving && hasControl) {
			targetPos = startPos + new Vector3((1f-walkTime) * moveStep * movement.x, Mathf.Sin((1f-walkTime)*Mathf.PI) * moveStep, (1f-walkTime) * moveStep * movement.y);
			walkTime -= Time.deltaTime* speed;
			transform.position = targetPos;
			if(walkTime <= 0) {
				transform.position = (Vector3)startPos + new Vector3(moveStep * movement.x, 0f, moveStep * movement.y);
				targetPos = transform.position;
				isMoving = false;
			}
		} else if(hasControl) {
			Vector2 movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
			if(movementVector.sqrMagnitude > 0) {
				Move(movementVector);
			}
		}
	}

	void Move(Vector2 mv) {
		movement = mv;
		isMoving = true;
		startPos = transform.position;
		walkTime = 1f;
	}

}
