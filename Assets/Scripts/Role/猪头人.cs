using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class 猪头人 : Unit
{
    // Start is called before the first frame update

    public override void 单位死亡时(bool isEnemy)
    {
        if (IsEnemy != isEnemy)
        {
            OriData.MaxHp += 1;
            MaxHp += 1;
            Hp += 1;
            TakeHeal(MaxHp * 0.05f);
        }
    }
}
