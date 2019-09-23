using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum IssueType
{
    AnimalControl,
    Environment,
    Infrastructure,
    Regulations,
    NULL = -1
};

public class IssueController : MonoBehaviour {

    public string IssueText;
    public IssueType CurrentIssueType;
    public AudioSource AngryNoise;
    public CitySatisfactionManager CitySatisfaction;
    public float TimeTillChangeStart = 10.0f;
    public float TimeTillRed = 7.0f;
    public float ReduceSatisfaction = 3.0f;
    public float TimeBetweenReduceSatisfaction = 3.0f;
    public bool bIsBeingWorked = false;
    public bool bIsContractorMovingTowards = false;
    public bool bDaytime = true;

    Text UIspeechBubble;
    public int TileIndex;
    public TileManager tileManager;
    public IssueManager IssuesManager;
    public GameObject EffectsObject;
    public GameObject NegativeObject;
    public TileController myTile;

    public AudioClip PositiveSound;
    public AudioClip NegativeSound;

    public bool bIsCompleted = false;
    Canvas PointsCanvas;
    Canvas SpeechBubbleCanvas;
    Text SpeechBubbleText;
    SpriteRenderer IconSpriteRenderer;
    //MeshRenderer IconMeshRenderer;
    Color lerpedColor = Color.green;
    float TimeSinceChangeStart = 0.0f;
    float TimeSincePointsCanvasActivated = 0.0f;
    float TimeBetweenMovementPointsCanvas = 0.125f;
    bool bIsSpeechBubbleActive = false;
    bool bIsSelected = false;
    bool bHasShaked = false;
    GameObject Negative;
    Color GameYellow = new Color(254.0f / 255.0f, 198.0f / 255.0f, 4.0f / 255.0f, 0.8f);
    Color GameMurkyGrey = new Color32(221, 34, 39, 200);
    Color SelectedColor = new Color32(38, 59, 88, 200);
    Color OriginalColor; 

    string[] AnimalControlIssues = {
        "Help! There’s a wild dog loose that keeps getting into rubbish bags on our street! Please send someone to pick him up so we can get him back to his owner.",
        "Help! My dog’s gotten loose and I can’t catch him. Can I please have some assistance?",
        "My next door neighbours haven’t been home to feed their dog in a few days and I’m concerned for its safety. Could someone please come and check?",
        "Help! I’ve found a mother cat with a litter at the bottom of my garden. Can someone please come and pick them up to care for them?",
        "My neighbours pet cow has roamed into my garden and is eating my flowers! I need some help!",
        "Help! We have a wasp infestation at our school. We need someone to deal with it, fast!"
    };

    string[] EnvironmentIssues = {
        "There’s a landslide on this trail that I’m walking on. It looks pretty dangerous. Could you send someone to put up a warning?",
        "I think I’ve spotted some Kauri die-back on this trail! Could you send someone to check?",
        "Help! The facilities at this park are broken. Please send someone to fix it!",
        "I’ve noticed some toxic sludge in a stream, can someone do some tests to see if it’s unsafe?",
        "Help! Someone is doing burnouts at this bird sanctuary and I’m worried for the birds! Please send someone quick!",
        "Help, this river has flooding up across the banks and is blocking a trail! We need someone to come and put warnings up before someone gets stuck!"
    };

    string[] InfrastructureIssues = {
        "Help! There’s a massive pothole in this road.Can we have someone to clear it up quick?",
        "The road up to my child’s school doesn’t have a pavement, and it’s very dangerous.Can we have footpath here?",
        "The roadworkers on this project need some extra assistance, would you be able to send more?",
        "Help! The lights have stopped working at this intersection.We need someone to help direct traffic and fix the lights quick!",
        "I need to know if I can do some renovations on my historical house. Could you send someone to check it?"
    };
    
    string[] RegulationsIssues = {
        "Help! The next door neighbours are having a loud party and my baby can’t sleep.Can you please send someone to shut it down?",
        "I’ve noticed some rude graffiti on the side of the library, can you send someone to paint over it?",
        "I just spotted a rat in an A grade restaurant! Can you please send someone to check the grade?",
        "Hello! I need an event permit, could someone check out the site?",
        "Can I get someone to come show me where I can put up a sign around my business?"
    };

    // Use this for initialization
    void Awake () {
        Canvas[] canvases = GetComponentsInChildren<Canvas>();

        SpeechBubbleCanvas = canvases[0];
        SpeechBubbleText = SpeechBubbleCanvas.GetComponentInChildren<Text>();
        IconSpriteRenderer = GetComponent<SpriteRenderer>();
        SpeechBubbleCanvas.enabled = false;
        OriginalColor = GetComponent<SpriteRenderer>().material.color;      
    }

    private void Start()
    {
        StartCoroutine(ScaleDown());
    }
     
	void Update () {

        if (!bDaytime)
        {
            IconSpriteRenderer.material.EnableKeyword("_EMISSION");
        }
        else
        {
            IconSpriteRenderer.material.DisableKeyword("_EMISSION");
        }

        transform.rotation = Camera.main.transform.rotation;
        
        if (TimeTillChangeStart > 0.0f)
        {
            TimeTillChangeStart -= Time.deltaTime;
        }
        else
        {
            TimeSinceChangeStart += Time.deltaTime;
            
            if (!bIsBeingWorked)
            {
                lerpedColor = Color.Lerp(GameYellow, GameMurkyGrey, TimeSinceChangeStart / TimeTillRed);
                if (!bIsSelected)
                {
                    IconSpriteRenderer.material.color = lerpedColor;
                }

                if (TimeSinceChangeStart >= TimeTillRed)
                {
                    if (!bHasShaked)
                    {
                        AngryNoise.Play();
                        IconSpriteRenderer.transform.DOShakePosition(2.0f, 0.1f, 5, 90f, false, true);
                        bHasShaked = true;
                    }
                    else
                    {
                        TimeBetweenReduceSatisfaction -= Time.deltaTime;
                        if (TimeBetweenReduceSatisfaction <= 0.0f)
                        {
                            PlayNegativeSound();
                            TimeBetweenReduceSatisfaction = ReduceSatisfaction;
                            CitySatisfaction.ReduceCitySatisfaction();
                            ShowNegativeText();
                        }
                    }
                }
            }
        }
        
	}

    public void OnMouseOver()
    {
        if (!bIsSpeechBubbleActive)
        {
            OpenSpeechBubble();
        }
    }

    public void OnMouseExit()
    {
        if (bIsSpeechBubbleActive && !bIsSelected)
        {
            CloseSpeechBubble();
        }
    }
    public void SetIssueType(IssueType _IssueType)
    {
        CurrentIssueType = _IssueType;
        switch (CurrentIssueType)
        {
            case IssueType.AnimalControl:
                {
                    SpeechBubbleText.text = AnimalControlIssues[Random.Range(0, AnimalControlIssues.Length)];
                    break;
                }
            case IssueType.Environment:
                {
                    SpeechBubbleText.text = EnvironmentIssues[Random.Range(0, EnvironmentIssues.Length)];
                    break;
                }
            case IssueType.Infrastructure:
                {
                    SpeechBubbleText.text = InfrastructureIssues[Random.Range(0, InfrastructureIssues.Length)];
                    break;
                }
            default:
            case IssueType.Regulations:
                {
                    SpeechBubbleText.text = RegulationsIssues[Random.Range(0, RegulationsIssues.Length)];
                    break;
                }
        }
    }

    public void SetCompleted()
    {
        bIsCompleted = true;
        SetSpawnPointInactive();
        PlayPositiveSound();

        if (IssuesManager.timerManager.bTutorial)
        {
            IssuesManager.timerManager.TutorialEnd();
        }
    }

    public void SetSpawnPointInactive()
    {
       tileManager.m_AllTiles[TileIndex].m_bHasIssue = false;
    }

    public void SetTileIndex(int _index)
    {
        TileIndex = _index;
    }

    public void OpenSpeechBubble()
    {

        if (bIsSpeechBubbleActive)
        {
            CloseSpeechBubble();
        }
        else
        {
            bIsSpeechBubbleActive = true;
            UIspeechBubble.text = SpeechBubbleText.text;
        }       
    }

    public void CloseSpeechBubble()
    {
        UIspeechBubble.text = "";
        bIsSpeechBubbleActive = false;
    }

    public bool GetSpeechBubbleActive()
    {
        return bIsSpeechBubbleActive;
    }

    public void ShowPointsCanvas()
    {
        PointsCanvas.enabled = true;
    }

    public void SetSelected(bool _bIsSelected)
    {
        bIsSelected = _bIsSelected;
        if (_bIsSelected)
        {
            GetComponent<SpriteRenderer>().material.color = SelectedColor;
            UIspeechBubble.text = SpeechBubbleText.text;

        }
        else
        {
            GetComponent<SpriteRenderer>().material.color = OriginalColor;
            UIspeechBubble.text = "";
        }
    }

    IEnumerator FadeOut(GameObject _object)
    {
        yield return new WaitForSeconds(0.5f);

        if (_object)
        {
            _object.GetComponentInChildren<Text>().DOFade(0.0f, 0.5f);
            _object.transform.DOMove(new Vector2(1.0f, 2.0f), 1.0f, false).SetRelative();
            _object.transform.DOScale(0.0f, 1.5f);
            StartCoroutine(DestroyObject(_object));
        }
    }

    IEnumerator DestroyObject(GameObject _object)
    {
        yield return new WaitForSeconds(2);
        Destroy(_object);
    }

    IEnumerator ScaleDown()
    {
        yield return new WaitForSeconds(0.5f);
        transform.DOScale(0.25f, 0.1f).OnComplete(() => { transform.DOScale(0.08f, 0.2f); });
    }

    public void ShowNegativeText()
    {
        Negative = Instantiate(NegativeObject, new Vector3(transform.position.x + 0.5f, transform.position.y + 0.5f, transform.position.z), Quaternion.Euler(0.0f, 0.0f, 0.0f), EffectsObject.transform);
        Negative.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        Negative.transform.DOScale(new Vector3(2.0f, 2.0f), 0.5f);
        StartCoroutine(FadeOut(Negative));
    }

    public void PlayPositiveSound()
    {
        AudioSource.PlayClipAtPoint(PositiveSound, transform.position, 2.5f);
    }

    public void PlayNegativeSound()
    {
        AudioSource.PlayClipAtPoint(NegativeSound, transform.position);
    }

    public void SetEffectsObject(GameObject _effectsobject)
    {
        EffectsObject = _effectsobject;
    }

    public void SetSpeechBubble(Text _speechbubble)
    {
        UIspeechBubble = _speechbubble;
    }

    public bool GetIsBeingWorked()
    {
        return bIsBeingWorked;
    }
}
