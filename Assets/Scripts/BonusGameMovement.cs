using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusGameMovement : MonoBehaviour {

	public GameObject driblePoint;
	public GameObject winUI;
	public Text time;
	public Text winnerText;

	GameObject ball;
	Rigidbody rb;
	float spd = 1.5f;
	float timer = 60f;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}

	void Update(){
		timer -= Time.deltaTime;
		time.text = "Time : " + (int)timer;
		if (timer <= 1) {
			Time.timeScale = Time.timeScale == 1f ? 0f : 1f;
			winUI.SetActive(true);
			winnerText.text = "Red Team Win!";
		}
		if (ball != null) {
			ball.GetComponent<Ball> ().someoneGotMe(driblePoint);
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
			rb.transform.Translate (Time.deltaTime * spd, 0, 0);
		}else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
			rb.transform.Translate (Time.deltaTime * -spd, 0, 0);
		}
		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
			rb.transform.Translate (0, 0, Time.deltaTime * spd);
		}else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
			rb.transform.Translate (0, 0, Time.deltaTime * -spd);
		}

		//player 430-438
		//enemy 440-448
		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch (0);
			Vector3 touchPoint = Camera.main.ScreenToWorldPoint (touch.position);
			if (touchPoint.z < 440) {
				rb.transform.Translate (0, 0, Time.deltaTime * spd);
			} else if (touchPoint.z > 440) {
				rb.transform.Translate (0, 0, Time.deltaTime * -spd);
			}
			if (touchPoint.x > 820) {
				rb.transform.Translate (Time.deltaTime * spd, 0, 0);
			}else if (touchPoint.x < 820) {
				rb.transform.Translate (Time.deltaTime * -spd, 0, 0);
			}
		}
	}

	public void pauseToogle(){
		Time.timeScale = Time.timeScale == 1f ? 0f : 1f;
	}

	public void toogleObj (GameObject obj){
		if (!obj.activeSelf) {
			obj.SetActive(true);
		} else {
			obj.SetActive(false);
		}
	}

	void OnCollisionEnter (Collision col){
		if(col.gameObject.tag == "Ball"){
			ball = col.gameObject;
		}
	}
}
