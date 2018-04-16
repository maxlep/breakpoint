using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyMotor : MonoBehaviour {

	public float speed = 5f;
	public float jumpHeight = 1f;
	public float jumpDistance = 1f;
	public float fallMultiplier = 1f;

	Rigidbody rb;

	string lastAction;
	bool actionComplete;
	
	void Start () {
		rb = GetComponent<Rigidbody>();
		lastAction = null;
		actionComplete = true;
	}

	void FixedUpdate() {
		if (rb.velocity.magnitude > speed) rb.velocity *= speed / rb.velocity.magnitude;
		if (Vector3.Dot(rb.velocity, Physics.gravity) > 0) rb.AddForce(Physics.gravity * (fallMultiplier - 1));
	}

	void AddForce_CapSpeed(Vector3 force) {
		float f = (speed - rb.velocity.magnitude) / speed;
		f = Mathf.Max(f, 0);
		rb.AddForce(f * force);
	}

	public string GetLastAction() {
		return lastAction;
	}

	public void ClearLastAction() {
		lastAction = null;
	}

	public bool IsBusy() {
		return !actionComplete;
	}

	public void DoNow(IEnumerator action) {
		// StopAllCoroutines();
		StartCoroutine(action);
	}

	public void DoAfter(IEnumerator action) {
		StartCoroutine(After(action));
	}

	public IEnumerator After(IEnumerator action) {
		while (IsBusy())
			yield return null;
		StartCoroutine(action);
	}

	public IEnumerator Queue(List<IEnumerator> list) {
		foreach (IEnumerator action in list) {
			StartCoroutine(action);
			while (IsBusy())
				yield return null;
		}
	}


	// Actions
	public IEnumerator FrontFlip() {
		lastAction = "FrontFlip";
		actionComplete = false;
		float v = rb.mass * Physics.gravity.magnitude * 35;
		float h = 60;
		Vector3 force = Vector3.up * v + transform.forward * h;
		AddForce_CapSpeed(force);

		float elapsedTime = 0;
		while (elapsedTime < 0.3f) {
			yield return null;
			elapsedTime += Time.deltaTime;
		}
		Vector3 torque = transform.right * 50;
		rb.AddTorque(torque);

		if (lastAction == "FrontFlip") actionComplete = true;

		//elapsedTime = 0;
		//while (elapsedTime < 1.25f) {
		//	yield return null;
		//	elapsedTime += Time.deltaTime;
		//}
		//StartCoroutine("BackFlip");
	}

	public IEnumerator BackFlip() {
		lastAction = "BackFlip";
		actionComplete = false;
		float v = rb.mass * Physics.gravity.magnitude * 35;
		float h = -60;
		Vector3 force = Vector3.up * v + transform.forward * h;
		AddForce_CapSpeed(force);

		float elapsedTime = 0;
		while (elapsedTime < 0.3f) {
			yield return null;
			elapsedTime += Time.deltaTime;
		}
		Vector3 torque = transform.right * -50;
		rb.AddTorque(torque);

		if (lastAction == "BackFlip") actionComplete = true;
	}

	public IEnumerator Slide() {
		lastAction = "Slide";
		actionComplete = false;
		Vector3 v = rb.velocity;
		while (true) {
			yield return null;
			rb.velocity = v;
		}
	}

	public IEnumerator Spin() {
		lastAction = "Spin";
		actionComplete = false;
		Vector3 a = rb.angularVelocity;
		while (true) {
			yield return null;
			rb.angularVelocity = a;
		}
	}

	public IEnumerator Wait(float seconds) {
		lastAction = "Wait";
		actionComplete = false;
		while (seconds > 0) {
			yield return null;
			seconds -= Time.deltaTime;
		}
		if (lastAction == "Wait") actionComplete = true;
	}
}
