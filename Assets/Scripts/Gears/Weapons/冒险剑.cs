using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 冒险剑 : GearBase
{
    public override void OnAdd(UnitData owner)
    {
        OwnerUnitData = owner;
        OwnerUnitData.Weapon = this;
        OwnerUnitData.Atk += 10;
        TeamManager.Ins.ShowTeam();
    }

    public override void OnRemove()
    {
        OwnerUnitData.Atk -= 10;
        OwnerUnitData.WeaponName = null;
        OwnerUnitData = null;
    }
    public override void OnAtk()
    {
      
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
    }

    public override void Test()
    {
        Debug.Log("装备了赤血剑");
    }
}
