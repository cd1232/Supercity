using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IconManager : MonoBehaviour {

    public CategoryController[] Icons;

    // Use this for initialization
    void Start() {
        Icons = GetComponentsInChildren<CategoryController>();
    }

    // Update is called once per frame
    void Update() {

        //for (int i = 0; i < Icons.Length; ++i)
        //{
        //    if (Icons[i].bSelected)
        //    {
        //        Icons[i].transform.DOScale(1.1f, 0.1f);
        //    }
        //    else
        //    {
        //        Icons[i].transform.DOScale(0.8f, 0.1f);
        //    }
        //}

    }

    public void SetAllIconsDeselected()
    {
        for (int i = 0; i < Icons.Length; ++i)
        {
            Icons[i].bSelected = false;
            Icons[i].transform.DOScale(0.8f, 0.1f);
        }
    }
}
