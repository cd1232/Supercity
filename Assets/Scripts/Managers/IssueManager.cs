using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public struct SpawnTile
{
    public SpawnTile(GameObject _tile)
    {
        tile = _tile;
        bIsBeingUsed = false;
    }

    public GameObject tile;
    public bool bIsBeingUsed;
}

public class IssueManager : MonoBehaviour {

    //This will hold all issues
    public TileManager tileManager;
    public IssueController IssuePrefab;
    public Transform SpawnedIssuesContainer;
    public List<IssueController> CurrentIssues;
    public CitySatisfactionManager CitySatisfaction;
    public TimerManager timerManager;
    IssueController[] AllIssues;
    public GameObject EffectsObject;
    public GameObject NegativePrefab;
    public int ProblemsToSpawn = 9;
    public Text UISpeechBubble;
    public bool Daytime = true;

    public Material m_BlueMaterial;

    public struct IssueTime
    {
        public IssueTime(IssueType _type, System.DateTime _time)
        {
            type = _type;
            time = _time;
            bHasSpawned = false;
        }

        public IssueType type;
        public System.DateTime time;
        public bool bHasSpawned;
    }

    List<IssueTime> SpawningTimes = new List<IssueTime>();

    public float TimeBetweenSpawns = 10.0f;


    // Use this for initialization
    void Start()
    {
        CurrentIssues = new List<IssueController>();
        StartNewDay(9, 15);
    }

    // Update is called once per frame
    void Update () {

         foreach (IssueController issue in CurrentIssues)
         {
             issue.bDaytime = Daytime;
         }
        
        int NewIssueIndex = GetNextSpawnIndex();

        if (NewIssueIndex > -1)
        {
            if (timerManager.GetTime() >= SpawningTimes[NewIssueIndex].time)
            {
                SpawnIssue(SpawningTimes[NewIssueIndex].type);
                var spawnTime = SpawningTimes[NewIssueIndex];
                spawnTime.bHasSpawned = true;
                SpawningTimes[NewIssueIndex] = spawnTime;
            }
        }
    }

    void SpawnIssue(IssueType issueType)
    {
        TileController[] allTiles = tileManager.m_AllTiles;

        int iRandomNumber = Random.Range(0, allTiles.Length - 1);
        int iFailureCount = 0;


        while (allTiles[iRandomNumber].GetComponent<TileController>().m_bHasIssue)
        {
            iRandomNumber = Random.Range(0, allTiles.Length - 1);
            iFailureCount++;
            if (iFailureCount > 30)
                return;
        }

        allTiles[iRandomNumber].GetComponent<MeshRenderer>().material = m_BlueMaterial;

        allTiles[iRandomNumber].tag = "BuildingWithIssue";

        Vector3 issueSpawnPoint = allTiles[iRandomNumber].transform.position + new Vector3(0.0f, 1.0f, 0.0f);
        IssueController newIssue = Instantiate(IssuePrefab, issueSpawnPoint , Quaternion.Euler(0.0f, 0.0f, 0.0f), SpawnedIssuesContainer);
        CurrentIssues.Add(newIssue);
        newIssue.NegativeObject = NegativePrefab;
        newIssue.SetEffectsObject(EffectsObject);
        newIssue.SetTileIndex(iRandomNumber);
        newIssue.myTile = allTiles[iRandomNumber];
        newIssue.IssuesManager = this;
        newIssue.tileManager = tileManager;
        newIssue.SetSpeechBubble(UISpeechBubble);
        newIssue.CitySatisfaction = CitySatisfaction;
        allTiles[iRandomNumber].GetComponent<TileController>().SetHasIssue(newIssue);
        newIssue.SetIssueType(issueType);
        
    } 

    
    public void StartNewDay(int _numProblems, int _hoursInDay)
    {
        SpawningTimes.Clear();
        System.DateTime currentTime = timerManager.GetTime();
        
        int hoursInFirstDay = _hoursInDay;
        ProblemsToSpawn = _numProblems;

        float timeBetweenProblems = (float)hoursInFirstDay / (float)ProblemsToSpawn;
        int problemsPerCategory = ProblemsToSpawn / 4;
        
        Dictionary<IssueType, int> numCategorySpawned = new Dictionary<IssueType, int>();
        numCategorySpawned.Add(IssueType.AnimalControl, 0);
        numCategorySpawned.Add(IssueType.Environment, 0);
        numCategorySpawned.Add(IssueType.Infrastructure, 0);
        numCategorySpawned.Add(IssueType.Regulations, 0);

        int randomCategory = 0;
        int categoryWhileCheck = 0;
        for (int i = 0; i < (problemsPerCategory * 4); ++i)
        {
            randomCategory = Random.Range(0, 4);
            
            while (numCategorySpawned[(IssueType) randomCategory] >= problemsPerCategory)
            {
                categoryWhileCheck++;
                randomCategory++;
                if (randomCategory == 4)
                {
                    randomCategory = 0;
                }
                if (categoryWhileCheck > 4)
                {
                    Debug.Log("This shouldn't be happening");
                    break;
                }
            }
            SpawningTimes.Add(new IssueTime((IssueType)randomCategory, currentTime));
            numCategorySpawned[(IssueType)randomCategory] += 1;
            currentTime = currentTime.AddHours(timeBetweenProblems);
            categoryWhileCheck = 0;
        }

        int checkEven = problemsPerCategory * 4;
        while (checkEven < ProblemsToSpawn)
        {
            randomCategory = Random.Range(0, 4);
            SpawningTimes.Add(new IssueTime((IssueType)randomCategory, currentTime));
            numCategorySpawned[(IssueType)randomCategory] += 1;
            currentTime = currentTime.AddHours(timeBetweenProblems);
            checkEven++;
        }
        
    }

    public void CloseAllIssues()
    {
        foreach (IssueController issue in CurrentIssues)
        {
            issue.CloseSpeechBubble();
        }
    }

    public void ResetIssues()
    {
        for (var i = 0; i < CurrentIssues.Count; i++)
        {
            CurrentIssues[i].myTile.SetIssueCompleted();
            CurrentIssues[i].SetCompleted();
            Destroy(CurrentIssues[i].gameObject);
        }

        CurrentIssues.Clear();
    }

    int GetNextSpawnIndex()
    {
        int NextSpawnIndex = -1;
        for (int i = 0; i < SpawningTimes.Count; ++i)
        {
            if (!SpawningTimes[i].bHasSpawned)
            {
                NextSpawnIndex = i;
                break;
            }
        }
        return NextSpawnIndex;
    }
}
