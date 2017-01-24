using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	Vector3 targetPos;//3d vector to store the position that we want the camera to smoothly move towards
	Quaternion targetRot;//quaternion to store the rotation that we want the camera to smoothly rotate towards
	Matrix4x4 targetMatrix;//4x4 matrix to store the type of projection matrix we want the camera to smoothly switch to.
	//The projectionmatrix is used to switch between perspective and orthographic cameras, which we can then use to switch
	//between first and third person modes, where first person will be perspective and third person will be orthographic.
	bool playerSpawned;
	void Start() {
		targetPos = transform.position;
		targetRot = transform.rotation;
		targetMatrix = GetComponent<Camera> ().projectionMatrix;
		//setting the position and rotation to the position and rotation of the camera.
		//setting the target projection matrix of the camera to the current projection matrix of the camera.
	}


	void Update() {
		if (playerSpawned) {
			targetPos = GameObject.FindObjectOfType<PlayerMovement> ().transform.position + new Vector3 (-6, 7, -10);
		}
		transform.position = Vector3.Lerp (transform.position, targetPos, Time.deltaTime * 20f);
		transform.rotation = Quaternion.Lerp (transform.rotation, targetRot, Time.deltaTime * 20f);
		//moving smoothly towards the target position and rotation by lerping towards them from our current position.
		//by lerping between current position and target position (or rotation) we get a smooth motion between the 
		//two vectors/rotations
		GetComponent<Camera> ().projectionMatrix = Math.MatrixLerp (GetComponent<Camera> ().projectionMatrix, targetMatrix, Time.deltaTime * 20f);
		//smoothly interpolating between the camera's current projection matrix and the camera's target matrix.
	}

	public void SetTarget(Vector3 pos, Quaternion rot) {
		//function to set the target position and rotation from any other gameobject
		targetPos = pos;
		targetRot = rot;
	}

	public void SetMatrixPerspective() {
		//function to set the target matrix on the camera object to a perspective 4x4 matrix

	}

	public void SetMatrixOrtho() {
		//function to set the target matrix on the camera object to an orthographic 4x4 matrix

	}

}
