using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {
	//The GameManager class is used to control all of the overall aspects of the game.

	int score = 0;
	int peopleKilled = 0;
	public int gameMode;
	//integers to store the current players score, kill counter and gamemode.

	bool bloom = true;
	bool motionBlur = false;
	bool WASD = true;
	//booleans to represent the 3 options that the player can choose from when selecting their graphics options.

	public void NewGame() {
		Camera.main.gameObject.GetComponent<CameraControl> ().SetTarget (new Vector3 (32f, 0f, -100f), Quaternion.identity);
		//setting the position of the camera so that it can see the game menu
	}

	public void SetName() {
		string name = GameObject.Find ("Main Menu").transform.FindChild ("NameInputField").GetComponent<InputField> ().text;
		PlayerPrefs.SetString ("Name", name);
	}
	//when the player enters their name, this function is called. This function first finds the name that the player inputted
	//using GameObject.Find to locate the input field, and getting the text that is in the input field right now.
	//Then we store the name that the player entered using PlayerPrefs, under the key "Name".

	public void ChangeBloom() {
		bloom = !bloom;
		SetGraphicsPreferences ();
	}

	public void ChangeMotionBlur() {
		motionBlur = !motionBlur;
		SetGraphicsPreferences ();
	}

	public void ChangeWASD() {
		WASD = !WASD;
		SetGraphicsPreferences ();
	}

	//These three functions are all called when the player presses the buttons responsible for activating and deactivating bloom,
	//motion blur and WASD control. When each button is pressed, it swaps the current state of said option by simply performing a
	//NOT operator on the variable. After calling any of these, SetGraphicsPreferences is called.

	void SetGraphicsPreferences() {
		string preferences = "";
		if (bloom) {
			preferences += "1";
		} else {
			preferences += "0";
		}
		if (motionBlur) {
			preferences += "1";
		} else {
			preferences += "0";
		}
		if (WASD) {
			preferences += "1";
		} else {
			preferences += "0";
		}
		PlayerPrefs.SetString ("graphicsOptions", preferences);
		//By creating a string we can represent 3 options in only one stored key. The preferences string is used to store the
		//bloom, motion blur and WASD control options in one string. In each case, if the option is enabled, the string will
		//record a 1. If not, the string records a 0. Then we can parse this string in later to handle how the camera renders
		//things, as well as change how player input is handled.
	}


	public void StartGame(int type) {//1 orthographic 3rd person 2 first person 3 first person no sound 4 first person no looking side to side
		//This function is called by pressing any of the four options from the new game menu. The integer type is then recorded
		//as the gamemode, where 1 represents 3rd person mode, 2 represents 1st person mode, 3 represents 1st person with no sound
		//and 4 represents 1st person without being able to look side to side.
		gameMode = type;
		Camera.main.gameObject.GetComponent<CameraControl> ().SetTarget (new Vector3(-6f, 8f, 10f), Quaternion.Euler(30f,30f,0f));
		//we set the target of the camera to a position and rotation so that we can look over the level appropriately.
		if(gameMode != 1) {
			Camera.main.gameObject.GetComponent<CameraControl> ().SetMatrixPerspective ();
		}
		GameObject.FindObjectOfType<LevelBuilder>().BuildLevel();
		GameObject.FindObjectOfType<LevelBuilder>().SpawnPlayer();
		//after moving the camera and defining the gamemode, we can now call the BuildLevel and SpawnPlayer functions on the
		//LevelBuilder script in the scene.
	}

	public void KilledPeople(int count) {
		//This function is called by the CarAI class. When the car crashes and dies, it sends a random integer between 1 and 5
		//to this function, which then is used to record the number of people killed. The peopleKilled variable is then incremented
		//based on this variable.
		peopleKilled += count;
		foreach (Text text in Camera.main.GetComponentsInChildren<Text>()) {
			if (text.gameObject.name == "peopleText") {
				text.text = "People Killed: " + score.ToString ();
				//here we find the 'peopleKilled label under the camera's overlay object. Then we set the text of this overlay to
				//show how many people have died.
			}
		}
	}

	public void ScoreIncrease() {
		//when the player moves, we can increase the score based on how far the player has moved. This function is called from
		//the PlayerMovement class, any time the player moves this function is called which will define a new score based on how
		//far the player has got in the level. This can be done by getting the players Z coordinate. The further into the level the
		//player has got, the bigger their z coordinate will be.
		GameObject player = GameObject.FindObjectOfType<PlayerMovement> ().gameObject;
		//First we get the player gameobject.
		if (player.transform.position.z-24 > score) {
			//since the player is spawned 24 units into the level, we subtract 24 from the players position.
			//Then we check if the player has moved further than the current score would indicate.
			score = (int)player.transform.position.z - 24;
			//if they have, set the score to the current position of the player (minus the previously mentioned 24 unit offset).
			foreach (Text text in Camera.main.GetComponentsInChildren<Text>()) {
				if (text.gameObject.name == "scoreText") {
					text.text = "Score: " + score.ToString ();
					//Here, like in the people kill counter function, we get the score label under the cam,ea's overlay object.
					//Then we set the text of this overlay to show the score of the player.
				}
			}
		}
	}

}
