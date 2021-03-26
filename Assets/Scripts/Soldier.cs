using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Soldier : MonoBehaviour
{
	public GameObject ballPos;
	public GameObject spawnEff;
	Rigidbody rb;
	Renderer render;
	Vector3 currentPos;

	[HideInInspector]
	public Vector3 originPos;
	[HideInInspector]
	public Color originColor;
	[HideInInspector]
	public Vector3 goalPos;
	[HideInInspector]
	public GameObject ball;

	[HideInInspector]
	public bool gotABall;
	[HideInInspector]
	public bool timeToRush;
	[HideInInspector]
	public bool standBye;
	[HideInInspector]
	public bool attacker;

	float deffReactive = 4f;
	float attackerReactive = 2.5f;
	float returnSpd = 2f;
	float chaseSpd = 1.5f;
	float carryingSpd = 0.75f;
	float deffSpd = 1f;
	float timer = 0f;

	[HideInInspector]
	public string soldier;
	[HideInInspector]
	public string foe;

	SpawnManager spawn;
	GameManager manager;

	void Start(){
		spawn = GameObject.Find ("GameManager").GetComponent<SpawnManager>();
		manager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		originPos = transform.position;
		rb = GetComponent<Rigidbody>();
		render = GetComponent<Renderer> ();
		originColor = GetComponent<Renderer> ().material.color;
		gotABall = false;
		timeToRush = false;
		standBye = false;
		Instantiate (spawnEff, transform.position,Quaternion.identity);
	}

	void Update (){
		if (gameObject.tag == "Player") {
			if (spawn.playerAsAAttacker) {
				attacker = true;
			} else {
				attacker = false;
			}
		}else if (gameObject.tag == "Enemy") {
			if (spawn.playerAsAAttacker) {
				attacker = false;
			} else {
				attacker = true;
			}
		}
		currentPos = transform.position;
		if (standBye) {
			timer += Time.deltaTime;
		}
		if (attacker && timer >= attackerReactive) {
			standByeToogle ();
			timer = 0;
		}else if (!attacker && timer >= deffReactive) {
			standByeToogle ();
			timer = 0;
		}
	}
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Attacker
	public void soldierGotTheBall(){
		//Give a rush command to all ally but this ally
		GameObject[] team = GameObject.FindGameObjectsWithTag (soldier);
		for (int i = 0; i < team.Length; i++) {
			if (!team[i].Equals(gameObject) && !team[i].GetComponent<Soldier>().standBye) {
				team [i].GetComponent<Soldier>().rush();
			}
		}
	}

	public void rush(){
		timeToRush = true;
	}

	public void chaseTheBall (){
		Vector3 theBallPos = GameObject.FindGameObjectWithTag ("Ball").transform.position;
		if (currentPos.x > theBallPos.x) {
			chaseSpd = chaseSpd > 0 ? chaseSpd * -1 : chaseSpd * 1;
			rb.transform.Translate (Time.deltaTime * chaseSpd, 0, 0);
		} else if (currentPos.x < theBallPos.x) {
			chaseSpd = chaseSpd < 0 ? chaseSpd * -1 : chaseSpd * 1;
			rb.transform.Translate (Time.deltaTime * chaseSpd, 0, 0);
		}

		if (currentPos.z > theBallPos.z) {
			chaseSpd = chaseSpd > 0 ? chaseSpd *= -1 : chaseSpd * 1;
			rb.transform.Translate (0, 0, Time.deltaTime * chaseSpd);
		} else if (currentPos.z < theBallPos.z) {
			chaseSpd = chaseSpd < 0 ? chaseSpd *= -1 : chaseSpd * 1;
			rb.transform.Translate (0, 0, Time.deltaTime * chaseSpd);
		}
	}

	public void goToTheGoal(){
		if (Vector3.Distance (currentPos, ball.transform.position) > 1 && !timeToRush) {
			chaseTheBall ();
		} else {
			ball.GetComponent<Ball>().someoneGotMe(ballPos);
			if (currentPos.x > goalPos.x){
				carryingSpd = carryingSpd > 0 ? carryingSpd * -1 : carryingSpd * 1;
				rb.transform.Translate (Time.deltaTime * carryingSpd, 0, 0);
			}else if (currentPos.x < goalPos.x){
				carryingSpd = carryingSpd < 0 ? carryingSpd * -1 : carryingSpd * 1;
				rb.transform.Translate (Time.deltaTime * carryingSpd, 0, 0);
			}

			if (currentPos.z > goalPos.z){
				carryingSpd = carryingSpd > 0 ? carryingSpd *=-1 : carryingSpd * 1;
				rb.transform.Translate (0, 0, Time.deltaTime * carryingSpd);
			}else if (currentPos.z < goalPos.z){
				carryingSpd = carryingSpd < 0 ? carryingSpd *=-1 : carryingSpd * 1;
				rb.transform.Translate (0, 0, Time.deltaTime * carryingSpd);
			}
		}

	}

	public void goAllIn (){
		if (currentPos.z > goalPos.z) {
			chaseSpd = chaseSpd > 0 ? chaseSpd *= -1 : chaseSpd * 1;
			rb.transform.Translate (0, 0, Time.deltaTime * chaseSpd);
		} else if (currentPos.z < goalPos.z) {
			chaseSpd = chaseSpd < 0 ? chaseSpd *= -1 : chaseSpd * 1;
			rb.transform.Translate (0, 0, Time.deltaTime * chaseSpd);
		}
	}

	//Meet with deffender
	public void getCaught(){
		standByeToogle ();
		GameObject[] team = GameObject.FindGameObjectsWithTag (soldier);
		float[,] theNearest = new float[team.Length,2];
		int nearest = 0;
		int temp = 0;
		int itself = 0;
		//searching the nearest ally
		for (int i = 0; i < team.Length; i++)
		{
			if (!team[i].Equals(gameObject) && !team[i].GetComponent<Soldier>().standBye)
			{
				float anyOneThere = Vector3.Distance(team[i].transform.position, currentPos);
				theNearest[i, 0] = anyOneThere;
				theNearest[i, 1] = (float)i;
			}
			else if (team[i].Equals(gameObject))
			{
				itself = i;
				theNearest[i, 0] = 1000f;
				theNearest[i, 1] = (float)i;
			}else if (!team[i].Equals(gameObject) && team[i].GetComponent<Soldier>().standBye) {
				theNearest[i, 0] = 1000f;
				theNearest[i, 1] = (float)i;
			}
			else
			{
				theNearest[i, 0] = 1000f;
				theNearest[i, 1] = (float)i;
			}
		}
		for (int i = 0; i < team.Length; i++)
		{
			if (i == 1) {	
				if (i != itself && i - 1 != itself) {
					nearest = theNearest [i, 0] < theNearest [i - 1, 0] ? i : i - 1;
					temp = nearest;
				} else if (i == itself) {
					nearest = i - 1;
					temp = nearest;
				} else if (i - 1 == itself) {
					nearest = i;
					temp = nearest;
				}
			} else if (i > 1 && i != itself && i - 1 != itself) {
				if (team [i].GetComponent<Soldier> ().standBye == false
					&& team [i - 1].GetComponent<Soldier> ().standBye == false) {
					if (theNearest [i, 0] < theNearest [i - 1, 0]) {
						nearest = theNearest [i, 0] < theNearest [nearest, 0] ? i : nearest;
						temp = nearest;
					} else if (theNearest [i, 0] > theNearest [i - 1, 0]) {
						nearest = theNearest [i - 1, 0] < theNearest [nearest, 0] ? i - 1 : nearest;
						temp = nearest;
					}
				} else if (team [i].GetComponent<Soldier> ().standBye == false
					&& team [i - 1].GetComponent<Soldier> ().standBye== true) {
					nearest = theNearest [i, 0] < theNearest [nearest, 0] ? i : nearest;
					temp = nearest;
				} else if (team [i].GetComponent<Soldier> ().standBye == true
					&& team [i - 1].GetComponent<Soldier> ().standBye == false) {
					nearest = theNearest [i - 1, 0] < theNearest [nearest, 0] ? i - 1 : nearest;
					temp = nearest;
				}
			} else if (i > 1 && i == itself) {
				if (team [i - 1].GetComponent<Soldier> ().standBye == false) {
					nearest = theNearest [i - 1, 0] < theNearest [nearest, 0] ? i - 1 : nearest;
					temp = nearest;
				}
			} else if (i > 1 && i - 1 == itself) {
				if (team [i].GetComponent<Soldier> ().standBye == false) {
					nearest = theNearest [i, 0] < theNearest [nearest, 0] ? i : nearest;
					temp = nearest;
				}
			} else {
				nearest = itself;
			}
		}
		//Pass
		team [nearest].GetComponent<Soldier>().ball = ball;
		team [nearest].GetComponent<Soldier>().gotABall = true;
		team [nearest].GetComponent<Soldier>().timeToRush = false;
		gotABall = false;
		rush ();
		ball.GetComponent<Ball>().someoneGotMe(team [nearest].GetComponent<Soldier>().ballPos);
	}
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Deffender
	public void playerAproaching(){
		GameObject[] foes = GameObject.FindGameObjectsWithTag (foe);
		for (int i = 0; i < foes.Length; i++) {
			bool itsGotTheBall = foes [i].GetComponent<Soldier> ().gotABall;
			bool itsStandBye = foes [i].GetComponent<Soldier> ().standBye;
			if (!itsStandBye && itsGotTheBall && Vector3.Distance(currentPos, foes [i].transform.position) <= 3) {
				chaseTheFoe (foes[i]);
			}
		}
	}

	public void chaseTheFoe (GameObject theFoe){
		if (currentPos.x > theFoe.transform.position.x){
			deffSpd = deffSpd > 0 ? deffSpd * -1 : deffSpd * 1;
			rb.transform.Translate (Time.deltaTime * deffSpd, 0, 0);
		}else if (currentPos.x < theFoe.transform.position.x){
			deffSpd = deffSpd < 0 ? deffSpd * -1 : deffSpd * 1;
			rb.transform.Translate (Time.deltaTime * deffSpd, 0, 0);
		}

		if (currentPos.z > theFoe.transform.position.z){
			deffSpd = deffSpd > 0 ? deffSpd *=-1 : deffSpd * 1;
			rb.transform.Translate (0, 0, Time.deltaTime * deffSpd);
		}else if (currentPos.z < theFoe.transform.position.z){
			deffSpd = deffSpd < 0 ? deffSpd *=-1 : deffSpd * 1;
			rb.transform.Translate (0, 0, Time.deltaTime * deffSpd);
		}

		if (Vector3.Distance(currentPos, theFoe.transform.position) <= 1) {
			standByeToogle ();
			theFoe.GetComponent<Soldier>().getCaught();
		}
	}

	public void returnToOrigin (){
		if (Vector3.Distance(currentPos, originPos) > 0.1f) {
			if (currentPos.x > originPos.x){
				returnSpd = returnSpd > 0 ? returnSpd * -1 : returnSpd * 1;
				rb.transform.Translate (Time.deltaTime * returnSpd, 0, 0);
			}else if (currentPos.x < originPos.x){
				returnSpd = returnSpd < 0 ? returnSpd * -1 : returnSpd * 1;
				rb.transform.Translate (Time.deltaTime * returnSpd, 0, 0);
			}

			if (currentPos.z > originPos.z){
				returnSpd = returnSpd > 0 ? returnSpd *=-1 : returnSpd * 1;
				rb.transform.Translate (0, 0, Time.deltaTime * returnSpd);
			}else if (currentPos.z < originPos.z){
				returnSpd = returnSpd < 0 ? returnSpd *=-1 : returnSpd * 1;
				rb.transform.Translate (0, 0, Time.deltaTime * returnSpd);
			}
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public void sucuide (){
		Destroy (gameObject);
	}

	public void standByeToogle(){
		standBye = !standBye;
		if (standBye) {
			render.material.color = Color.gray;
		} else {
			render.material.color = originColor;
			Instantiate (spawnEff, currentPos, Quaternion.identity);
		}
	}

	public void roleToogle(){
		attacker = !attacker;
	}

	void OnCollisionEnter (Collision col){
		if(col.gameObject.tag == "Ball" && !timeToRush && !standBye){
			ball = col.gameObject;
			soldierGotTheBall ();
			gotABall = true;
		}
	}
}

