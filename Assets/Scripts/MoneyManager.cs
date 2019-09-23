using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MoneyManager : MonoBehaviour
{

    float fMoney = 6000;
    private Text MoneyText;
    private Color StartColor;

    // Use this for initialization
    void Start () {

        MoneyText = GetComponentInChildren<Text>();
        MoneyText.text = fMoney.ToString();
        StartColor = MoneyText.color;    
    }

    public void AddMoney(float _value)
    {
        fMoney += _value;
        MoneyText.text = fMoney.ToString();
        MoneyText.DOColor(Color.green, 0.2f);
        StartCoroutine(ReturnToNormal());
    }

    public void SubtractMoney(float _value)
    {
        fMoney -= _value;
        MoneyText.text = fMoney.ToString();
        MoneyText.DOColor(Color.red, 0.2f);
        StartCoroutine(ReturnToNormal());
    }

    public float GetMoney()
    {
        return fMoney;
    }

    IEnumerator ReturnToNormal()
    {
        yield return new WaitForSeconds(0.5f);
        MoneyText.DOColor(StartColor, 0.2f);
    }
}
