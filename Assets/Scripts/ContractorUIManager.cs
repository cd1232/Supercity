using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractorUIManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float posY = 5.0f;
        foreach (ContractorUI contractorUI in GetComponentsInChildren<ContractorUI>())
        {
            Vector2 pos = contractorUI.GetComponent<RectTransform>().anchoredPosition;
            pos.y = posY;
            contractorUI.GetComponent<RectTransform>().anchoredPosition = pos;
            posY -= 110.0f;
        }
	}
}
