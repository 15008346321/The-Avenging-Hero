using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 巨魔 : Unit
{
    public override void OnTurnStart()
    {
        召唤哥布林();
    }
    public override void 受到攻击时()
    {
        base.受到攻击时();
        if (Hp * 2 < MaxHp && !IsSkillTriggered)
        {
            召唤哥布林();
            IsSkillTriggered = true;
        }
    }

    public void 召唤哥布林()
    {
        GameObject gbl = Resources.Load("Prefabs/Unit/" + "哥布林") as GameObject;
        BattleMgr.Ins.SummonCreator(gbl, IsEnemy, 0.8f);
    }
}
