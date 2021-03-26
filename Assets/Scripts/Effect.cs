using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour {

	float spawnTimer = 0f;

	void Start (){
		gameObject.SetActive (true);
	}

	// Update is called once per frame
	void Update () {
		spawnTimer += Time.deltaTime;
		if (spawnTimer >= 0.5f) {
			Destroy (gameObject);
		}
	}
}
