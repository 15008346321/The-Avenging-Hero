using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class 村长的治愈护符 :遗物基类
{
    public 村长的治愈护符()
    {
        Name = "村长的传家宝";
        Dscrp = "回合结束时,恢复小队生命值最低的单位5生命值";
        遗物图片 = CSVMgr.Ins.遗物图片[Name];
        可装备 = false;
    }

    public override void 回合结束时()
    {
        触发动画();
        Unit u = BattleMgr.Ins.小队列表.Where(u => u.IsDead == false && u.生命值 < u.MaxHp).OrderBy(u => u.生命值).FirstOrDefault();
        if (u != null)
        {
            u.技能.TakeHeal(5);
        }
    }

    public override void Test()
    {
        Debug.LogError("这是村长的传家宝");
    }
}
