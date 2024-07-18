using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class 初级法师 : Unit
{
    //public List<Buff> Buffs = new();
    private void Start()
    {
        Init();
    }
    public void Init()
    {
        TotalAtkCount = 1;
        RemainAtkCount = 1;
        TotalCombCount = 1;
        RemainCombCount = 1;
        //不同角色不同追打
        CombTypes.Add("BuffBurning");
    }
    
    public override void ExecuteAtk()
    {
        BattleMgr.Ins.AtkU = this;
        BattleMgr.Ins.AtkedU = BattleMgr.Ins.GetOppositeTarget();
        if (BattleMgr.Ins.AtkedU == null)//没有目标就走位 然后进行下一个
        {
            MoveToEnemyFrontRow();
            BattleMgr.Ins.FindNextActionUnit(1);
        }
        else
        {
            print("msg " + "play closeAtk1");
            Animator.Play("closeAtk1");//动画帧后段会计算伤害 在这之前获取目标
            //不同角色不同技能名
            ShowSkillName("火球");
            GetRandomMagic(1);
        }
    }
    //动画帧上调用
    public override void AddAtkEffectOnAtk()
    {
        print("check" + BattleMgr.Ins.AtkedU.name + " if isdead");
        if (BattleMgr.Ins.AtkedU.isDead) return;
        if (!TrySuccess(0.5f)) return;
        if (BattleMgr.Ins.AtkedU != null)
        {
            BattleMgr.Ins.ShowFont(BattleMgr.Ins.AtkedU, 0, "GetBleed");
            //添加流血buff 已经有了就加层数
            if (BattleMgr.Ins.AtkedU.GetComponent<BuffBleed>() == null)
            {
                BuffBleed b = BattleMgr.Ins.AtkedU.gameObject.AddComponent<BuffBleed>();
                BattleMgr.Ins.AtkedU.Buffs.Add(b);
                b.Init(this, BattleMgr.Ins.AtkedU);
            }
            else
            {
                BattleMgr.Ins.AtkedU.GetComponent<BuffBleed>().AddLayer();
            }
            BattleMgr.Ins.CheckComb(this, "BuffBleed");
        }
    }
    //在普攻动画的帧上调用!!!
    //攻击时计算增伤buff
    public override void CaculDamageOnAtk()
    {
        var damage = Atk;

        print("msg " + name + " CaculDamageOnAtk");
        BattleMgr.Ins.AtkedU.CaculDamageOnAtked(damage);
    }
    public override void ExecuteComb()
    {
        BattleMgr.Ins.CombU = this;
        Animator.Play("closeAtk2");
        //不同角色不同追打
        ShowSkillName("熏风");
        GetRandomMagic(1);
    }
    //在追打动画的帧上调用!!!
    public override void CaculDamageOnComb()
    {
        var damage = Mathf.RoundToInt(0.3f * Atk);
        BattleMgr.Ins.AtkedU.CaculDamageOnAtked(damage);
    }
    public override void AddAtkEffectOnComb()
    {
        throw new NotImplementedException();
    }
    //被攻击时计算减伤buff
    public override void CaculDamageOnAtked(int damage)
    {
        BattleMgr.Ins.ShowFont(this, damage, "Hurt");
    }
    public override void CaculDamageOnCombed(int damage)
    {
        BattleMgr.Ins.ShowFont(this, damage, "Hurt");
    }
    public override void OnTurnEnd()
    {
        //添加该单位回合结束时动作

        //结算buff回合结束时效果
        foreach (var item in Buffs)
        {
            item.OnTurnEnd();
        }
    }

    public void Test()
    {

        print("test");
    }

}
