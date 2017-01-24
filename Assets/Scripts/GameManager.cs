using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {


	public void NewGame() {
		Camera.main.gameObject.GetComponent<CameraControl> ().SetTarget (new Vector3 (32f, 0f, -10f), Quaternion.identity);
		//setting the position of the camera so that it can see the game menu
	}

	public void StartGame() {
		Camera.main.gameObject.GetComponent<CameraControl> ().SetTarget (new Vector3(-6f, 8f, 10f), Quaternion.Euler(30f,30f,0f));
		//moving camera to see where the game will be played. Also set the rotation of the camera to appropriately look over the level
		GameObject.FindObjectOfType<LevelBuilder>().BuildLevel();
	}


}
