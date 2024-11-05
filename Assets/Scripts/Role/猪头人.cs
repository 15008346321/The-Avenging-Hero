using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class 猪头人 : Unit
{
    // Start is called before the first frame update

    public override void 单位死亡时(bool 死亡单位是否是玩家阵营)
    {
        if (该单位是否是玩家阵营 != 死亡单位是否是玩家阵营)
        {
            OriData.MaxHp += 1;
            MaxHp += 1;
            Hp += 1;
            TakeHeal(MaxHp * 0.05f);
        }
    }
}
