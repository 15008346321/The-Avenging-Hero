using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 哥布林 : Unit
{
    // Start is called before the first frame update

    public override void OnTurnStart()
    {
        if (!IsEnemy)
        {
            Atk += BattleMgr.Ins.Team.Count;
        }
        else
        {
            Atk += BattleMgr.Ins.Enemys.Count;
        }
    }
}
