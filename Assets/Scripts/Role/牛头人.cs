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
        SkillPoint += 1;
        SkillPointIcon[SkillPoint - 1].DOFade(1, 0);
        if (SkillPoint == SkillPointMax)
        {
            IsSkillReady = true;
        }
    }

    public override void 技能帧()
    {
        BattleMgr.Ins.SetDebuff(BuffsEnum.盲目);
    }
}
