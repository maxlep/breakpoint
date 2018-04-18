using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

	public float speed = 1f;
	public int numBreakpoints = 1;

	Rigidbody rb;
	Camera cam;

	private LinkedList<Hackable> breakpoints;

	void Start () {
		rb = GetComponent<Rigidbody>();
		cam = Camera.main;
		breakpoints = new LinkedList<Hackable>();
	}
	
	void Update () {
		Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
		float diff = speed - Vector3.Project(rb.velocity, dir).magnitude;
		if (diff < 0) diff = 0;
		Vector3 friction = rb.velocity * -0.5f;
		rb.AddForce(dir * diff * 40 + friction);
		if (rb.velocity.magnitude > 0) {
			//transform.rotation = Quaternion.LookRotation(Vector3.Scale(rb.velocity.normalized, new Vector3(1, 0, 1)));
		}

		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
			if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit)) {
				Debug.Log("Clicked");
				Hackable target = hit.transform.GetComponent<Hackable>();
				if (target != null) {
					ApplyBreakpoint(target);
				}
			}
		}
		if (rb.velocity.y < 0) rb.AddForce(Physics.gravity);
	}

	void ApplyBreakpoint(Hackable target) {
		if (breakpoints.Contains(target)) {
			target.Unhack();
			breakpoints.Remove(target);
		} else {
			if (breakpoints.Count == numBreakpoints) {
				breakpoints.First.Value.Unhack();
				breakpoints.RemoveFirst();
			}
			breakpoints.AddLast(target);
			target.Hack();
		}
	}
}
