using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform u,t;
    public Unit unit;

    public void test()
    {
        //u.DOMove(t.position, 1f);
        BattleMgr.Ins.ShowSkillName(unit,"拔刀斩");
    }
}
