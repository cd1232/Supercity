using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimateText : MonoBehaviour
{
    public string TextToAnimate;
    public float TimePerLetter = 0.05f;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(AnimatedText(TextToAnimate));
    }
    IEnumerator AnimatedText(string strComplete)
    {
        int i = 0;
        string str = "";
        Text textComponent = GetComponentInChildren<Text>();
        if (textComponent)
        {
            while (i < strComplete.Length)
            {
                str += strComplete[i++];
                textComponent.text = str;
                yield return new WaitForSeconds(TimePerLetter);
            }
        }

        yield return null;
    }
}