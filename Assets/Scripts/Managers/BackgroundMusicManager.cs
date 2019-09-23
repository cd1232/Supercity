using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{

    public TimerManager timerManager;

    public AudioSource CityBackground;
    public AudioSource Crickets;

    // Use this for initialization
    void Start()
    {
        AudioSource[] audio = GetComponents<AudioSource>();
        CityBackground = audio[1];
        Crickets = audio[2];

        Crickets.volume = 0.8f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerManager.GetTimeAsString().Equals("8:00 PM"))
        {
            CityBackground.Stop();
            Crickets.Play();
            Crickets.loop = true;
        }
        else if (timerManager.GetTimeAsString().Equals("6:00 AM"))
        {
            Crickets.Stop();
            CityBackground.Play();
            CityBackground.loop = true;
        }
    }
}
