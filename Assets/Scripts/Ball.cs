using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour {

	public GameObject goalEff;
	Vector3 currentPos;
	Vector3 playerGoal;
	Vector3 EnemyGoal;
	GameManager manager;
	float ballSpd = 1.5f;
	//bool idle;
	Scene scene;

	// Use this for initialization
	void Start () {
		scene = SceneManager.GetActiveScene();
		if (scene.name != "bonusScene") {
			manager = GameObject.Find ("GameManager").GetComponent<GameManager>();
		}
		playerGoal = GameObject.FindGameObjectWithTag ("PlayerGoal").transform.position;
		EnemyGoal = GameObject.FindGameObjectWithTag ("EnemyGoal").transform.position;
		//idle = true;
	}
	
	// Update is called once per frame
	void Update () {
		currentPos = transform.position;
	}

	void FixedUpdate (){
		if (scene.name == "bonusScene") {
			GameObject player = GameObject.FindGameObjectWithTag ("Player");
			if (Vector3.Distance (EnemyGoal, currentPos) <= 1) {
				Time.timeScale = Time.timeScale == 1f ? 0f : 1f;
				player.GetComponent<BonusGameMovement>().winUI.SetActive(true);
				player.GetComponent<BonusGameMovement>().winnerText.text = "Blue Team Win!";
				Quaternion rotate = Quaternion.Euler (-90, 0, 0);
				Instantiate (goalEff, EnemyGoal, rotate);
				Destroy (gameObject);
			}
		} else if(manager != null){
			if (manager.timeLeft <=1) {
				manager.timeOut ();
			}
			if (Vector3.Distance(playerGoal,currentPos) <= 1) {
				manager.enemyRoundWin ();
				Quaternion rotate = Quaternion.Euler (-90,0,0);
				Instantiate (goalEff, playerGoal,rotate);
				Destroy (gameObject);
			}else if (Vector3.Distance(EnemyGoal,currentPos) <= 1) {
				manager.playerRoundWin ();
				Quaternion rotate = Quaternion.Euler (-90,0,0);
				Instantiate (goalEff, EnemyGoal,rotate);
				Destroy (gameObject);
			}
		}
	}

	public void someoneGotMe (GameObject soldier){
		if (Vector3.Distance(currentPos, soldier.transform.position) > 0.5f) {
			if (currentPos.x > soldier.transform.position.x){
				ballSpd = ballSpd > 0 ? ballSpd * -1 : ballSpd * 1;
				transform.Translate (Time.deltaTime * ballSpd, 0, 0);
			}else if (currentPos.x < soldier.transform.position.x){
				ballSpd = ballSpd < 0 ? ballSpd * -1 : ballSpd * 1;
				transform.Translate (Time.deltaTime * ballSpd, 0, 0);
			}

			if (currentPos.z > soldier.transform.position.z){
				ballSpd = ballSpd > 0 ? ballSpd *=-1 : ballSpd * 1;
				transform.Translate (0, 0, Time.deltaTime * ballSpd);
			}else if (currentPos.z < soldier.transform.position.z){
				ballSpd = ballSpd < 0 ? ballSpd *=-1 : ballSpd * 1;
				transform.Translate (0, 0, Time.deltaTime * ballSpd);
			}
		}
	}
}
