using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelBuilder : MonoBehaviour {

	public int tileCount = 0;
	GameObject[] roads;
	public GameObject player;
	List<GameObject> spawnedRoads;
	bool procedurallyGeneratedLevel = true;

	void Awake() {
		roads = Resources.LoadAll<GameObject> ("roads");
		spawnedRoads = new List<GameObject> ();
		GameObject level = new GameObject ();
		level.name = "level";
	}

	public void SpawnPlayer() {
		player = (GameObject)Instantiate (player, new Vector3 (0, 1, 24), Quaternion.identity);
		player.name = "Player";
		GameObject.FindObjectOfType<CameraControl> ().PlayerSpawned ();
	}

	public void BuildLevel() {
		print ("buildlevel");
		if (procedurallyGeneratedLevel) {
			int buildCount = tileCount + 24;
			for (int i = tileCount; i < buildCount; i++) {
				
				int selection = Random.Range (1, roads.Length);

				if (i == 0) {
					GameObject startTile = (GameObject)Instantiate (roads [0], new Vector3 (0, 0, 12), Quaternion.identity);
					startTile.transform.parent = GameObject.Find ("level").transform;
					i = 5;
					continue;
				} else if (i == 6) {
					selection = 1;
				}

				int rotation = Random.value > 0.5f ? 0 : 1;
				GameObject road = (GameObject)Instantiate (roads [selection], new Vector3 (0, 0, i * 2f + 12f), Quaternion.Euler (0f, 180 * rotation, 0f));
				road.transform.parent = GameObject.Find ("level").transform;
				spawnedRoads.Add (road);

				switch (selection) {
				case -1:
					i++;
					break;
				case -2:
					i += 2;
					break;
				}//change this to be the number for a double road


				tileCount = i;
			}
			tileCount++;
		}

	}

	public void PlayerMoved() {
		if (player.transform.position.z >= tileCount*2 - 20) {
			BuildLevel ();
		}
		if (player.transform.position.z % 2 == 0) {
			DynamicLoad ();
		}
	}

	void DynamicLoad() {
		foreach (GameObject road in spawnedRoads) {
			if (road.transform.position.z < player.transform.position.z - 16 || road.transform.position.z > player.transform.position.z + 36) {
				if (road.GetComponent<CarSpawning> () != null) {
					road.GetComponent<CarSpawning> ().DelteCars ();
				}
				road.SetActive (false);
			} else {
				road.SetActive (true);
			}
		}
	}
}
