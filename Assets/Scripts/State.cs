using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class State : MonoBehaviour
{
    public Unit U;
    public TextMeshProUGUI tmp;
    public Animator animator;
    public Image image;
    
    //heal动画帧上调用

    public void SetToPool()
    {
        transform.SetParent(StatePoolMgr.Ins.transform);
        gameObject.SetActive(false);
    }
}
