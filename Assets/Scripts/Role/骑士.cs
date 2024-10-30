using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 骑士 : Unit
{
    public override void OnBattleStart()
    {
        foreach (var item in BattleMgr.Ins.Team)
        {
            item.获取护盾(Mathf.Round(MaxHp * 0.2f));
        } 
    }

    public override void OnTurnStart()
    {
        获取护盾(Mathf.Round(MaxHp * 0.05f));
    }
}
