using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMotor))]
public class Enemy : MonoBehaviour, Hackable {

	EnemyMotor motor;
	Rigidbody rb;
	Body body;

	bool hacked, flag;

	bool dir = false;
	
	void Start () {
		motor = GetComponent<EnemyMotor>();
		rb = GetComponent<Rigidbody>();
		body = GetComponentInChildren<Body>();
		flag = false;
	}

	void Update () {
		if (hacked) {
			if (flag) {
				Debug.Log("Hack action");
				flag = false;
				switch (motor.GetLastAction()) {
				case "FrontFlip":
					motor.StartCoroutine(motor.Spin());
					break;
				case "BackFlip":
					motor.StartCoroutine(motor.Spin());
					break;
				}
			}
		} else {
			// Enemy logic
			if (!motor.IsBusy() && body.OnGround() && body.Upright()) {
				motor.DoNow(motor.Wait(0.5f));
				if (dir) {
					dir = false;
					//motor.DoAfter(motor.BackFlip());
					//motor.StartCoroutine(motor.BackFlip());
				} else {
					dir = true;
					motor.DoAfter(motor.FrontFlip());
					//motor.StartCoroutine(motor.FrontFlip());
				}
			}

			// Test jumping
			if (Input.GetKeyDown(KeyCode.F)) {
				motor.StartCoroutine(motor.FrontFlip());
			}
		}
	}

	public void Hack() {
		if (hacked) return;
		Debug.Log("Hacked");
		hacked = true;
		flag = true;
		motor.StopAllCoroutines();
	}

	public void Unhack() {
		if (!hacked) return;
		hacked = false;
		motor.StopAllCoroutines();
		motor.ClearLastAction();
	}

	public bool IsHacked() {
		return hacked;
	}
}
