using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CantKnockOver : MonoBehaviour {

	public Vector3 offset;

	Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		rb.centerOfMass += offset;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
