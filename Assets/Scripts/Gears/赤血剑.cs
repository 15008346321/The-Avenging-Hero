using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 赤血剑 : GearBase
{
    public override void OnAdd()
    {
        if(Level == 0)//白
        {
            OwnerUnit.Atk += 2;
        }else if(Level == 1)//绿
        {
            OwnerUnit.Atk += 5;
        }
        else if (Level == 2)//蓝
        {
            OwnerUnit.Atk += 9;
        }
        else if (Level == 3)//紫
        {
            OwnerUnit.Atk += 14;
            OwnerUnit.Speed += 3;
        }
        else if (Level == 4)//橙
        {
            OwnerUnit.Atk += 20;
            OwnerUnit.Speed += 5;
        }
        else if (Level == 5)//红
        {
            OwnerUnit.Atk += 30;
            OwnerUnit.Speed += 7;
        }
    }

    public override void OnRemove()
    {
        if (Level == 0)//白
        {
            OwnerUnit.Atk -= 2;
        }
        else if (Level == 1)//绿
        {
            OwnerUnit.Atk -= 5;
        }
        else if (Level == 2)//蓝
        {
            OwnerUnit.Atk -= 9;
        }
        else if (Level == 3)//紫
        {
            OwnerUnit.Atk -= 14;
            OwnerUnit.Speed -= 3;
        }
        else if (Level == 4)//橙
        {
            OwnerUnit.Atk -= 20;
            OwnerUnit.Speed -= 5;
        }
        else if (Level == 5)//红
        {
            OwnerUnit.Atk -= 30;
            OwnerUnit.Speed -= 7;
        }
    }
    public override void OnAtk()
    {
        CurrXP += 1;
        if (CurrXP == XP[Level]) Upgrade();
        if (Level >= 3)
        {
            OwnerUnit.Atk += 1;
        }
        if (Level >= 4)
        {
            foreach (var item in BattleMgr.Ins.Targets)
            {
                BuffBleed newBuff = new();
                item.Buffs.Add(newBuff);
                newBuff.Init(OwnerUnit, item);
            }
        }
    }

    public override void OnComb()
    {
        throw new System.NotImplementedException();
    }

    public override void OnEarth()
    {
        throw new System.NotImplementedException();
    }

    public override void OnFire()
    {
        throw new System.NotImplementedException();
    }

    public override void OnThunder()
    {
        throw new System.NotImplementedException();
    }

    public override void OnWater()
    {
        throw new System.NotImplementedException();
    }

    public override void OnWind()
    {
        throw new System.NotImplementedException();
    }

    public override void OnHeal()
    {
        throw new System.NotImplementedException();
    }

    public override void OnShield()
    {
        throw new System.NotImplementedException();
    }

    public override void Upgrade()
    {
        CurrXP = 0;
        OnRemove();
        Level += 1;
        OnAdd();
    }

    public override void Test()
    {
        throw new System.NotImplementedException();
    }
}
