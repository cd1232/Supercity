﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Time.timeScale = 1;
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(Time.timeScale);
	}
}
