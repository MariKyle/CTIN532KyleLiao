using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditFloatUp : MonoBehaviour {

	// Use this for initialization



		void Start () {

		}

	void Update() {
		transform.Translate(Vector3.up * 17 * Time.deltaTime, Space.World);
	}

}