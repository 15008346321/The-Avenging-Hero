using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetMbrSelIdx : MonoBehaviour
{
    // Start is called before the first frame update
    public Button btn;

    public void SetCurrMbr()
    {
        TeamManager.Ins.MbrSelIdx = transform.GetSiblingIndex();
    }
}
