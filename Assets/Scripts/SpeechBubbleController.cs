using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubbleController : MonoBehaviour {

    Text SpeechBubbleText;
    Image[] SpeechBubbleImage;

	// Use this for initialization
	void Start () {
        SpeechBubbleText = GetComponentInChildren<Text>();
        SpeechBubbleImage = GetComponentsInChildren<Image>();
    }
	
	// Update is called once per frame
	void Update () {
		
        if (SpeechBubbleText.text == "")
        {
            for (int i = 0; i < SpeechBubbleImage.Length; ++i)
            {
                SpeechBubbleImage[i].enabled = false;
            }
        }
        else
        {
            for (int i = 0; i < SpeechBubbleImage.Length; ++i)
            {
                SpeechBubbleImage[i].enabled = true;
            }
        }
	}

    public void SetText(Text _text)
    {
        //SpeechBubbleText.text = _text;
    }
}
