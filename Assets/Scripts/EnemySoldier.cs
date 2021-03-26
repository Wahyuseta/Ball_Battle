using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoldier : MonoBehaviour {

	Soldier ourSoldier;
	Vector3 currentPos;
	GameObject[] fence;
	GameObject[] team;

	// Use this for initialization
	void Start () {
		team = GameObject.FindGameObjectsWithTag ("Enemy");
		ourSoldier = GetComponent<Soldier> ();
		ourSoldier.originColor = Color.red;
		ourSoldier.attacker = false;
		ourSoldier.foe = "Player";
		ourSoldier.soldier = "Enemy";
		ourSoldier.goalPos = GameObject.FindGameObjectWithTag ("PlayerGoal").transform.position;
		fence = GameObject.FindGameObjectsWithTag ("PlayerFence");
	}

	void FixedUpdate(){
		currentPos = transform.position;
		if (ourSoldier.attacker) {
			for (int i = 0; i < fence.Length; i++) {
				//Destroy enemy except the one who got the ball
				if (fence[i].transform.position.z > currentPos.z) {
					if (fence[i].transform.position.z % currentPos.z <= 0.5f) {
						if (!ourSoldier.gotABall) {
							ourSoldier.sucuide ();
						}
					}
				}else if (fence[i].transform.position.z < currentPos.z) {
					if (currentPos.z % fence[i].transform.position.z <= 0.5f) {
						if (!ourSoldier.gotABall) {
							ourSoldier.sucuide ();
						}
					}
				}
			}
		}
		//Atacker script
		if (ourSoldier.attacker && !ourSoldier.standBye) {
			if (!ourSoldier.gotABall && GameObject.FindGameObjectWithTag ("Ball") != null && !ourSoldier.timeToRush) {
				// if the team didnt take the ball yet
				GameObject[] team = GameObject.FindGameObjectsWithTag ("Enemy");
				if (team.Length <= 1) {
					ourSoldier.chaseTheBall ();
				}else {
					for (int i = 0; i < team.Length; i++) {
						if (team [i].GetComponent<Soldier> ().gotABall) {
							ourSoldier.rush ();
						} else if(i == team.Length-1){
							ourSoldier.chaseTheBall ();
						}
					}
				}
			} else if (ourSoldier.gotABall) {
				// if this soldier has take the ball
				ourSoldier.goToTheGoal ();
			} else if (ourSoldier.timeToRush) {
				// if the team has already take the ball but not this soldier
				ourSoldier.goAllIn();
			}
		} else if(!ourSoldier.attacker){
			//Deffender script
			if (!ourSoldier.standBye) {
				ourSoldier.playerAproaching();
			}else
				if (ourSoldier.standBye && currentPos != ourSoldier.originPos) {
				ourSoldier.returnToOrigin ();
			}
		}
	}
}
