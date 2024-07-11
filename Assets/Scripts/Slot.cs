using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Button Btn;
    void Start()
    {
        Btn = GetComponent<Button>();
        Btn.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        BagManager.Ins.CurrSlot = transform.GetSiblingIndex();
        BagManager.Ins.ShowDscrp();
    }
}
