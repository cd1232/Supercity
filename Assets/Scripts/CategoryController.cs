using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CategoryController : MonoBehaviour {

    public HQManager MyHQ;
    public IconManager iconManager;
    public bool bSelected = false;

    //public IssueManager issueManager;
    //HQManager SelectedHQ;
    //MoveTo SelectedContractor;
    //IssueController SelectedIssue;


    // Use this for initialization
    void Start () {
        
        GetComponent<Button>().onClick.AddListener(delegate { IconClicked(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public void ShowPopUp()
    //{
    //    m_PopUp.SetActive(true);
    //    bPopUpOpen = true;
    //    bInPopUp = false;
    //}

    //public void InPopUp()
    //{
    //    m_PopUp.SetActive(true);
    //    bPopUpOpen = true;
    //    bInPopUp = true;
    //}

    //public void HidePopUp()
    //{
    //    m_PopUp.SetActive(false);
    //    bPopUpOpen = false;
    //    bInPopUp = false;

    //}

    public void UpgradeHQ()
    {
        Debug.Log("UPGRADE");
        MyHQ.Upgrade();
    }

    public void IconClicked()
    {
        //bPopUpOpen = !bPopUpOpen;
        //mouseController.SetSelectedHQ(MyHQ);
        //transform.DOScale(0.25f, 0.1f).OnComplete(() => { transform.DOScale(0.08f, 0.2f); });

        foreach (CategoryController c in iconManager.Icons)
        {
            c.bSelected = false;
            c.transform.DOScale(0.8f, 0.1f);
        }

        transform.DOScale(1.1f, 0.1f);
        bSelected = true;
        MouseController.GetInstance().m_bIsSelectingHQ = true;
        MouseController.GetInstance().SetSelectedHQ(MyHQ);
    }

    public void HoverEnter()
    {
        if (!bSelected)
        {
            transform.DOScale(0.9f, 0.1f);
        }
    }

    public void HoverExit()
    {
        if (!bSelected)
        {
            transform.DOScale(0.8f, 0.1f);
        }
    }

}
