using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	Vector3 targetPos;//3d vector to store the position that we want the camera to smoothly move towards
	Quaternion targetRot;//quaternion to store the rotation that we want the camera to smoothly rotate towards
	Matrix4x4 targetMatrix;//4x4 matrix to store the type of projection matrix we want the camera to smoothly switch to.
	//The projectionmatrix is used to switch between perspective and orthographic cameras, which we can then use to switch
	//between first and third person modes, where first person will be perspective and third person will be orthographic.
	bool playerSpawned;
	public bool playerAlive = true;
	GameObject playerObject;
	//playerSpawned and playerObject are used to get the camera to track the player's gameObject. When the player is spawned
	//a function called PlayerSpawned() is called on this class, which first sets the bool playerSpawned to true, then
	//gets the actual player object from the scene, storing it in the variable playerObject.
	//playerAlive is just used to determine whether or not the player has been hit by a car.
	float speed = 20f;
	//speed is used to determine how quickly the camera should smoothly move towards the target rotation, position and matrix
	//higher speed means the camera will move more quickly towards the camera's target rotation, position and matrix.

	void Start() {
		targetPos = transform.position;
		targetRot = transform.rotation;
		targetMatrix = GetComponent<Camera> ().projectionMatrix;
		//setting the position and rotation to the position and rotation of the camera.
		//setting the target projection matrix of the camera to the current projection matrix of the camera.
	}


	void Update() {
		if (playerSpawned) {//first we check if the player is spawned already
			//this is so that we do not try to access a null object, so we avoid any NullReferenceException errors.
			targetRot = playerObject.transform.rotation;
			//setting the target rotation of this camera to the rotation of the player. This is done so that when the player
			//is dead in first person mode, the camera's rotation will follow the body of the player, to give the effect of
			//the player being hit by a car.
		}
		switch(GameObject.FindObjectOfType<GameManager>().gameMode) {
		//using a switch statement with the gamemode of the current game to determine how we should position the camera.
		case 1:
			if (playerSpawned) {
				targetPos = playerObject.transform.position - Vector3.forward * 47f - Vector3.right * 33.5f;
				targetPos.y = 38f;
				if (playerAlive) {
					targetPos.x = -33.5f;
				}
				//first checks if we have already spawned the player with if(playerSpawned). If we have, then we can set the target 
				//position of the camera to be the position of the player plus a bit of offset so that the camera stays far enough 
				//away from the player that the rest of the game can be seen
				//we also only check if the player is alive before defining the target position's x value, since when the player does
				//lose, it would be cool for the camera to follow the players body as it tumbles about.
			}
			break;
		case 2:
			targetPos = playerObject.transform.position;
			//first we set the target position of the camera to the position of the player object.
			//since we are in first person, we are meant to be looking out from the player's perspective, therefore we would have the
			//camera at the position of the player
			int case2rotation = 0;
			//we define an integer known as case2rotation to be 0. This will be used to define the target rotation of the camera after
			//the player has pressed the left and right mouse buttons to turn the camera.
			if (Input.GetMouseButton (0)) {
				case2rotation--;
			}
			//if the player has pressed the left mouse button (button 0) we can subtract one from the case2rotation integer
			if (Input.GetMouseButton (1)) {
				case2rotation++;
			}
			//if the player has pressed the right mouse button (button 1) we can add one to the case2rotation integer
			if (playerAlive) {
				targetRot = Quaternion.Euler (0, 90 * case2rotation, 0);
			}
			//now, we first check if the player is alive (since if the player wasnt alive, we would want the camera to follow the player's
			//physics motion as mentioned above). Then, we set the target rotation of the player to be a rotation about the y axis, with the
			//rotation being 90 * case2rotation. If case2rotation were -1, we would get a quaternion representing a rotatiton 90 degrees to the
			//left, so the player camera would look left if we pressed the right mouse button. If case2rotation were 1, we would get a quaternion
			//representing a rotation of 90 degrees to the right, so the player camera would look right if we pressed the right mouse button.
			//then, if the player pressed either both or neither of the mouse buttons, the rotation would be 0, so the camera would just
			//look dead ahead.
			break;
		case 3:
			targetPos = playerObject.transform.position;
			int case3rotation = 0;
			if (Input.GetMouseButton (0)) {
				case3rotation--;
			}
			if (Input.GetMouseButton (1)) {
				case3rotation++;
			}
			if (playerAlive) {
				targetRot = Quaternion.Euler (0, 90 * case3rotation, 0);
			}
			break;
			//This is the same as case 2, but we have specified a new integer as case3rotation, so that we do not have conflicting inputs with 
			//case 2.
		case 4:
			targetPos = playerObject.transform.position;
			if (playerAlive) {
				targetRot = Quaternion.identity;
			}
			//here we are just setting the cameras target position to the players position as specified before, and if the player is alive, we will
			//make it so that the camera only faces forward. This is because case 4 represents the gamemode where the player cannot look left or right,
			//the player can only look forward and rely on hearing to determine where cars are. As a result, we would set the rotation of the camera
			//to only look forward.
			break;
		}
		transform.position = Vector3.Lerp (transform.position, targetPos, Time.deltaTime * speed);
		transform.rotation = Quaternion.Lerp (transform.rotation, targetRot, Time.deltaTime * speed);
		//moving smoothly towards the target position and rotation by lerping towards them from our current position.
		//by lerping between current position and target position (or rotation) we get a smooth motion between the 
		//two vectors/rotations
		GetComponent<Camera> ().projectionMatrix = Math.MatrixLerp (GetComponent<Camera> ().projectionMatrix, targetMatrix, Time.deltaTime);
		//smoothly interpolating between the camera's current projection matrix and the camera's target matrix.
		}

	public void SetTarget(Vector3 pos, Quaternion rot) {
		//function to set the target position and rotation from any other gameobject
		targetPos = pos;
		targetRot = rot;
	}

	public void SetMatrixPerspective() {
		//function to set the target matrix on the camera object to a perspective 4x4 matrix
		targetMatrix = Matrix4x4.Perspective(90f,1.77778f,0.1f,1000f);
		//Using Matrix4x4.Perspective and some values to define a new 4x4 projection matrix for the camera.
		speed = 100f;
		//since we are using a perspective matrix, we know that the player is in first person mode.
		//Therefore, it would be wise to use a really high speed so that the camera closely follows the movement of the player. In first person mode
		//smooth camera motions are not as much of a priority, whereas accurate representation of the player's position is more important
		//as such, we set the speed variable to 100, so that the camera moves quickly to its target position and rotation.
	}

	public void PlayerSpawned() {
		//function to tell the cameracontrol class when the player has been spawned.
		//This is used later on, when in the update function we check for whether or
		//not the player has been spawned.
		playerSpawned = true;
		playerObject = GameObject.FindObjectOfType<PlayerMovement> ().gameObject;
		//setting the local variable playerObject to be the player GameObject by using FindObjectOfType.
		speed = 10f;
		gameObject.transform.Find ("Overlay").gameObject.SetActive (true);
		//since the player has been spawned, we know that the game must have started, so we can enable the 'overlay' object, which holds the
		//score and kill counter for the player to see when in game.
	}

}
