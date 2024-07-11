using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetMbrInfoIdx : MonoBehaviour
{
    // Start is called before the first frame update
    public Button btn;

    public void SetCurrMbr()
    {
        TeamManager.Ins.MbrInfoIdx = transform.GetSiblingIndex();
    }
}
