using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOF_GameManager : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        Screen.SetResolution(1920, 1080, true);
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
	}
}
