using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CitySatisfactionManager : MonoBehaviour {

    public GameObject FailedPanel;
    public TimerManager timerManager;
    public Text HighScoreHeading;
    public Text HighScoreText;

    public int JobDoneIncrease = 4;
    public int DecayRate = 1;

    Slider CitySatisfactionSlider;
    Text CitySatisfactionText;
    Image FillAreaImage;
    float TimeTillUpdate = 2.0f;
    float CitySatisfaction = 30.0f;
    Color GameYellow = new Color(254.0f / 255.0f, 198.0f / 255.0f, 4.0f / 255.0f, 1.0f);
    Color GameMurkyGrey = new Color32(61, 56, 42, 255);



    // Use this for initialization
    void Start () {
        CitySatisfactionSlider = GetComponent<Slider>();
        CitySatisfactionSlider.value = CitySatisfaction;
        CitySatisfactionText = CitySatisfactionSlider.GetComponentInChildren<Text>();
        CitySatisfactionText.text = CitySatisfaction.ToString() + "%";
        FillAreaImage = CitySatisfactionSlider.fillRect.GetComponentInChildren<Image>();

        FillAreaImage.color = Color.Lerp(GameMurkyGrey, GameYellow, CitySatisfaction / 100.0f);       
    }
	
	// Update is called once per frame
	void Update () {

        TimeTillUpdate -= Time.deltaTime;

        if (CitySatisfaction <= 0)
        {
            if (!FailedPanel.activeSelf)
            {
               
                CitySatisfaction = 0;
                CitySatisfactionSlider.value = 0;
                CitySatisfactionText.text = CitySatisfaction.ToString() + "%";
                FillAreaImage.color = Color.Lerp(GameMurkyGrey, GameYellow, CitySatisfaction / 100.0f);
                Time.timeScale = 0;
                int CurrentDay = timerManager.CurrentDay;

                FailedPanel.SetActive(true);
                float previousCurrentDay = SaveLoadHighScore.Load();
                if (CurrentDay > previousCurrentDay)
                {
                    SaveLoadHighScore.Save(CurrentDay);
                    HighScoreHeading.text = "New High Score: ";
                    HighScoreText.text = CurrentDay.ToString() + " Day(s)";
                }
                else
                {
                    HighScoreText.text = previousCurrentDay.ToString() + " Day(s)";
                }
            }

           // SaveLoadHighScore.Save(CitySatisfaction);

        }
        
	}

    void ResetTimeTillUpdate()
    {
        TimeTillUpdate = 2.0f;
    }

    public void ReduceCitySatisfaction()
    {
        if (CitySatisfaction > 0)
        {
            CitySatisfaction -= DecayRate;
            CitySatisfactionSlider.value = CitySatisfaction;
            CitySatisfactionText.text = CitySatisfaction.ToString() + "%";
            FillAreaImage.color = Color.Lerp(GameMurkyGrey, GameYellow, CitySatisfaction / 100.0f);
        }
    }

    public void JobDone()
    { 
        CitySatisfaction += JobDoneIncrease;

        if (CitySatisfaction > 100.0f)
            CitySatisfaction = 100.0f;

        CitySatisfactionSlider.value = CitySatisfaction;
        CitySatisfactionText.text = CitySatisfaction.ToString() + "%";
        FillAreaImage.color = Color.Lerp(GameMurkyGrey, GameYellow, CitySatisfaction / 100.0f);
    }

    public float GetCitySatisfaction()
    {
        return CitySatisfaction;
    }

    public void SetCitySatisfaction(float _CitySatisfaction)
    {
        CitySatisfactionSlider.value = _CitySatisfaction;
        CitySatisfactionText = CitySatisfactionSlider.GetComponentInChildren<Text>();
        CitySatisfactionText.text = _CitySatisfaction.ToString() + "%";
        FillAreaImage = CitySatisfactionSlider.fillRect.GetComponentInChildren<Image>();

        FillAreaImage.color = Color.Lerp(GameMurkyGrey, GameYellow, _CitySatisfaction / 100.0f);
    }
}
