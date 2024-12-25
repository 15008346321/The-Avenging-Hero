using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class 猪头人 : Unit
{
    // Start is called before the first frame update

    public override void 有单位阵亡时(阵营Enum _阵营)
    {
        if (阵营 != _阵营)
        {
            OriData.MaxHp += 1;
            MaxHp += 1;
            生命值 += 1;
            TakeHeal(MaxHp * 0.05f);
        }
    }
}
