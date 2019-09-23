using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsContainManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // face the camera
        transform.rotation = Camera.main.transform.rotation;
    }
}
