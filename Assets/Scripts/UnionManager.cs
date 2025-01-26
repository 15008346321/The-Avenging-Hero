using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnionManager : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnClickTravel()
    {
        EventsMgr.Ins.生成一个战斗路线();
    }
}
