using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 法师 : Unit
{
    // Start is called before the first frame update
    List<int> BehindCells = new() { 7,8,9 };
    public bool repelTarget = false;

    public override void OnAtkMonent()
    {
        base.OnAtkMonent();
        FireBall();
    }

    public void FireBall()
    {
        float damage = Mathf.Round(Bloods.Find(item => item.Name == "火元素").Value * 0.5f);

        print("fireball " +damage);
        BattleMgr.Ins.Targets[0].TakeDamage(damage,DamageType.火元素伤害);
        BattleMgr.Ins.Targets[0].Buffs.Add(new 燃烧(BattleMgr.Ins.Targets[0]));
    }
}
