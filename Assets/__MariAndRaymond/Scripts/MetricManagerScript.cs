﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class MetricManagerScript : MonoBehaviour
{
    string createText = "";
    public static MetricManagerScript _metricsInstance = null;

    //public int sampleMetric1, sampleMetric2;

    void Awake()
    {
        if (_metricsInstance == null)
        {
            _metricsInstance = this;
        }
        else if (_metricsInstance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);

		string time = System.DateTime.UtcNow.ToString ();
		string dateTime = System.DateTime.Now.ToString (); //Get the time to tack on to the file name
		dateTime = dateTime.Replace ("/", "-");
		createText += "[Feast of Fools Metrics] Start Tracking: " + "- Time: " + dateTime + "\r\n";
    }

    //private void OnEnable()
    //{
	//	if (FOF_GameManager.Instance.CurrentLevel == FOF_GameManager.ELevel.tableScene) 
	//	{
	//		string time = System.DateTime.UtcNow.ToString ();
	//		string dateTime = System.DateTime.Now.ToString (); //Get the time to tack on to the file name
	//		dateTime = dateTime.Replace ("/", "-");
	//		createText += "[Feast of Fools Metrics] Start Tracking: " + "- Time: " + dateTime + "\r\n";
	//	}
    //}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnApplicationQuit();
        }
    }

    //When the game quits we'll actually write the file.
    void OnApplicationQuit()
    {
        GenerateMetricsString();
        string time = System.DateTime.UtcNow.ToString(); string dateTime = System.DateTime.Now.ToString(); //Get the time to tack on to the file name
        dateTime = dateTime.Replace("/", "-"); //Replace slashes with dashes, because Unity thinks they are directories..
        dateTime = dateTime.Replace(" ", "_");
        dateTime = dateTime.Replace(":", "-");
        string reportFile = "FeastOfFools_Metrics_" + dateTime + ".txt";
        Debug.Log(reportFile);


        FileInfo file = new System.IO.FileInfo(reportFile);
        File.WriteAllText(file.FullName, createText);
        //In Editor, this will show up in the project folder root (with Library, Assets, etc.)
        //In Standalone, this will show up in the same directory as your executable
    }

    void GenerateMetricsString()
    {
        //		createText = 
        //			"Number of times something happened 1: " + sampleMetric1 + "\n" +
        //			"Number of times something happened 2: " + sampleMetric2;

        //createText += "[Feast of Fools Metrics] Start Tracking\r\n";
    }

    public void LogTime(string WhatYouWantToCallIt)
    {
        string time = System.DateTime.UtcNow.ToString(); string dateTime = System.DateTime.Now.ToString(); //Get the time to tack on to the file name
        dateTime = dateTime.Replace("/", "-");
        createText += WhatYouWantToCallIt + ": " + "- Time: " + dateTime + "\r\n";
    }

    public void LogInt(string WhatYouWantToCallIt, int theIntToLog)
    {
        string time = System.DateTime.UtcNow.ToString(); string dateTime = System.DateTime.Now.ToString(); //Get the time to tack on to the file name
        dateTime = dateTime.Replace("/", "-");
        createText += WhatYouWantToCallIt + ": " + theIntToLog + " - Time: " + dateTime + "\r\n";
    }

    public void LogFloat(string WhatYouWantToCallIt, float theFloatToLog)
    {
        string time = System.DateTime.UtcNow.ToString(); string dateTime = System.DateTime.Now.ToString(); //Get the time to tack on to the file name
        dateTime = dateTime.Replace("/", "-");
        createText += WhatYouWantToCallIt + ": " + theFloatToLog + " - Time: " + dateTime + "\r\n";
    }

    public void LogVector3(string WhatYouWantToCallIt, Vector3 theVector3ToLog)
    {
        string time = System.DateTime.UtcNow.ToString(); string dateTime = System.DateTime.Now.ToString(); //Get the time to tack on to the file name
        dateTime = dateTime.Replace("/", "-");
        createText += WhatYouWantToCallIt + ": " + "- Vector3 (" + theVector3ToLog.x + ", " + theVector3ToLog.y + ", " + theVector3ToLog.z + ") - Time: " + dateTime + "\r\n";
    }

    //Add to your set metrics from other classes whenever you want
    // public void AddToSample1(int amtToAdd){
    // 	sampleMetric1 += amtToAdd;
    // }
}
