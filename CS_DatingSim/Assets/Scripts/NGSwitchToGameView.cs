using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NGSwitchToGameView : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SwitchToGameView()
    {
        if (FadeScript.Fading) { return; }
        StartCoroutine(FadeScript.FadeOut(GameObject.Find("transition").GetComponent<Image>(), "Level1"));
    }
}
