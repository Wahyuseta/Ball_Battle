using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour {

	public GameObject ball;
	public GameObject center;
	GameManager manager;
	[HideInInspector]
	public bool playerAsAAttacker = true;
	// Use this for initialization
	void Start () {
		if (center != null) {
			SpawnBall ();
		}
		Scene scene = SceneManager.GetActiveScene ();
		if (scene.name == "bonusScene") {
			GameObject[] regeneratePoint = GameObject.FindGameObjectsWithTag ("GeneratePoint");
			if (regeneratePoint != null) {
				int range = Random.Range (0, regeneratePoint.Length - 1);
				float x = regeneratePoint [range].transform.position.x;
				float y = regeneratePoint [range].transform.position.y -0.5f;
				float z = regeneratePoint [range].transform.position.z;
				Vector3 spawnPos = new Vector3 (x, y, z);
				Instantiate (ball, spawnPos, Quaternion.identity);
			}
		}
		manager = GetComponent<GameManager> ();
	}

	public void attackerToogle(){
		playerAsAAttacker = !playerAsAAttacker;
	}

	public void SpawnBall (){
		//player 430-438
		//enemy 440-448
		float z;
		if (playerAsAAttacker) {
			z = Random.Range (center.transform.position.z - 8, center.transform.position.z);
		} else {
			z = Random.Range (center.transform.position.z + 8, center.transform.position.z);
		}
		float x = Random.Range (center.transform.position.x-3, center.transform.position.x + 3);
		float y = center.transform.position.y + 0.2f;
		Vector3 spawnPos = new Vector3 (x,y,z);
		Instantiate (ball, spawnPos, Quaternion.identity);
	}
}