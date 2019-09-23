using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using DG.Tweening;

public class MoveTo : MonoBehaviour {

    public HQManager MyHQ;
    public CitySatisfactionManager SatisfactionManager;
    public GameObject HappinessObject;
    public ParticleSystem PlusParticleObject;
    public GameObject EffectsObject;
    public IssueType ContractorType;
    public GameObject ContractorUIPrefab;
    public Canvas ContractorUICanvas;
    public GameObject MyContractorUI;
    public bool bIsReturning = false;
    public int TimeToCompleteJob = 10;

    private NavMeshAgent agent;
    private IssueController CurrentIssue;
    private TileController TileToMoveTo;
    private Vector3 originalSpawnPoint;
    private int JobsCompleted = 0;
    private Text StatusText;
    private Slider TimeRemainingInJobSlider;
    private GameObject Happiness;
    private GameObject PlusParticle;
    private bool bIsWorking = false;
    private bool bHasReturned = false; 

    // Use this for initialization
    void Awake () {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        NavMeshHit myNavHit;
        if (NavMesh.SamplePosition(transform.position, out myNavHit, 2.0f, -1))
        {
            originalSpawnPoint = myNavHit.position;
            transform.position = originalSpawnPoint;
        }
        else
        {
            Debug.Log("Contractor could not find point to spawn on nav mesh");
        }
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
       // agent.updateRotation = false;

        int numChildren = 0;
        foreach (Transform t in ContractorUICanvas.GetComponentsInChildren<Transform>())
        {
            if (t.parent == ContractorUICanvas.transform)
            {
                numChildren++;
            }
        }
                
        MyContractorUI = Instantiate(ContractorUIPrefab, ContractorUICanvas.transform, false);

        Vector2 temp = MyContractorUI.GetComponent<RectTransform>().anchoredPosition;
        temp.y -= numChildren * 100;
        MyContractorUI.GetComponent<RectTransform>().anchoredPosition = temp;
        MyContractorUI.GetComponent<ContractorUI>().myContractor = this;
        StatusText = MyContractorUI.GetComponentInChildren<Text>();
        TimeRemainingInJobSlider = MyContractorUI.GetComponentInChildren<Slider>();
        TimeToCompleteJob -= MyHQ.GetUpgrade();
        TimeRemainingInJobSlider.maxValue = TimeToCompleteJob;
        TimeRemainingInJobSlider.gameObject.SetActive(false);
        StatusText = MyContractorUI.GetComponentInChildren<Text>();

        StatusText.text = "Travelling to Job..";


        MeshRenderer contractorMesh = GetComponent<MeshRenderer>();
        switch (ContractorType)
        {
            case IssueType.AnimalControl:
                contractorMesh.material.color = new Color32(244, 211, 49, 255);
                break;
            case IssueType.Environment:
                contractorMesh.material.color = new Color32(184, 193, 58, 255);
                break;
            case IssueType.Infrastructure:
                contractorMesh.material.color = new Color32(150, 156, 163, 255);
                break;
            case IssueType.Regulations:
                contractorMesh.material.color = new Color32(219, 32, 31, 255);
                break;
            default:
                break;
        }
        

        
    }
	
	// Update is called once per frame
	void Update () {
        
        if (CurrentIssue)
        {
            if (CurrentIssue.bIsCompleted)
            {
                CurrentIssue = null;
                agent.SetDestination(originalSpawnPoint);
            }
        }


        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    PathEnd(); 
                }
            }
        }
    }

    void PathEnd()
    {
        if (CurrentIssue)
        {
            if (!CurrentIssue.bIsBeingWorked && ContractorType == CurrentIssue.CurrentIssueType)
            {
                bIsWorking = true;
                CurrentIssue.bIsBeingWorked = true;
                TimeRemainingInJobSlider.gameObject.SetActive(true);
                StatusText.text = "Is Working..";     
            }
            else if (CurrentIssue.bIsBeingWorked && bIsWorking)
            {
                if (TimeRemainingInJobSlider.value + Time.deltaTime >= TimeToCompleteJob)
                {
                    ShowPositiveText();
                    SatisfactionManager.JobDone();
                    CurrentIssue.SetCompleted();
                    CurrentIssue.gameObject.SetActive(false);
                    TileToMoveTo.SetIssueCompleted();
                    JobsCompleted++;
                    CurrentIssue = null;
                    TimeRemainingInJobSlider.value = 0.0f;
                    TimeRemainingInJobSlider.gameObject.SetActive(false);
                    bool hasSucceeded = agent.SetDestination(originalSpawnPoint);
                    if (!hasSucceeded)
                    {
                        //Debug.Log("Didn't succeed at setting path");
                    }
                    StatusText.text = "Is Returning..";
                    bIsWorking = false;
                    bIsReturning = true;
                }

                TimeRemainingInJobSlider.value += Time.deltaTime;
            }
            else if (!CurrentIssue.bIsCompleted && !bIsWorking)
            {
               // Debug.Log("Someone is already working on it OR ContractorType != IssueType");
                CurrentIssue.ShowNegativeText();
                SatisfactionManager.ReduceCitySatisfaction();
                agent.SetDestination(originalSpawnPoint);
                StatusText.text = "Is Returning..";
                CurrentIssue = null;
                bIsReturning = true;
            }
            else if (!bIsWorking)
            {
                //Debug.Log("The job is already completed");
                CurrentIssue = null;
                agent.SetDestination(originalSpawnPoint);
            }
        }
        else if (!bHasReturned)
        {
           // Debug.Log("I have returned OR i cant find path back");
            bHasReturned = true;
            MyHQ.JobsCompleted += JobsCompleted;
            MyHQ.JobsCompletedForDay += JobsCompleted;
            MyHQ.ReturnContractor();
            Destroy(MyContractorUI.gameObject, 1.4f);
            Destroy(gameObject, 1.5f);
        }
    }

    void ShowPositiveText()
    {
        Happiness = Instantiate(HappinessObject, new Vector3(CurrentIssue.transform.position.x, CurrentIssue.transform.position.y + 0.5f, CurrentIssue.transform.position.z), Quaternion.Euler(0.0f, 0.0f, 0.0f), EffectsObject.transform);
        PlusParticle = Instantiate(PlusParticleObject.gameObject, new Vector3(CurrentIssue.transform.position.x, CurrentIssue.transform.position.y + 0.5f, CurrentIssue.transform.position.z), Quaternion.Euler(0.0f, 0.0f, 0.0f), EffectsObject.transform);
        Happiness.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        PlusParticle.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        Happiness.transform.DOScale(new Vector3(2.0f, 2.0f), 0.5f);
        StartCoroutine(FadeOut(Happiness));
    }

    IEnumerator FadeOut(GameObject _object)
    {
        yield return new WaitForSeconds(1.5f);

        if (_object)
        {
            _object.GetComponentInChildren<Text>().DOFade(0.0f, 0.2f);
            _object.transform.DOMove(new Vector2(1.0f, 2.0f), 1.0f, false).SetRelative();
            _object.transform.DOScale(0.0f, 1.5f);
            StartCoroutine(DestroyObject(_object));
        }
    }

    IEnumerator DestroyObject(GameObject _object)
    {
        yield return new WaitForSeconds(2);
        Destroy(_object);
        Destroy(PlusParticle);
    }

    public void SetGoal(IssueController _issueClicked, Vector3 _position)
    {
        TileToMoveTo = _issueClicked.myTile;
        bool bSuccess = agent.SetDestination(_position);
        if (!bSuccess)
        {
            Debug.Log("Contractor could not set destination");
        }

        if (TileToMoveTo.m_PublicTransportLevel > 0)
        {
            agent.speed *= 1.0f + (TileToMoveTo.m_PublicTransportLevel * 0.1f);
        }

        CurrentIssue = _issueClicked;
        CurrentIssue.bIsContractorMovingTowards = true;
        bIsReturning = false;
    }
}
