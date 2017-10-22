using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLevelM : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	void Update(){
		if(Input.GetMouseButton(0))
		{
			Application.LoadLevel("Title Sequence");
		}
	}
}
