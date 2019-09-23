using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ZONE { CENTER, WEST, EAST, NORTH, SOUTH };
public enum TYPE { SUBURB, CITY, GRASS };

public class TileController : MonoBehaviour {


    public bool m_bHasIssue = false;
    public IssueController m_Issue;
    public ZONE m_Zone;
    public TYPE m_Type;
    public int m_PublicTransportLevel = 0;
    public bool m_bRubbishTruckSent = false;

    public ParticleSystem PlusParticleObject;
    public GameObject EffectsObject;
    private GameObject PlusParticle;
    public ParticleSystem SmokeParticle;
    private GameObject SmokeParticleObject;


    private IssueManager m_IssueManager;
    private Color m_StartColor;
    private Color m_Color;

    private bool bSmokeOn = false;
   

	// Use this for initialization
	void Start () {


        m_StartColor = GetComponent<MeshRenderer>().material.color;
        m_IssueManager = FindObjectOfType<IssueManager>();
    }
	
	// Update is called once per frame
	void Update () {	
        if (m_bRubbishTruckSent && !PlusParticle && !CameraController.GetInstance().GetIsRotating())
        {
            PlusParticle = Instantiate(PlusParticleObject.gameObject, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.Euler(0.0f, 0.0f, 0.0f), EffectsObject.transform);
            PlusParticle.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            StartCoroutine(DestroyObject());
        }

        if (m_Issue && !bSmokeOn)
        {
            if (m_Issue.bIsBeingWorked)
            {
                bSmokeOn = true;
                SmokeParticleObject = Instantiate(SmokeParticle.gameObject, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity, transform);
            }
           
        }

        if (!m_Issue)
        {
            if (SmokeParticleObject)
            {
                Destroy(SmokeParticleObject);
            }
        }

	}

    public void SetHasIssue(IssueController _issue)
    {
        m_Issue = _issue;
        m_bHasIssue = true;
        if (m_bRubbishTruckSent) {
            m_Issue.TimeTillChangeStart *= 1.10f;
        }
        m_Color = GetComponent<MeshRenderer>().material.color = Color.blue;
    }

    public void SetIssueCompleted()
    {
        m_Issue = null;
        m_bHasIssue = false;
        transform.tag = "Untagged";
        GetComponent<MeshRenderer>().material.color =  m_StartColor;
    }

    IEnumerator SpawnPlus()
    {
        yield return new WaitForSeconds(1.0f);
        PlusParticle = Instantiate(PlusParticleObject.gameObject, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.Euler(0.0f, 0.0f, 0.0f), EffectsObject.transform);
        StartCoroutine(DestroyObject());

    }

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(2);
        //Destroy(_object);
        Destroy(PlusParticle);
    }

    IEnumerator DestroyObjectRotate()
    {
        yield return new WaitForSeconds(0.2f);
        //Destroy(_object);
        Destroy(PlusParticle);
    }
}
