using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MbrBtn : MonoBehaviour
{
    // Start is called before the first frame update
    public Button btn;
    public Image Arrow;
    public void ShowDetail()
    {
        TeamMgr.Ins.CurrMbrIdx = transform.GetSiblingIndex();
        TeamMgr.Ins.ShowTeam();
    }
}
