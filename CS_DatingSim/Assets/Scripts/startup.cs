using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class startup : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Image img = this.GetComponent<Image>();
        var temp = img.color;
        temp.a = 1.0f;
        img.color = temp;
        StartCoroutine(FadeScript.FadeIn(img));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
