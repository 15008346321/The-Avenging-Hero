using System.Collections;
using UnityEngine;

public class 火焰纹杖 : GearBase
{
    public override void OnAdd(UnitData owner)
    {
        OwnerUnitData = owner;
        OwnerUnitData.Weapon = this;
        OwnerUnitData.Fire += 10;
        OwnerUnitData.Water -= 5;
        TeamManager.Ins.ShowTeam();
    }

    public override void OnAtk()
    {
        throw new System.NotImplementedException();
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

    public override void OnHeal()
    {
        throw new System.NotImplementedException();
    }

    public override void OnRemove()
    {
        OwnerUnitData.Fire -= 10;
        OwnerUnitData.Water += 5;

        OwnerUnitData.Weapon = null;
        OwnerUnitData = null;
    }

    public override void OnShield()
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

    public override void Test()
    {
        throw new System.NotImplementedException();
    }

    public override void Upgrade()
    {
        throw new System.NotImplementedException();
    }
}
