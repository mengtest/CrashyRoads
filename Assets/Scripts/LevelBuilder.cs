using UnityEngine;
using System.Collections;

public class LevelBuilder : MonoBehaviour {

	int loadCount = 20;
	GameObject[] roads;
	public GameObject player;

	void Awake() {
		roads = Resources.LoadAll<GameObject> ("roads");
	}

	public void BuildLevel() {
		GameObject level = new GameObject ();
		for (int i = 0; i < loadCount; i++) {
			int selection = Mathf.RoundToInt (Random.value * roads.Length) % roads.Length;
			Instantiate (roads [selection], new Vector3 (0, 0, i+15f), Quaternion.identity);
			switch (selection) {
			case -1:
				i++;
				break;
			case -2:
				i += 2;
				break;
			}//change this to be the number for a double road 
		}
		Instantiate (player, new Vector3 (0, 1, 20), Quaternion.identity);
		GameObject.FindObjectOfType<CameraControl> ().PlayerSpawned ();
	}

}
