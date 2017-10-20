using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class FadeVideo : MonoBehaviour {


    public bool fadeOut = false;
    public bool fadeIn = true;
    public float t = 0.0f;

    public void Start()
    {
        //fadeOut = false;
        //fadeIn = false;
    }
    public void Update()
    {
        if (fadeOut)
        {
            fadeVideo();
        }

        if (fadeIn)
        {
            fadeVideo(false);
        }
    }

    void fadeVideo(bool fadeOutVideo = true)
    {
        t += 0.5f * Time.deltaTime;

        Color color = GetComponent<Renderer>().material.color;

        if (fadeOutVideo)
            color.a = Mathf.Lerp(1, 0, t);
        else
            color.a = Mathf.Lerp(0, 1, t);


        //Debug.Log("color.a: " + color.a.ToString());

        GetComponent<Renderer>().material.SetColor("_Color", color);

        if (t >= 1)
        {
            t = 0.0f;

            if (fadeOutVideo)
                fadeOut = false;
            else
                fadeIn = false;
        }
    }

}
