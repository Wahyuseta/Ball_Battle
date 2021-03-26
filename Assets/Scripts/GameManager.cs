using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public Text time;
	public Text playerPoint;
	public Text enemyPoint;
	public Text winnerText;
	public Image enemyCost;
	public Image playerCost;
	public GameObject roundUI;
	public GameObject winUI;
	public GameObject fieldCenter;
	public GameObject player;
	public GameObject enemy;
	public GameObject rematchBtn;
	public GameObject bonusGameBtn;

	SpawnManager spawn;
	[HideInInspector]
	public float timeLeft;
	float costPerBar;
	float attackerCost;
	float deffenderCost;
	float soldierSpawnTime;
	int playerScore;
	int enemyScore;
	int match;

	/// <summary>
	/// Need to actually make the score sistem (Done)
	/// Need to reSpawn the ball after match
	/// Need to calc the energy
	/// Need to make spawnSoldier work
	/// Need to make touch input work
	/// Need to make AR
	/// Need particle Eff, sound, animation, etc
	/// </summary>

	void Start(){
		spawn = GetComponent<SpawnManager> ();
		timeLeft = 140;
		deffenderCost = 3f;
		attackerCost = 2f;
		soldierSpawnTime = 0.5f;
		costPerBar = 0.17f;
		playerScore = 0;
		enemyScore = 0;
		match = 0;
	}

	void Update(){
		//player 430-438
		//enemy 440-448
		if (Input.GetButtonDown("Fire1"))
		{
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if (mousePos.z > 430 && mousePos.z < 438) {
				if (mousePos.x > 817 && mousePos.x < 823) {
					if (spawn.playerAsAAttacker) {
						if (playerCost.fillAmount >= attackerCost * costPerBar) {
							Instantiate (player,
								new Vector3(mousePos.x, fieldCenter.transform.position.y +  0.5f, mousePos.z),
								Quaternion.identity);
							spawnAttacker (playerCost);
						}
					} else {
						if (playerCost.fillAmount >= deffenderCost * costPerBar) {
							Instantiate (player,
								new Vector3(mousePos.x, fieldCenter.transform.position.y +  0.5f, mousePos.z),
								Quaternion.identity);
							spawnDeffender (playerCost);
						}
					}
				}
			}else if (mousePos.z > 440 && mousePos.z < 448) {
				if (mousePos.x > 817 && mousePos.x < 823) {
					if (!spawn.playerAsAAttacker) {
						if (enemyCost.fillAmount >= attackerCost * costPerBar) {
							Instantiate (enemy,
								new Vector3(mousePos.x, fieldCenter.transform.position.y +  0.5f, mousePos.z), 
								Quaternion.identity);
							spawnAttacker (enemyCost);
						}
					} else {
						if (enemyCost.fillAmount >= deffenderCost * costPerBar) {
							Instantiate (enemy, 
								new Vector3(mousePos.x, fieldCenter.transform.position.y +  0.5f, mousePos.z), 
								Quaternion.identity);
							spawnDeffender (enemyCost);
						}
					}
				}

			}
		}
		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch (0);
			Vector3 touchPoint = Camera.main.ScreenToWorldPoint (touch.position);
			if (touchPoint.z > 430 && touchPoint.z < 438) {
				if (touchPoint.x > 817 && touchPoint.x < 823) {
					if (spawn.playerAsAAttacker) {
						if (playerCost.fillAmount >= attackerCost * costPerBar) {
							Instantiate (player,
								new Vector3(touchPoint.x, fieldCenter.transform.position.y + 0.5f, touchPoint.z),
								Quaternion.identity);
							spawnAttacker (playerCost);
						}
					} else {
						if (playerCost.fillAmount >= deffenderCost * costPerBar) {
							Instantiate (player,
								new Vector3(touchPoint.x, fieldCenter.transform.position.y +  0.5f, touchPoint.z),
								Quaternion.identity);
							spawnDeffender (playerCost);
						}
					}
				}
			}else if (touchPoint.z > 440 && touchPoint.z < 448) {
				if (touchPoint.x > 817 && touchPoint.x < 823) {
					if (spawn.playerAsAAttacker) {
						if (enemyCost.fillAmount >= attackerCost * costPerBar) {
							Instantiate (enemy,
								new Vector3(touchPoint.x, fieldCenter.transform.position.y +  0.5f, touchPoint.z), 
								Quaternion.identity);
							spawnAttacker (enemyCost);
						}
					} else {
						if (enemyCost.fillAmount >= deffenderCost * costPerBar) {
							Instantiate (enemy, 
								new Vector3(touchPoint.x, fieldCenter.transform.position.y +  0.5f, touchPoint.z), 
								Quaternion.identity);
							spawnDeffender (enemyCost);
						}
					}
				}

			}
		}
		//playerCost.fill
		if (playerCost != null && enemyCost != null) {

			playerCost.fillAmount += 0.001f;
			enemyCost.fillAmount += 0.001f;
		}

		if (time != null) {
			time.text = "Time : " + (int)timeLeft;
			if (timeLeft > 0) {
				timeLeft -= Time.deltaTime;
			}
		}
		if (enemyPoint != null && playerPoint != null) {
			playerPoint.text = playerScore.ToString();
			enemyPoint.text = enemyScore.ToString();
			if (match == 5) {
				winUI.SetActive (true);
				pauseToogle ();
				if (playerScore > enemyScore) {
					winnerText.text = "Blue Team Win!";
				} else if (playerScore < enemyScore) {
					winnerText.text = "Red Team Win!";
				} else {
					winnerText.text = "Draw!";
					toogleObj (rematchBtn);
					toogleObj (bonusGameBtn);
				}
			}
		}
	}

	public void spawnAttacker(Image cost){
		cost.fillAmount -= costPerBar * attackerCost;
	}

	public void spawnDeffender(Image cost){
		cost.fillAmount -= costPerBar * deffenderCost;
	}

	public void pauseToogle(){
		Time.timeScale = Time.timeScale == 1f ? 0f : 1f;
	}

	public void timeOut(){
		pauseToogle ();
		toogleObj(roundUI);
		match++;
	}

	public void enemyRoundWin(){
		pauseToogle ();
		enemyScore++;
		match++;
		if (match < 5) {
			toogleObj(roundUI);
		}
	}

	public void playerRoundWin(){
		pauseToogle ();
		playerScore++;
		match++;
		if (match < 5) {
			toogleObj(roundUI);
		}
	}

	public void nextRound (){
		GameObject[] enemy = GameObject.FindGameObjectsWithTag ("Enemy");
		GameObject[] player = GameObject.FindGameObjectsWithTag ("Player");
		for (int i = 0; i < enemy.Length; i++) {
			Destroy (enemy[i]);
		}
		for (int i = 0; i < player.Length; i++) {
			Destroy (player[i]);
		}
		GameObject eff = GameObject.FindGameObjectWithTag ("Effect");
		Destroy (eff);
		GameObject balls = GameObject.FindGameObjectWithTag ("Ball");
		Destroy (balls);
		timeLeft = 140;
		playerCost.fillAmount = 0;
		enemyCost.fillAmount = 0;
		toogleObj(roundUI);
		spawn.attackerToogle ();
		spawn.SpawnBall ();
		pauseToogle ();
	}

	public void toogleObj (GameObject obj){
		if (!obj.activeSelf) {
			obj.SetActive(true);
		} else {
			obj.SetActive(false);
		}
	}
}
