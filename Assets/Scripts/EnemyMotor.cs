using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyMotor : MonoBehaviour {

	public float speed = 5f;
	public float jumpHeight = 3f;
	public float jumpDistance = 3f;

	Rigidbody rb;

	string lastAction;
	bool actionComplete;
	
	void Start () {
		rb = GetComponent<Rigidbody>();
		lastAction = null;
		actionComplete = true;
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
		actionComplete = true;
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

	public bool DoWait(IEnumerator action) {
		StartCoroutine(After(action));
		while (IsBusy()) { }
		return true;
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
		float v = Mathf.Sqrt(2 * Physics.gravity.magnitude * jumpHeight);
		//float time = v / Physics.gravity.magnitude + Mathf.Sqrt(2 * jumpHeight / Physics.gravity.magnitude * fallMultiplier);
		float time = v / Physics.gravity.magnitude * 2;
		float h = jumpDistance / time;
		Vector3 force = Vector3.up * v + transform.forward * h;
		rb.AddForce(force, ForceMode.VelocityChange);

		float waitTime = 0.3f;
		while (waitTime > 0) {
			yield return null;
			waitTime -= Time.deltaTime;
		}
		//Vector3 av = transform.right * (2 * Mathf.PI / (time - 0.3f - 0.2f));
		Vector3 av = Vector3.Cross(-transform.forward, Vector3.up) * (2 * Mathf.PI / (time - 0.3f - 0.2f));
		rb.AddTorque(av, ForceMode.VelocityChange);

		waitTime = time - 0.3f - 0.2f;
		while (waitTime > 0) {
			yield return null;
			waitTime -= Time.deltaTime;
		}
		// rb.AddTorque(-av, ForceMode.VelocityChange);

		if (lastAction == "FrontFlip") actionComplete = true;
	}

	public IEnumerator BackFlip() {
		lastAction = "BackFlip";
		float v = Mathf.Sqrt(2 * Physics.gravity.magnitude * jumpHeight);
		float time = v / Physics.gravity.magnitude * 2;
		float h = jumpDistance / time;
		Vector3 force = Vector3.up * v + -transform.forward * h;
		rb.AddForce(force, ForceMode.VelocityChange);

		float waitTime = 0.3f;
		while (waitTime > 0) {
			yield return null;
			waitTime -= Time.deltaTime;
		}
		//Vector3 av = transform.right * -(2 * Mathf.PI / (time - 0.3f - 0.2f));
		Vector3 av = Vector3.Cross(transform.forward, Vector3.up) * (2 * Mathf.PI / (time - 0.3f - 0.2f));
		rb.AddTorque(av, ForceMode.VelocityChange);

		waitTime = time - 0.3f - 0.2f;
		while (waitTime > 0) {
			yield return null;
			waitTime -= Time.deltaTime;
		}

		if (lastAction == "BackFlip") actionComplete = true;
	}

	public IEnumerator Flip(Vector3 dir) {
		lastAction = "Flip";
		float v = Mathf.Sqrt(2 * Physics.gravity.magnitude * jumpHeight);
		float time = v / Physics.gravity.magnitude * 2;
		float h = jumpDistance / time;
		dir = Vector3.Scale(dir, new Vector3(1, 0, 1)).normalized;
		Vector3 force = Vector3.up * v + dir * h;
		rb.AddForce(force, ForceMode.VelocityChange);

		float waitTime = 0.3f;
		while (waitTime > 0) {
			yield return null;
			waitTime -= Time.deltaTime;
		}
		//Vector3 av = transform.right * -(2 * Mathf.PI / (time - 0.3f - 0.2f));
		Vector3 av = Vector3.Cross(dir, Vector3.up) * -(2 * Mathf.PI / (time - 0.3f - 0.2f));
		rb.AddTorque(av, ForceMode.VelocityChange);

		waitTime = time - 0.3f - 0.2f;
		while (waitTime > 0) {
			yield return null;
			waitTime -= Time.deltaTime;
		}

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
