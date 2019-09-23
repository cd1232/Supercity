using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContractorUI : MonoBehaviour {

    public MoveTo myContractor;
    public GameObject ContractorSprite;
    private Sprite[] sprites;

    // Use this for initialization
    void Start () {
        sprites = Resources.LoadAll<Sprite>("ContractorFaces");
        if (myContractor.ContractorType == 0)
        {
            ContractorSprite.GetComponent<Image>().sprite = sprites[0];
        }
        else if (myContractor.ContractorType == (IssueType) 1)
        {
            ContractorSprite.GetComponent<Image>().sprite = sprites[1];
        }
        else if (myContractor.ContractorType == (IssueType)2)
        {
            ContractorSprite.GetComponent<Image>().sprite = sprites[2];
        }
        else if (myContractor.ContractorType == (IssueType)3)
        {
            ContractorSprite.GetComponent<Image>().sprite = sprites[3];
        }

        GetComponent<Button>().onClick.AddListener(delegate { MouseController.GetInstance().SelectContractor(myContractor); });
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
