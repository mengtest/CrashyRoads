using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarSpawning : MonoBehaviour {

	GameObject[] cars;
	int spawnCount = 0;
	float spawnTimer;
	public int roadType;
	List<GameObject> spawnedCars;

	void OnEnable () {
		//first we have to select 3 to 4 random positions where we can spawn cars. These positions
		//have to be at least 20 units apart, so that the cars dont accidentially detect each other
		//and start slowing down for no reason. So first we will generate a random number to choose
		//how many cars we should spawn
		roadType = Random.Range(3,8);
		cars = Resources.LoadAll<GameObject> ("Cars");
		spawnedCars = new List<GameObject> ();
		int carCount = Random.Range(2,4);

		Vector3[] positions = new Vector3[carCount];
		for (int i = 0; i < carCount; i++) {
			int xPos = (int)(Random.Range (-5, 5.1f) * 10);
			bool overlapping = false;
			for (int j = 0; j < i; j++) {
				if(Mathf.Abs(positions[j].x - xPos) < 20) {
					overlapping = true;
				}
			}
			if (overlapping) {
				i--;
				continue;
			} else {
				positions [i] = new Vector3 (xPos, 1, transform.position.z);
			}
		}
		for (int i = 0; i < carCount; i++) {
			GameObject carGO = (GameObject)Instantiate (cars [Random.Range (0, cars.Length)], positions [i], transform.rotation);
			carGO.GetComponent<CarAI> ().topSpeed = 10 * roadType;
			spawnedCars.Add (carGO);
		}
	}

	void Update() {
		spawnTimer -= Time.deltaTime;
		if (spawnTimer <= 0 && spawnCount > 0) {
			GameObject carGO = (GameObject)Instantiate (cars [Random.Range (0, cars.Length)], transform.Find("spawnLoc").position, transform.rotation);
			carGO.GetComponent<CarAI> ().topSpeed = 10 * roadType;
			spawnedCars.Add (carGO);
			spawnCount--;
			spawnTimer = (10-roadType);
		}
	}

	public void SpawnCar() {
		spawnCount++;
		if (spawnTimer <= 0f) {
			spawnTimer = (10-roadType);
		}
	}

	public void DelteCars() {
		foreach (GameObject car in spawnedCars) {
			Destroy (car);
		}
	}

}
