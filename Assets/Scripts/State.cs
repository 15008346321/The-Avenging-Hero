using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class State : MonoBehaviour
{
    public Unit U;
    public TextMeshProUGUI tmp;
    public Animator animator;
    
    //heal动画帧上调用

    public void SetToPool()
    {
        transform.SetParent(StatePoolMgr.Ins.transform);
        gameObject.SetActive(false);
    }
}
