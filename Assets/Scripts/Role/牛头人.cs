using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class 牛头人 : Unit
{
    // Start is called before the first frame update

    public override void 受到攻击时()
    {
        获取技能点();
    }

    public override void 技能帧()
    {
        BattleMgr.Ins.对目标群体加buff(BuffsEnum.盲目);
    }
}
