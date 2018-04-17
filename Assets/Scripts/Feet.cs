using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feet : MonoBehaviour {

	bool onGround;

	// Use this for initialization
	void Start () {

	}

	void Update() {

	}

	void OnTriggerEnter(Collider collider) {
		if (collider.tag == "Ground") onGround = true;
	}

	void OnTriggerExit(Collider collider) {
		if (collider.tag == "Ground") onGround = false;
	}

	public bool OnGround() {
		return onGround;
	}
}
