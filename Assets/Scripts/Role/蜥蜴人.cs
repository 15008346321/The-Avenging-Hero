using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class 蜥蜴人 : Unit
{
    // Start is called before the first frame update
    public override void 更新伤害减免()
    {
        物理伤害减免 =  2 + Mathf.RoundToInt(Bloods.Find(b => b.Name == "水元素").Value * 0.33f);
    }
    public override void 获取攻击目标()
    {
        base.获取攻击目标();
        if(BattleMgr.Ins.Targets.Count>0)
        {
            Unit t2 = BattleMgr.Ins.FindUnitOnCell(BattleMgr.Ins.Targets[0].Cell + 3, IsEnemy);
            if (t2!=null)
            {
                print("Find t2");
                BattleMgr.Ins.Targets.Add(t2);
            }
        }
    }
}
