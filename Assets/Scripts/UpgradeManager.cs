using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpgradeManager : MonoBehaviour {

    public HQManager[] AllHQs;
    public TileManager tileManager;
    public GameObject RubbishZones;
    public GameObject TransportZones;
    public GameObject RubbishTruckPrefab;
    public GameObject train1;
    public GameObject train2;
    public Transform RubbishTruckSpawnPoint;
    public MoneyManager Money;
    public AudioClip animalUpgrade;
    public AudioClip environmentUpgrade;
    public AudioClip infrastructrueUpgrade;
    public AudioClip regulationsUpgrade;
    public AudioClip budgetSound;
    public AudioClip rubbishTruck;
    public AudioClip train;

    public Text UpgradeCostText;

    private Button[] UpgradeButtons;
    private Button[] UpgradeCategoryButtons;
    private Button[] RubbishTrucksZonesButtons;
    private Button[] TransportZonesButtons;
    private int UpgradeCost = 120;

    private Text UpgradeText;

	// Use this for initialization
	void Start () {
        UpgradeText = GetComponentInChildren<Text>();
        UpgradeButtons = GetComponentsInChildren<Button>();

        UpgradeCategoryButtons = 
            new Button[4] { UpgradeButtons[0], UpgradeButtons[1], UpgradeButtons[2], UpgradeButtons[3] };

        RubbishTrucksZonesButtons = RubbishZones.GetComponentsInChildren<Button>();
        TransportZonesButtons = TransportZones.GetComponentsInChildren<Button>();
        UpdateStars();
        UpdateStarsTransport();

        //train1.GetComponent<Train>().enabled = false;
       // train2.GetComponent<Train>().enabled = false;

        train1.SetActive(false);
        train2.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
      
    }

    public void UpdateStars()
    {
        int currentType = 0;
        foreach (Button b in UpgradeCategoryButtons)
        {

            Image[] image = b.GetComponentsInChildren<Image>();

            for (int i = 0; i < image.Length; ++i)
            {
                if (image[i] == b.GetComponent<Image>())
                    continue;

                if (AllHQs[currentType].GetUpgrade() < i)
                {
                    image[i].color = Color.clear;
                    //image[i].gameObject.SetActive(false);
                }
                else
                {
                    image[i].color = Color.white;
                    image[i].gameObject.SetActive(true);
                }
            }
            currentType++;
        }
    }

    public void PlayBudgetSound()
    {
        AudioSource.PlayClipAtPoint(budgetSound, Camera.main.transform.position, 1.0f);
    }

    public void UpgradeContractorCategory(int _category)
    {
     
        for (int i = 0; i < UpgradeButtons.Length; ++i)
        {
            if (i == _category)
            {
                UpgradeButtons[i].interactable = true;
            }
            else
            {
                UpgradeButtons[i].interactable = false;
            }

        }


        foreach (HQManager manager in AllHQs)
        {
            if (manager.ContractorType == (IssueType) _category)
            {
                TransportZones.SetActive(false);
                RubbishZones.SetActive(false);
                manager.Upgrade();
                break;
            }
        }

        if ((IssueType) _category == IssueType.AnimalControl)
        {
            AudioSource.PlayClipAtPoint(animalUpgrade, Camera.main.transform.position, 0.7f);
        }
        else if ((IssueType) _category == IssueType.Environment)
        {
            AudioSource.PlayClipAtPoint(environmentUpgrade, Camera.main.transform.position, 0.7f);
        }
        else if ((IssueType) _category == IssueType.Infrastructure)
        {
            AudioSource.PlayClipAtPoint(infrastructrueUpgrade, Camera.main.transform.position, 0.7f);
        }
        else if ((IssueType) _category == IssueType.Regulations)
        {
            AudioSource.PlayClipAtPoint(regulationsUpgrade, Camera.main.transform.position, 0.7f);
        }

        UpgradeText.text = "Upgraded!";
        Money.SubtractMoney(UpgradeCost);
        UpgradeCost += 20;
        UpdateStars();
    }

    void UpdateStarsTransport()
    {
        int currentType = 0;
        foreach (Button b in TransportZonesButtons)
        {

            Image[] image = b.GetComponentsInChildren<Image>();

            for (int i = 0; i < image.Length; ++i)
            {
                if (image[i] == b.GetComponent<Image>())
                    continue;

                if (tileManager.CheckTransportLevel((ZONE)currentType + 1) < i)
                {
                    image[i].color = Color.clear;
                    //image[i].gameObject.SetActive(false);
                }
                else
                {
                    image[i].color = Color.white;
                    image[i].gameObject.SetActive(true);
                }
            }
            currentType++;
        }
    }

    public void ShowPanel(int _type)
    {
        if (_type == 0)
        {
            AudioSource.PlayClipAtPoint(rubbishTruck, Camera.main.transform.position, 0.7f);
            TransportZones.SetActive(false);
            RubbishZones.SetActive(true);
        }
        else if (_type == 1)
        {
            AudioSource.PlayClipAtPoint(train, Camera.main.transform.position, 0.7f);
            RubbishZones.SetActive(false);
            TransportZones.SetActive(true);
        }

        for (int i = 0; i < UpgradeButtons.Length; ++i)
        {
            UpgradeButtons[i].interactable = false;
        }

        if (_type == 0)
        {
            UpgradeButtons[4].interactable = true;
        }
        else if (_type == 1)
        {
            UpgradeButtons[5].interactable = true;
        }
    }

    public void SendRubbishTruck(int _zone)
    {
        for (int i = 0; i < RubbishTrucksZonesButtons.Length; ++i)
        {
            if (i == _zone - 1)
            {
                RubbishTrucksZonesButtons[i].interactable = true;
            }
            else
            {
                RubbishTrucksZonesButtons[i].interactable = false;
            }

        }

        UpgradeText.text = "The rubbish truck will clean up this zone for tomorrow!";

        GameObject rubbishTruck = Instantiate(RubbishTruckPrefab, RubbishTruckSpawnPoint.position, Quaternion.identity, RubbishTruckSpawnPoint.parent);
        rubbishTruck.GetComponent<RubbishTruckController>().SetTilesToGoTo(tileManager.GetTiles((ZONE) _zone));
        tileManager.SendRubbishTruck((ZONE)_zone);
        Money.SubtractMoney((float)UpgradeCost);
        UpgradeCost += 20;
    }

    public void SendTrain(int _zone)
    {
        for (int i = 0; i < TransportZonesButtons.Length; ++i)
        {
            if (i == _zone - 1)
            {
                TransportZonesButtons[i].interactable = true;
            }
            else
            {
                TransportZonesButtons[i].interactable = false;
            }

        }

        UpgradeText.text = "This zones transport is upgraded!";
        tileManager.StartPublicTransport((ZONE)_zone);
        Money.SubtractMoney((float)UpgradeCost);
        UpgradeCost += 20;
        UpdateStarsTransport();


       // train1.SetActive(true);
        //train2.SetActive(true);
    }

    public void ResetButtons()
    {
        for (int i = 0; i < UpgradeButtons.Length; ++i)
        {
            UpgradeButtons[i].interactable = true;
        }

        for (int i = 0; i < TransportZonesButtons.Length; ++i)
        {
            TransportZonesButtons[i].interactable = true;
        }

        for (int i = 0; i < RubbishTrucksZonesButtons.Length; ++i)
        {
            RubbishTrucksZonesButtons[i].interactable = true;
        }

        TransportZones.SetActive(false);
        RubbishZones.SetActive(false);
        UpgradeCostText.text = "The cost of today's upgrade " + UpgradeCost;
    }
}
