using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 剑士 : Unit
{
    // Start is called before the first frame update
    List<int> BehindCells = new() { 7,8,9 };
    public bool repelTarget = false;
    public override void GetTargets()
    {
        base.GetTargets();
        if(BattleMgr.Ins.Targets.Count > 0)
        {
            Unit t2 = BattleMgr.Ins.FindUnitOnCell(BattleMgr.Ins.Targets[0].Cell + 3, IsEnemy);
            if (t2 != null)
            {
                BattleMgr.Ins.Targets.Add(t2);
                repelTarget = false;
            }
            else
            {
                repelTarget = true;
            }
        }
    }

    public override void OnAtkMonent()
    {
        base.OnAtkMonent();
        Repel();
    }

    public void Repel()
    {
        if (!repelTarget) return;
        if (!BehindCells.Contains(BattleMgr.Ins.Targets[0].Cell))
        {
            GameObject side;
            if (BattleMgr.Ins.Targets[0].IsEnemy)
            {
                side = BattleMgr.Ins.eneObj;
            }
            else
            {
                side = BattleMgr.Ins.ourObj;
            }
            //下标0开始 所以只+2
            Transform newParent = side.transform.GetChild(BattleMgr.Ins.Targets[0].Cell + 2);
            BattleMgr.Ins.Targets[0].Cell = BattleMgr.Ins.Targets[0].Cell + 3;
            BattleMgr.Ins.Targets[0].transform.parent = newParent;
            BattleMgr.Ins.Targets[0].transform.DOLocalMoveX(0, 0.5f);
        }
    }


}
