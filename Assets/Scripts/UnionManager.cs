using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnionManager : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnClickTravel()
    {
        Debug.LogError("OnClickTravel");
        Menu.Ins.EnterNewLevel();
        BattleMgr.Ins.InitTeam();
        EventsMgr.Ins.ExploreBtn.gameObject.SetActive(true);
    }
}
