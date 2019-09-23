using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HQManager : MonoBehaviour {

    public CitySatisfactionManager SatisfactionManager;
    public IssueType ContractorType;
    public GameObject ContractorPrefab;
    public GameObject Happiness;
    public GameObject EffectsObject;
    public ParticleSystem PlusParticleObject;
    public int JobsCompleted;
    public int JobsCompletedForDay;
    public int MaxContractors = 2;
    public Canvas ContractorUICanvas;

    public GameObject[] Contractors;

    public MoneyManager Money;

    private int ContractorsSpawned = 0;
    private Transform PointToSpawn;
    private bool bIsSelected = false;
    private Color m_StartColor;
    private Color m_Color;
    private int iUpgrade = 0;

    // Use this for initialization
    void Start() {
        PointToSpawn = GetComponentsInChildren<Transform>()[1];
        m_StartColor = GetComponent<MeshRenderer>().material.color;
        JobsCompleted = 0;
        JobsCompletedForDay = 0;
    }

    public void SpawnContractor(Vector3 _positionToMoveTo, IssueController _issueClicked)
    {
        if (ContractorsSpawned < MaxContractors)
        {
            //TODO position of spawn point
            GameObject newContractor = Instantiate(ContractorPrefab, PointToSpawn.position, Quaternion.identity, transform.parent.parent);
            MoveTo script = newContractor.GetComponent<MoveTo>();     

            if (script)
            {
                script.MyHQ = this;
                script.ContractorUICanvas = ContractorUICanvas;
                script.SetGoal(_issueClicked, _positionToMoveTo);
                script.HappinessObject = Happiness;
                script.EffectsObject = EffectsObject;
                script.PlusParticleObject = PlusParticleObject;
                script.ContractorType = ContractorType;
                script.SatisfactionManager = SatisfactionManager;
            }
            else
            {
                Debug.Log("No MoveTo Script found on object");
            }
            ContractorsSpawned++;
            Money.SubtractMoney(20.0f);
        }
    }

    public void ReturnContractor()
    {
        ContractorsSpawned--;
    }

    public void RemoveContractors()
    {
        for (int i = 0; i < Contractors.Length; ++i)
        {
            Destroy(Contractors[i]);
            ContractorsSpawned--;
        }
    }

    public void SetSelected(bool _bIsSelected)
    {
        bIsSelected = _bIsSelected;
        if (_bIsSelected)
        {
            GetComponent<MeshRenderer>().material.color = Color.magenta;
        }
        else
        {
            GetComponent<MeshRenderer>().material.color = m_StartColor;
        }
    }

    public void Upgrade()
    {
        if (iUpgrade < 3)
            ++iUpgrade;
    }

    public int GetUpgrade()
    {
        return iUpgrade;
    }

}
