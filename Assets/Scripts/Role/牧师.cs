using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 牧师 : Unit
{
    // Start is called before the first frame update
    List<int> BehindCells = new() { 7,8,9 };
    public bool repelTarget = false;

    public override void ExecuteAtk()
    {
        ExecuteSkill();
    }
    public override void 获取技能目标()
    {
        BattleMgr.Ins.获取阵营血量最低目标(IsEnemy);
    }
    public override void 技能帧()
    {
        float healvalue = 3 + Mathf.Round(Bloods.Find(item => item.Name == "水元素").Value * 0.5f);

        BattleMgr.Ins.Targets[0].TakeHeal(healvalue);
    }
}
