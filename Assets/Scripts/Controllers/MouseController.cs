using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour {

    public static MouseController instance;

    public static MouseController GetInstance()
    {
        return instance;
    }

    public bool m_bIsSelectingHQ = false;
    public IssueManager issueManager;
    HQManager SelectedHQ;
    MoveTo SelectedContractor;
    IssueController SelectedIssue;
    public IconManager iconManager;

    private void Start()
    {
        instance = this;
    }

    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            if (hit.collider != null)
            {
                if (hit.collider.transform.CompareTag("ContractorHQ"))
                {
                }
                else if (hit.collider.transform.CompareTag("BuildingWithIssue"))
                {
                    if (SelectedHQ)
                    {
                        // in tutorial set time back
                        if (issueManager.timerManager.bTutorial)
                        {
                            issueManager.timerManager.StartTime();
                        }
                        SelectedHQ.SpawnContractor(hit.collider.transform.position, null);
                        SelectedHQ.SetSelected(false);
                        SelectedHQ = null;
                    }
                    else
                    {
                        if (SelectedIssue)
                        {
                            SelectedIssue.SetSelected(false);
                        }

                        SetSelectedIssue(hit.collider.GetComponent<TileController>().m_Issue);
                    }
                }
                else if (hit.collider.transform.CompareTag("Issue"))
                {
                    IssueController issueClicked = hit.collider.GetComponent<IssueController>();
                    Vector3 moveToLocation = issueClicked.myTile.transform.position;

                    NavMeshHit myNavHit;
                    if (NavMesh.SamplePosition(moveToLocation, out myNavHit, 100.0f, -1))
                    {
                        moveToLocation = myNavHit.position;
                    }
                    else
                    {
                        Debug.Log("Contractor did not find nav mesh point near destination");
                    }

                    if (SelectedContractor && SelectedContractor.bIsReturning)
                    {
                        SelectedContractor.SetGoal(issueClicked, moveToLocation);
                        SelectedContractor.MyContractorUI.GetComponentInChildren<Image>().color = Color.white;
                        SelectedContractor = null;
                    }
                    if (SelectedHQ)
                    {
                        // in tutorial set time back
                        if (issueManager.timerManager.bTutorial)
                        {
                            issueManager.timerManager.StartTime();
                        }
                        SelectedHQ.SpawnContractor(moveToLocation, issueClicked);
                        SelectedHQ.SetSelected(false);
                        iconManager.SetAllIconsDeselected();
                        SelectedHQ = null;
                    }
                    else
                    {
                        if (SelectedIssue)
                        {
                            SelectedIssue.SetSelected(false);
                        }
                        SetSelectedIssue(issueClicked);
                    }
                }
               else
                {
                    issueManager.CloseAllIssues();
                    if (SelectedHQ)
                    {
                        SelectedHQ.SetSelected(false);
                        SelectedHQ = null;
                    }
                    if (SelectedContractor)
                    {
                        SelectedContractor.MyContractorUI.GetComponentInChildren<Image>().color = Color.white;
                        SelectedContractor = null;
                    }
                    if (SelectedIssue)
                    {
                        SelectedIssue.SetSelected(false);
                        SelectedIssue = null;
                    }
                }         
            }
            else if (!EventSystem.current.currentSelectedGameObject)
            {
                issueManager.CloseAllIssues();
                if (SelectedHQ)
                {
                    SelectedHQ.SetSelected(false);
                    SelectedHQ = null;
                }
                if (SelectedContractor)
                {
                    SelectedContractor.MyContractorUI.GetComponentInChildren<Image>().color = Color.white;
                    SelectedContractor = null;
                }
                if (SelectedIssue)
                {
                    SelectedIssue.SetSelected(false);
                    SelectedIssue = null;
                }
            }
        }
    }

    public void SetSelectedHQ(HQManager _hq)
    {
        if (SelectedContractor)
        {
            SelectedContractor.MyContractorUI.GetComponentInChildren<Image>().color = Color.white;
            SelectedContractor = null;
        }

        if (SelectedHQ)
        {
            SelectedHQ.SetSelected(false);
        }       

        if (SelectedIssue)
        {
            _hq.SpawnContractor(SelectedIssue.myTile.transform.position, SelectedIssue);
            m_bIsSelectingHQ = false;
            SelectedIssue.SetSelected(false);
            SelectedIssue = null;
        }
        else
        {
            SelectedHQ = _hq;
            _hq.SetSelected(true);
        }
    }

    void SetSelectedIssue(IssueController issue)
    {
        SelectedIssue = issue;
        issue.SetSelected(true);
    }

    void SetSelectedContractor(MoveTo _contractor)
    {
        SelectedContractor = _contractor;
        SelectedContractor.MyContractorUI.GetComponentInChildren<Image>().color = Color.magenta;
    }

    public void SelectContractor(MoveTo _contractor)
    {
        _contractor.MyContractorUI.GetComponentInChildren<Image>().color = Color.magenta;
        SelectedContractor = _contractor;
    }    
}
