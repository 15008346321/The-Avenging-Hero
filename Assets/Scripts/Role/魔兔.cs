using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class 魔兔 : 技能基类
{
    // Start is called before the first frame update

    public override void 攻击特效()
    {
        if(Random.Range(0,2) == 1)
        {
            BattleMgr.Ins.Targets[0].添加Buff(BuffsEnum.出血);
            角色实例.添加Buff(BuffsEnum.出血);
            foreach (var item in BattleMgr.Ins.AllUnit)
            {
                if (item.BuffsList.Exists(b => b.Name == BuffsEnum.出血)) 角色实例.Atk += 1;
            }
        }
    }
}
