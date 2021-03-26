using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

	AudioSource ncs1;
	// Use this for initialization
	void Start () {
		ncs1 = GetComponent<AudioSource> ();
		ncs1.Play ();
		ncs1.loop = true;
	}
}
