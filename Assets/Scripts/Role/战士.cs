using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 战士 : Unit
{
    // Start is called before the first frame update
    public bool 可以击退当前单个目标 = false;
    public override void 获取攻击目标()
    {
        base.获取攻击目标();
        if(BattleMgr.Ins.Targets.Count > 0)
        {
            //看目标后一个有没有单位
            Unit t2 = BattleMgr.Ins.查找指定阵营位置上单位(BattleMgr.Ins.Targets[0].Cell + 3, BattleMgr.Ins.Targets[0].阵营);
            if (t2 != null)
            {
                BattleMgr.Ins.Targets.Add(t2);
                可以击退当前单个目标 = false;
            }
            else
            {
                //目标后面空的就可以击退
                可以击退当前单个目标 = true;
            }
        }
    }

    public override void 攻击帧()
    {
        击退();
        base.攻击帧();
    }

    public void 击退()
    {
        if (!可以击退当前单个目标) return;
        //目标在第三列肯定后面没人就不用判断了
        if (!BattleMgr.Ins.Col3.Contains(BattleMgr.Ins.Targets[0].Cell))
        {
            GameObject FIndIn;
            if (BattleMgr.Ins.Targets[0].阵营 == 阵营Enum.敌方)
            {
                FIndIn = BattleMgr.Ins.ourObj;
            }
            else
            {
                FIndIn = BattleMgr.Ins.eneObj;
            }
            //下标0开始 所以只+2
            Transform newParent = FIndIn.transform.GetChild(BattleMgr.Ins.Targets[0].Cell + 2);
            BattleMgr.Ins.Targets[0].Cell = BattleMgr.Ins.Targets[0].Cell + 3;
            BattleMgr.Ins.Targets[0].transform.parent = newParent;
            BattleMgr.Ins.Targets[0].transform.DOLocalMoveX(0, 0.5f);
        }
    }
}
