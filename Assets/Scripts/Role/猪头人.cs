using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class 猪头人 : 技能基类
{
    // Start is called before the first frame update

    public override void 有单位阵亡时(阵营Enum _阵营)
    {
        if (角色实例.阵营 != _阵营)
        {
            角色实例.OriData.MaxHp += 1;
            角色实例.MaxHp += 1;
            角色实例.生命值 += 1;
            TakeHeal(角色实例.MaxHp * 0.05f);
        }
    }
}
