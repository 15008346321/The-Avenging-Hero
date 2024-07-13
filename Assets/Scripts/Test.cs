using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform u, t;
    public Unit unit;

    public void test()
    {
        //u.DOMove(t.position, 1f);
        BattleMgr.Ins.ShowSkillName(unit, "拔刀斩");
    }

    public void test1()
    {
        StartCoroutine(test2());
    }

    public IEnumerator test2()
    {
        GameObject g = BattleMgr.Ins.GetUnitObj("初级剑士");
        yield return new WaitForSeconds(2);
        Unit u = Instantiate(g).transform.GetChild(0).GetComponent<Unit>();
    }
}
