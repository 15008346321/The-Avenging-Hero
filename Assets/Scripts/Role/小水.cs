using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class 小水 : 技能基类
{
    // Start is called before the first frame update
    public Unit 召唤者;
    public bool 屏障已激活;

    public override void 回合结束时()
    {
        ExecuteSkill();
       
    }

    public override void 技能帧()
    {
        治疗术();
    }

    public void 治疗术()
    {
        List<Unit> 查找的阵营;

        if (角色实例.阵营 == 阵营Enum.我方) 查找的阵营 = BattleMgr.Ins.小队列表;

        else 查找的阵营 = BattleMgr.Ins.敌方小队列表;

        查找的阵营.OrderBy(u => u.生命值).FirstOrDefault().技能.TakeHeal(召唤者.Bloods.Find(b => b.Name == 魔力类型Enum.水元素).Value * 0.5f);
    }

    public override void 该单位阵亡时()
    {
        if(屏障已激活)
        {
            召唤者.获取护盾(召唤者.Bloods.Find(b => b.Name == 魔力类型Enum.水元素).Value);
        }
    }
}
