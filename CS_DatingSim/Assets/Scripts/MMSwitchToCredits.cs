﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MMSwitchToCredits : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SwitchScene()
    {
        if (FadeScript.Fading) { return; }
        StartCoroutine(FadeScript.FadeOut(GameObject.Find("transition").GetComponent<Image>(), "Credits"));
    }
}
