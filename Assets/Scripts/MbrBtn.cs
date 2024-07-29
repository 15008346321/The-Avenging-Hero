using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MbrBtn : MonoBehaviour
{
    // Start is called before the first frame update
    public Button btn;

    public void ShowDetail()
    {
        TeamManager.Ins.MbrInfoIdx = transform.GetSiblingIndex();
        TeamManager.Ins.ShowDetail();
    }
}
