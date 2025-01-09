using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class 技能基类
{
    public Unit 角色实例;
    public bool 技能1已触发, 技能2已触发;
    public void Init(Unit u)
    {
        角色实例 = u;
    }
    public virtual void 战斗开始时()
    {

    }
    public virtual void 回合开始时()
    {

    }
    public virtual void 回合结束时()
    {
        for (int i = 0; i < 角色实例.BuffsList.Count; i++)
        {
            角色实例.BuffsList[i].OnTurnEnd();
        }
    }
    public virtual void 战斗结束时()
    {

    }

    public virtual void ExecuteAtk()
    {
        Buff 盲目 = 角色实例.BuffsList.Find(item => item.Name == BuffsEnum.盲目);
        if (盲目 != null)
        {
            StatePoolMgr.Ins.状态(角色实例, "盲目-行动失败");
            return;
        }

        角色实例.动画播放完毕 = false;
        获取攻击目标();
        if (BattleMgr.Ins.Targets.Count == 0)//没有目标就走位 然后进行下一个
        {
            角色实例.移动到目标同一列();
        }
        else
        {
            角色实例.Anim.Play("atk");//后面两方法在动画帧后段调用
        }
    }

    //特殊攻击目标需要重写 默认正前方 
    public virtual void 获取攻击目标()
    {
        BattleMgr.Ins.获取正前方目标(角色实例.阵营, 角色实例.Cell);
    }

    //如果攻击有特殊逻辑则重写 动画上调用
    public virtual void 攻击帧()
    {
        BattleMgr.Ins.CaculDamage(角色实例.Atk);
        攻击特效();
    }

    //攻击帧()时调用 攻击时特效重写实现(获取技能点 附加伤害...)
    public virtual void 攻击特效()
    {
    }

    public void ExecuteSkill()
    {
        //需要重写实现逻辑
        获取技能目标();

        //之后调用base执行下面代码 把技能点消了
        角色实例.SkillPoint = 0;
        foreach (var item in 角色实例.SkillPointIcon)
        {
            item.DOFade(0.5f, 0);
        }
        角色实例.IsSkillReady = false;

        //动画中会执行 技能帧();
        角色实例.动画播放完毕 = false;
        角色实例.Anim.Play("skill", 0, 0);
    }

    //需要重写 默认正前方目标 
    public virtual void 获取技能目标()
    {
        BattleMgr.Ins.获取正前方目标(角色实例.阵营, 角色实例.Cell);
    }
    public virtual void 技能帧()
    {
    }
    //AttrType:Atk/Fire/Water/Wind/Thunder/Earth  AtkType:Atk/Comb
    public void TakeDamage(float Damage, ElementType elementType = ElementType.物理伤害, DamageType damageType = DamageType.攻击伤害)
    {
        Damage = Mathf.RoundToInt(Damage);
        //伤害减免
        更新伤害减免();
        switch (elementType)
        {
            case ElementType.物理伤害:
                Damage -= 角色实例.物理伤害减免;
                break;
            case ElementType.火元素伤害:
                break;
            case ElementType.土元素伤害:
                break;
            case ElementType.燃烧伤害:
                break;
            case ElementType.出血伤害:
                break;
            case ElementType.中毒伤害:
                break;
        }

        if (角色实例.护盾 > 0)
        {
            if (角色实例.护盾 >= Damage)
            {
                角色实例.护盾 -= Damage;
                Damage = 0;
            }
            else
            {
                Damage -= 角色实例.护盾;
                角色实例.护盾 = 0;
            }
        }
        角色实例.生命值 -= Damage;
        角色实例.更新生命值();
        角色实例.CheckDeath();

        //出类型伤害和特效
        switch (elementType)
        {
            case ElementType.物理伤害:
                StatePoolMgr.Ins.类型伤害(角色实例, Damage, "物理");
                break;
            case ElementType.火元素伤害:
                StatePoolMgr.Ins.类型伤害(角色实例, Damage, "火元素");
                break;
            case ElementType.土元素伤害:
                StatePoolMgr.Ins.类型伤害(角色实例, Damage, "土元素");
                break;
            case ElementType.燃烧伤害:
                StatePoolMgr.Ins.类型伤害(角色实例, Damage, "燃烧");
                break;
            case ElementType.出血伤害:
                StatePoolMgr.Ins.类型伤害(角色实例, Damage, "出血");
                break;
            case ElementType.中毒伤害:
                StatePoolMgr.Ins.类型伤害(角色实例, Damage, "中毒");
                break;
        }

        switch (damageType)
        {
            case DamageType.攻击伤害:
                受到攻击时();
                break;
            case DamageType.技能伤害:
                break;
            case DamageType.异常伤害:
                break;
        }
    }//被攻击时特效 减伤 养成

    public virtual void 更新伤害减免()
    {
    }

    public void TakeHeal(float HealValue)
    {
        if (角色实例.BuffsList.Exists(b => b.Name == BuffsEnum.燃烧)) HealValue = 0;
        HealValue = Mathf.Round(HealValue);
        if (角色实例.生命值 + HealValue >= 角色实例.MaxHp)
        {
            HealValue = 角色实例.MaxHp - 角色实例.生命值;
            角色实例.生命值 = 角色实例.MaxHp;
        }
        else
        {
            角色实例.生命值 += HealValue;
        }
        StatePoolMgr.Ins.类型伤害(角色实例, HealValue, "治疗");
        角色实例.更新生命值();
        受到治疗时();
    }

    public virtual void 受到攻击时()
    {
        foreach (var item in 角色实例.BuffsList)
        {
            //item.RcAtk();
        }
    }

    public virtual void 受到治疗时()
    {

    }

    public virtual void 该单位阵亡时()
    {
    }

    public virtual void 有单位阵亡时(阵营Enum _阵营)
    {
    }

    public void RcComb()
    {
        foreach (var item in 角色实例.BuffsList)
        {
            //item.RcComb();
        }
    }
    public void RcPhs()
    {
        foreach (var item in 角色实例.BuffsList)
        {
            //item.RcPhs();
        }
    }
    public void RcFire()
    {
        foreach (var item in 角色实例.BuffsList)
        {
            //item.RcFire();
        }
    }
    public void RcWater()
    {
        foreach (var item in 角色实例.BuffsList)
        {
            //item.RcWater();
        }
    }
    public void RcWind()
    {
        foreach (var item in 角色实例.BuffsList)
        {
            //item.RcWind();
        }
    }
    public void RcThunder()
    {
        foreach (var item in 角色实例.BuffsList)
        {
            //item.RcThunder();
        }
    }
    public void RcEarth()
    {
        foreach (var item in 角色实例.BuffsList)
        {
            //item.RcEarth();
        }
    }

    public int 设置召唤物位置()
    {
        if ((角色实例.Cell == 4 || 角色实例.Cell == 7) && (BattleMgr.Ins.查找指定阵营位置上单位(角色实例.阵营, 1) == null)) return 1;
        if ((角色实例.Cell == 5 || 角色实例.Cell == 8) && (BattleMgr.Ins.查找指定阵营位置上单位(角色实例.阵营, 2) == null)) return 2;
        if ((角色实例.Cell == 6 || 角色实例.Cell == 9) && (BattleMgr.Ins.查找指定阵营位置上单位(角色实例.阵营, 3) == null)) return 3;
        //没指定时 设置位置会从1-9挨个找空位
        return 1;
    }
}
