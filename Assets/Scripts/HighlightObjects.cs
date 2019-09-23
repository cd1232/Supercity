using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightObjects : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit Hit;
        

        if (Physics.Raycast(transform.position, fwd, out Hit, 500))
        {
           if (Hit.collider.gameObject.GetComponent<TileController>().m_bHasIssue)
            {
                Debug.Log("Issue");
            }
        }
    }
}
