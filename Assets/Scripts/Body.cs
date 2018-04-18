using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour {

	public Vector3 offsetCOG;
	public float fallMultiplier = 1;
	public float dampenStanding = 0.5f;
	public float stabilityAngle = 5;

	Rigidbody rb;

	bool onGround, feetonGround;
	int collCount, trigCount;
	
	void Start () {
		collCount = 0;
		trigCount = 0;
		rb = GetComponent<Rigidbody>();

		rb.centerOfMass += offsetCOG;
	}

	void Update() {
		if (rb.velocity.y < 0) rb.AddForce(Physics.gravity * (fallMultiplier - 1));
		if (OnGround()) {
			if (Upright() && rb.angularVelocity.magnitude < dampenStanding) {
				rb.angularVelocity = Vector3.zero;
			} else {
				Vector3 towardsUp = (transform.up - Physics.gravity.normalized).normalized;
				rb.AddTorque(-rb.angularVelocity * dampenStanding + towardsUp * dampenStanding);
			}
		}
	}

	void OnCollisionEnter(Collision collision) {
		onGround = true;
		collCount++;
	}

	void OnCollisionExit(Collision collision) {
		if (--collCount < 0) collCount = 0;
		if (collCount == 0) onGround = false;
	}

	void OnTriggerEnter(Collider other) {
		feetonGround = true;
		trigCount++;
	}

	void OnTriggerExit(Collider other) {
		if (--trigCount < 0) trigCount = 0;
		if (trigCount == 0) feetonGround = false;
	}


	public bool OnGround() {
		return onGround;
	}

	public bool Upright() {
		return feetonGround && Vector3.Angle(transform.up, -Physics.gravity) < stabilityAngle;
	}
}
