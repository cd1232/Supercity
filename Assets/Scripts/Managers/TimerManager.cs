using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TimerManager : MonoBehaviour
{
    public GameObject DayCompletedCanvas;
    public GameObject GameCompletedCanvas;
    public HQManager[] AllHQs;
    public CitySatisfactionManager CitySatisfaction;
    public TileManager tileManager;
    public IssueManager IssuesManager;
    public Text DayHeaderText;
    public Text AucklandersHelpedText;
    public Text MostLiveableText;
    public Text FeedbackText;
    public Text GameCompletedAucklanersHelpedText;
    public Text GameCompletedMostLiveableText;
    public Text HighScoreHeading;
    public Text HighScoreNumber;

    public GameObject TutorialIssuePanel;
    public GameObject TutorialIconPanel;
    public GameObject TutorialEndPanel;

    public GameObject Stars;
    public GameObject Clouds;

    public int CurrentDay;
    public float ProblemsPerPop = 150000;
    public float OldNumProblems = 9;

    public MoneyManager Money;

    public Material lights;

    Color Navy = new Color32(38, 59, 88, 255);

    public GameObject EffectsObject;

    public bool bTutorial = true;

    public Text TimerText;
    public Text DayText;
    public Text PopulationText;
   
    System.DateTime time;
    float timeElapsed = 0.0f;
    bool bHasDayFinished = false;
    int MinutesToAdd = 1;
    float iPopulation = 1000000;
    Color lerpColor;
    bool DayTime = true;
    public float TimeSinceChangeStart = 0.0f;
    private float duration = 3; // duration in seconds
    private float t = 0;
    Color DayTimeColor = new Color32(146, 146, 146, 255);
    Color NightTimeColor = new Color32(75, 75, 75, 255);
    SpriteRenderer[] Allstars;
    SpriteRenderer[] Allclouds;
    

    // Use this for initialization
    void Awake () {
        time = System.DateTime.Parse("2005-05-05 8:00 AM");
        TimerText.text = time.ToShortTimeString();
        CurrentDay = 1;

        DayText.text = "Day " + CurrentDay.ToString();
        MinutesToAdd = 1;

        PopulationText.text = iPopulation.ToString();
        QualitySettings.antiAliasing = 8;
        Time.timeScale = 0;

        Allstars = Stars.GetComponentsInChildren<SpriteRenderer>();
        Allclouds = Clouds.GetComponentsInChildren<SpriteRenderer>();
    }

    private void Start()
    {
       foreach(MeshRenderer rend in tileManager.m_EmissiveTiles)
        {
            rend.enabled = false;
        }

        IssuesManager.Daytime = true;
    }
    
    void Update()
    {
        if (!bTutorial)
        { 
            timeElapsed += Time.deltaTime;

            if (timeElapsed >= 2.0f)
            {
                timeElapsed = 0.0f;
                time = time.AddMinutes(MinutesToAdd * 15);
                TimerText.text = time.ToShortTimeString();

                if (time.ToShortTimeString().Equals("6:00 a.m."))
                {
                    DOTween.To(() => RenderSettings.ambientLight, x => RenderSettings.ambientLight = x,
                        new Color(0.8014706f, 0.5852523f, 0.5181363f, 1.0f), 1);
                }
                else if (time.ToShortTimeString().Equals("8:00 a.m."))
                {
                    DOTween.To(() => RenderSettings.ambientLight, x => RenderSettings.ambientLight = x,
                        new Color(1.0f, 1.0f, 1.0f, 1.0f), 1);

                    foreach (MeshRenderer rend in tileManager.m_EmissiveTiles)
                    {
                        rend.enabled = false;
                    }
                }
                else if (time.ToShortTimeString().Equals("6:00 p.m."))
                {
                    DOTween.To(() => RenderSettings.ambientLight, x => RenderSettings.ambientLight = x,
                         new Color(0.7352941f, 0.4541522f, 0.5181363f, 1.0f), 1);                
                }
                else if (time.ToShortTimeString().Equals("8:00 p.m."))
                {
                    DOTween.To(() => RenderSettings.ambientLight, x => RenderSettings.ambientLight = x,
                        new Color(0.075f, 0.11f, 0.242f, 1.0f), 1);
                    foreach (MeshRenderer rend in tileManager.m_EmissiveTiles)
                    {
                        rend.enabled = true;
                    }
                }

                if (time.ToShortTimeString().Equals("7:00 p.m."))
                {
                    // NIGHTTIME
                    MinutesToAdd = 2;
                    Camera.main.GetComponents<AudioSource>()[0].DOFade(0.0f, 1.0f);
                    Camera.main.GetComponents<AudioSource>()[1].Play();
                    Camera.main.GetComponents<AudioSource>()[1].DOFade(0.7f, 1.0f);

                    GetComponent<AudioSource>().Play();
                }
                else if (time.ToShortTimeString().Equals("7:00 a.m."))
                {
                    // DAYTIME
                    MinutesToAdd = 1;
                    Camera.main.GetComponents<AudioSource>()[0].DOFade(1.0f, 1.0f);
                    Camera.main.GetComponents<AudioSource>()[1].DOFade(0.0f, 1.0f);
                }
            }

            if (time.ToShortTimeString().Equals("7:00 p.m."))
            {
                DayTime = false;
                t = 0;
            }
            else if (time.ToShortTimeString().Equals("8:00 a.m."))
            {
                DayTime = true;
                t = 0;
            }

            if (!DayTime)
            {
                IssuesManager.Daytime = false;
                foreach (SpriteRenderer OneCloud in Allclouds)
                {
                    OneCloud.DOFade(0.0f, 0.2f);
                }

                foreach (SpriteRenderer OneStar in Allstars)
                {
                    OneStar.DOFade(1.0f, 2.0f);
                }


                if (t < 1)
                { // while t below the end limit...
                  // increment it at the desired rate every update:
                    t += Time.deltaTime / duration;
                }

                lerpColor = Color.Lerp(DayTimeColor, NightTimeColor, t);

                RenderSettings.skybox.SetColor("_Tint", lerpColor);
            }
            else
            {
                IssuesManager.Daytime = true;
                foreach (SpriteRenderer OneCloud in Allclouds)
                {
                    OneCloud.DOFade(1.0f, 2.0f);
                }

                foreach (SpriteRenderer OneStar in Allstars)
                {
                    OneStar.DOFade(0.0f, 0.2f);
                }

                if (t < 1)
                { // while t below the end limit...
                  // increment it at the desired rate every update:
                    t += Time.deltaTime / duration;
                }

                lerpColor = Color.Lerp(NightTimeColor, DayTimeColor, t);

                RenderSettings.skybox.SetColor("_Tint", lerpColor);
            }

            if (!bHasDayFinished && time.ToShortTimeString().Equals("12:00 AM") || time.ToShortTimeString().Equals("12:15AM"))
            {
                bHasDayFinished = true;

                // Add population
                iPopulation *= 1.02f;
                PopulationText.text = iPopulation.ToString();

                // Add currency
                Money.AddMoney(200.0f);

                // close all issues
                IssuesManager.ResetIssues();
                tileManager.ResetRubbish();

                RubbishTruckController rubbishTruck = FindObjectOfType<RubbishTruckController>();
                if (rubbishTruck)
                {
                    Destroy(rubbishTruck.gameObject);
                }

                // reset contractors
                for (int i = 0; i < AllHQs.Length; ++i)
                {
                    AllHQs[i].RemoveContractors();
                }

                Time.timeScale = 0;
                Transform[] gos = EffectsObject.GetComponentsInChildren<Transform>();
                for (int i = 1; i < gos.Length; ++i)
                {
                    Destroy(gos[i].gameObject);
                }
                DayCompletedCanvas.SetActive(true);

                int iJobsCompletedDay = 0;
                for (int i = 0; i < 4; ++i)
                {
                    iJobsCompletedDay += AllHQs[i].JobsCompletedForDay;
                    AllHQs[i].JobsCompletedForDay = 0;
                }
                AucklandersHelpedText.text = iJobsCompletedDay.ToString();
                MostLiveableText.text = CitySatisfaction.GetCitySatisfaction().ToString() + "%";
                DayHeaderText.text = "Congratulations! You Finished Day " + CurrentDay.ToString();

                float citySatisfaction = CitySatisfaction.GetCitySatisfaction();
                SaveLoadHighScore.Save(citySatisfaction);
                FeedbackText.text = "";
                if (CurrentDay == 1)
                {
                    if (citySatisfaction < 10.0f)
                    {
                        FeedbackText.text = "Rough first day huh? Better luck tomorrow, you can do this!";
                    }
                    else if (citySatisfaction < 25.0f)
                    {
                        FeedbackText.text = "Not bad for a first day!";
                    }
                    else if (citySatisfaction < 50.0f)
                    {
                        FeedbackText.text = "Good job on increasing Auckland's Liveability so quickly!";
                    }
                    else if (citySatisfaction < 75.0f)
                    {
                        FeedbackText.text = "Well on the way to being the most Liveable City! First day on the job, too!";
                    }
                }
                else if (CurrentDay < 5)
                {
                    if (citySatisfaction < 10.0f)
                    {
                        FeedbackText.text = "Hm, Auckland doesn't seem very happy with how you're managing this.";
                    }
                    else if (citySatisfaction < 25.0f)
                    {
                        FeedbackText.text = "Not too bad, but definitely not the most liveable city in the world...";
                    }
                    else if (citySatisfaction < 50.0f)
                    {
                        FeedbackText.text = "Aucklanders are happy - but they could be happier! Can you increase the rating tomorrow?";
                    }
                    else if (citySatisfaction < 75.0f)
                    {
                        FeedbackText.text = "Well on the way to being the most Liveable City!";
                    }
                    else if (citySatisfaction < 100.0f)
                    {
                        FeedbackText.text = "Aucklanders are happier than ever - nearly the most Liveable City in the World!";
                    }
                    else
                    {
                        FeedbackText.text = "Wow! You've helped make Auckland the World's Most Liveable City! ";
                    }
                }
                else
                {
                    if (citySatisfaction < 10.0f)
                    {
                        FeedbackText.text = "You survived 5 days - just!";
                    }
                    else if (citySatisfaction < 25.0f)
                    {
                        FeedbackText.text = "You survived 5 days, but Aucklanders are pretty disgruntled...";
                    }
                    else if (citySatisfaction < 50.0f)
                    {
                        FeedbackText.text = "Aucklanders are happy - but they could be happier!";
                    }
                    else if (citySatisfaction < 75.0f)
                    {
                        FeedbackText.text = "Well on the way to being the most Liveable City!";
                    }
                    else if (citySatisfaction < 100.0f)
                    {
                        FeedbackText.text = "Aucklanders are happier than ever - nearly the most Liveable City in the World!";
                    }
                    else
                    {
                        FeedbackText.text = "Wow! You've helped make Auckland the World's Most Liveable City! ";
                    }
                }

                CurrentDay++;
                DayText.text = "Day " + CurrentDay.ToString();

                int newNumProblems = (int)(iPopulation / ProblemsPerPop + 0.95f * OldNumProblems);
                OldNumProblems = newNumProblems;
                IssuesManager.StartNewDay(newNumProblems, 23);
            }
        }

    }

    public void RestartTime()
    {
        time = time.AddMinutes(30.0);
        TimerText.text = time.ToShortTimeString();
        Time.timeScale = 1;
        DayCompletedCanvas.SetActive(false);
        bHasDayFinished = false;
    }

    public void StartTime()
    {
        Time.timeScale = 1;
    }

    public System.DateTime GetTime()
    {
        return time;
    }

    public string GetTimeAsString()
    {
        return time.ToShortTimeString();
    }

    public void Tutorial()
    {
        bTutorial = true;
        Time.timeScale = 1;
        TutorialLevel();
    }

    void TutorialLevel()
    {
        TutorialIconPanel.SetActive(true);
        Vector3 pos = IssuesManager.CurrentIssues[0].transform.position;
        pos.x -= 5.0f;
        pos.y += 5.0f;
        TutorialIssuePanel.transform.position = pos;
        TutorialIssuePanel.SetActive(true);
    }

    public void TutorialEnd()
    {
        TutorialIssuePanel.SetActive(false);
        TutorialIconPanel.SetActive(false);
        TutorialEndPanel.SetActive(true);
    }

    public void FinishTutorial()
    {
        bTutorial = false;
        CitySatisfaction.SetCitySatisfaction(30.0f);
    }
}
