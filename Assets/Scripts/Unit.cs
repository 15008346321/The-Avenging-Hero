using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using System;
using TMPro;


public class Unit : MonoBehaviour
{
    public int Cell, Damage, AtkCountMax, AtkCountCurr, SkillPoint, SkillPointMax, 物理伤害减免;
    public float 生命值, MaxHp, Atk, 护盾, Speed;
    public string Element;
    public bool isBoss, IsDead,该单位是否是玩家阵营,单位是否进行了移动, 技能1已触发, 技能2已触发, IsSkillReady,IsAtkChanged,动画播放完毕,伤害字体已消失;
    public List<Buff> BuffsList = new();
    public List<Blood> Bloods = new();
    public string[] Tags = new string[4];
    public 阵营Enum 阵营;
    public TextMeshProUGUI SpeedTMP,生命值TMP,护盾TMP;
    public Animator Anim;
    public Image Icon;
    public Button Btn;
    public List<Image> SkillPointIcon = new(), BuffListImgs = new();
    public Transform StartParent,StatePos,RunPosParent,DragParent,BuffListImgNode,SkillPointImgNode;
    public RectTransform TMPNameNode;
    public EventSystem _EventSystem;
    public GraphicRaycaster gra;
    public UnitData OriData;
    public Action FinishAtkAction, FinishBehaviorAction;
    public void Awake()
    {
        _EventSystem = FindObjectOfType<EventSystem>();
        gra = FindObjectOfType<GraphicRaycaster>();
        FinishAtkAction += 行动结束;
        生命值TMP = transform.Find("Canvas/生命值/生命值TMP").GetComponent<TextMeshProUGUI>();
        护盾TMP = transform.Find("Canvas/护盾/护盾TMP").GetComponent<TextMeshProUGUI>();
        Icon = transform.Find("Canvas/Icon").GetComponent<Image>();
        BuffListImgNode = transform.Find("Canvas/BuffList");
        SkillPointImgNode = transform.Find("Canvas/SkillPointIcon");
        SpeedTMP = transform.Find("Canvas/Speed/TMP").GetComponent<TextMeshProUGUI>();
        Anim = transform.GetComponent<Animator>();
        //DragParent = GameObject.Find("Canvas/UI/DragParent").transform;
        StatePos = transform.Find("Canvas/FontPos");

        动画播放完毕 = true;
    }

    #region====初始化方法
    public void Init(UnitData data)
    {
        //TMP.text = data.Name;
        Icon.sprite = data.角色图片;
        生命值 = MaxHp = data.MaxHp;
        生命值TMP.text = 生命值.ToString();
        Atk = data.Atk;
        Speed = data.Speed;
        AtkCountMax = 1;
        Bloods = data.Bloods;
        SkillPointMax = data.SkillPointMax;
        InitSkillPointIcon();
        InitBuffListImgs();
    }

    public void InitSkillPointIcon()
    {
        if (SkillPointMax == 0)
        {
            SkillPointImgNode.gameObject.SetActive(false);
            return;
        }

        for (int i = 0; i < SkillPointImgNode.childCount; i++)
        {
            if (i >= SkillPointMax)
            {
                SkillPointImgNode.GetChild(i).gameObject.SetActive(false);
            }
            else
            {
                SkillPointIcon.Add(SkillPointImgNode.GetChild(i).GetComponent<Image>());
                SkillPointIcon[i].DOFade(0.5f, 0);
            }
        }
        SkillPointImgNode.GetComponent<HorizontalLayoutGroup>().spacing = (SkillPointImgNode.childCount - SkillPointMax) * -12.5f;
    }

    public void InitBuffListImgs()
    {
        for (int i = 0; i < BuffListImgNode.childCount; i++)
        {
            BuffListImgs.Add(BuffListImgNode.GetChild(i).GetComponent<Image>());
            BuffListImgs[i].enabled = false;
        }
    }

    #endregion

    #region ====战斗中移动
    public void 单位移动(int TargetCell)
    {
        单位是否进行了移动 = true;
        GameObject obj;
        if (阵营 == 阵营Enum.我方)
        {
            obj = BattleMgr.Ins.ourObj;
        }
        else
        {
            obj = BattleMgr.Ins.eneObj;
        }
        Anim.enabled = false;
        transform.DOMove(obj.transform.GetChild(TargetCell - 1).position, 1f).OnComplete(
            () => 
            {
                transform.SetParent(obj.transform.GetChild(TargetCell - 1));
                Cell = TargetCell;
                Anim.enabled = true;
                动画播放完毕 = true;
            }
        );
    }

    private void 移动到目标同一列()
    {
        单位是否进行了移动 = false;
        GameObject 该单位阵营;
        GameObject 敌对阵营;
        if (阵营 == 阵营Enum.我方)
        {
            该单位阵营 = BattleMgr.Ins.ourObj;
            敌对阵营 = BattleMgr.Ins.eneObj;
        }
        else
        {
            该单位阵营 = BattleMgr.Ins.eneObj;
            敌对阵营 = BattleMgr.Ins.ourObj;
        }
        //GetChild(Cell) 1位置移动到2 2cell的index是1
        if (Cell == 1)
        {
            //2有空去2
            if (该单位阵营.transform.GetChild(1).childCount == 0)
            {
                单位移动(2);
            }
            //4有空去4
            else if (该单位阵营.transform.GetChild(3).childCount == 0)
            {
                单位移动(4);
            }
        }
        else if (Cell == 3)
        {
            //2有空去2
            if (该单位阵营.transform.GetChild(1).childCount == 0)
            {
                单位移动(2);
            }
            //6有空去6
            else if (该单位阵营.transform.GetChild(5).childCount == 0)
            {
                单位移动(6);
            }
        }
        else if (Cell == 7)
        {
            //8有空去8
            if (该单位阵营.transform.GetChild(7).childCount == 0)
            {
                单位移动(8);
            }
            //4有空去4
            else if (该单位阵营.transform.GetChild(3).childCount == 0)
            {
                单位移动(4);
            }
        }
        else if (Cell == 9)
        {
            //8有空去8
            if (该单位阵营.transform.GetChild(7).childCount == 0)
            {
                单位移动(8);
            }
            //6有空去6
            else if (该单位阵营.transform.GetChild(5).childCount == 0)
            {
                单位移动(6);
            }
        }
        else if (Cell == 4)
        {
            //5有空去5
            if (该单位阵营.transform.GetChild(4).childCount == 0)
            {
                单位移动(5);
            }
            //12有空 移动到1
            else if (该单位阵营.transform.GetChild(0).childCount == 0 && 该单位阵营.transform.GetChild(1).childCount == 0)
            {
                单位移动(1);
            }
            //78有空 移动到7
            else if (该单位阵营.transform.GetChild(6).childCount == 0 && 该单位阵营.transform.GetChild(7).childCount == 0)
            {
                单位移动(7);
            }
        }
        else if (Cell == 6)
        {
            //5有空去5
            if (该单位阵营.transform.GetChild(4).childCount == 0)
            {
                单位移动(5);
            }
            //23有空 移动到3
            else if (该单位阵营.transform.GetChild(2).childCount == 0 && 该单位阵营.transform.GetChild(1).childCount == 0)
            {
                单位移动(3);
            }
            //89有空 移动到9
            else if (该单位阵营.transform.GetChild(8).childCount == 0 && 该单位阵营.transform.GetChild(7).childCount == 0)
            {
                单位移动(9);
            }
        }
        else if (Cell == 2)
        {
            //1有空 上路有敌人去1
            if (该单位阵营.transform.GetChild(0).childCount == 0 
                && (敌对阵营.transform.GetChild(0).childCount > 0 || 敌对阵营.transform.GetChild(3).childCount > 0 || 敌对阵营.transform.GetChild(6).childCount > 0))
            {
                单位移动(1);
            }
            //3有空下路有敌人 去3
            else if (该单位阵营.transform.GetChild(2).childCount == 0 
                && (敌对阵营.transform.GetChild(2).childCount > 0 || 敌对阵营.transform.GetChild(5).childCount > 0 || 敌对阵营.transform.GetChild(8).childCount > 0))
            {
                单位移动(3);
            }
            //上面都不满足 5有空去5
            else if (该单位阵营.transform.GetChild(4).childCount == 0)
            {
                单位移动(5);
            }
        }
        else if (Cell == 8)
        {
            //7有空 上路有敌人去7
            if (该单位阵营.transform.GetChild(6).childCount == 0
                && (敌对阵营.transform.GetChild(0).childCount > 0 || 敌对阵营.transform.GetChild(3).childCount > 0 || 敌对阵营.transform.GetChild(6).childCount > 0))
            {
                单位移动(7);
            }
            //9有空下路有敌人 去9
            else if (该单位阵营.transform.GetChild(8).childCount == 0
                && (敌对阵营.transform.GetChild(2).childCount > 0 || 敌对阵营.transform.GetChild(5).childCount > 0 || 敌对阵营.transform.GetChild(8).childCount > 0))
            {
                单位移动(9);
            }
            //上面都不满足 5有空去5
            else if (该单位阵营.transform.GetChild(4).childCount == 0)
            {
                单位移动(5);
            }
        }
        else if (Cell == 5)
        {
            //4有空 上路有敌人去4
            if (该单位阵营.transform.GetChild(3).childCount == 0 && (敌对阵营.transform.GetChild(0).childCount > 0 || 敌对阵营.transform.GetChild(3).childCount > 0 || 敌对阵营.transform.GetChild(6).childCount > 0))
            {
                单位移动(4);
            }
            //6有空 下路有敌人去6
            else if (该单位阵营.transform.GetChild(5).childCount == 0 && (敌对阵营.transform.GetChild(2).childCount > 0 || 敌对阵营.transform.GetChild(5).childCount > 0 || 敌对阵营.transform.GetChild(8).childCount > 0))
            {
                单位移动(6);
            }
            //4没空 上路有敌人 12有空 去2
            else if (该单位阵营.transform.GetChild(3).childCount > 0 && 该单位阵营.transform.GetChild(0).childCount == 0 && 该单位阵营.transform.GetChild(1).childCount == 0
                &&(敌对阵营.transform.GetChild(0).childCount > 0 || 敌对阵营.transform.GetChild(3).childCount > 0 || 敌对阵营.transform.GetChild(6).childCount > 0))
            {
                单位移动(2);
            }
            //4没空 上路有敌人 78有空 去8
            else if (该单位阵营.transform.GetChild(3).childCount > 0 && 该单位阵营.transform.GetChild(8).childCount == 0 && 该单位阵营.transform.GetChild(7).childCount == 0
                && (敌对阵营.transform.GetChild(0).childCount > 0 || 敌对阵营.transform.GetChild(3).childCount > 0 || 敌对阵营.transform.GetChild(6).childCount > 0))
            {
                单位移动(8);
            }
            //6没空 下路有敌人 23有空 去2
            else if (该单位阵营.transform.GetChild(5).childCount > 0 && 该单位阵营.transform.GetChild(2).childCount == 0 && 该单位阵营.transform.GetChild(1).childCount == 0
                && (敌对阵营.transform.GetChild(2).childCount > 0 || 敌对阵营.transform.GetChild(5).childCount > 0 || 敌对阵营.transform.GetChild(8).childCount > 0))
            {
                单位移动(2);
            }
            //6没空 下路有敌人 89有空 去8
            else if (该单位阵营.transform.GetChild(5).childCount > 0 && 该单位阵营.transform.GetChild(8).childCount == 0 && 该单位阵营.transform.GetChild(7).childCount == 0
                && (敌对阵营.transform.GetChild(2).childCount > 0 || 敌对阵营.transform.GetChild(5).childCount > 0 || 敌对阵营.transform.GetChild(8).childCount > 0))
            {
                单位移动(8);
            }
        }
        if (!单位是否进行了移动)
        {
            动画播放完毕 = true;
        }

    }
    #endregion
    #region====流程方法
    public void ReloadAtk()
    {
        AtkCountCurr = AtkCountMax;
    }

    //public void CheckComb(string currDebuff)
    //{
    //    if (BattleMgr.Ins.MainTarget.isDead) return;
    //    List<Unit> units;
    //    if (!IsEnemy)
    //    {
    //        units = BattleMgr.Ins.Team;
    //    }
    //    else
    //    {
    //        units = BattleMgr.Ins.Enemys;
    //    }
    //    foreach (var item in units)
    //    {
    //        if (item.Comb. CombTypes.Contains(currDebuff) 
    //            && item.Comb.RemainCombCount > 0 
    //            && !item.麻痹)
    //        {
    //            BattleMgr.Ins.AnimQueue.Insert(0, item.ID + ":Comb");
    //            item.Comb.RemainCombCount -= 1;
    //            break;
    //        }
    //    }
    //}
    public void 更新生命值()
    {
        //c# int 1/int 3 = 0
        生命值TMP.text = 生命值.ToString();

        if (护盾 > 0)
        {
            护盾TMP.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            护盾TMP.transform.parent.gameObject.SetActive(false);
        }
        护盾TMP.text = 护盾.ToString();
    }
    
    #endregion
#region====战斗方法
    public virtual void ExecuteAtk()
    {
        Buff 盲目 = BuffsList.Find(item => item.Name == BuffsEnum.盲目);
        if (盲目 != null)
        {
            StatePoolMgr.Ins.状态(this, "盲目-行动失败");
            return;
        }

        动画播放完毕 = false;
        获取攻击目标();
        if (BattleMgr.Ins.Targets.Count == 0)//没有目标就走位 然后进行下一个
        {
            移动到目标同一列();
        }
        else
        {
            Anim.Play("atk");//后面两方法在动画帧后段调用
        }
    }

    //特殊攻击目标需要重写 默认正前方 
    public virtual void 获取攻击目标()
    {
        BattleMgr.Ins.获取正前方目标(阵营, Cell);
    }

    //如果攻击有特殊逻辑则重写 动画上调用
    public virtual void 攻击帧()
    {
        BattleMgr.Ins.CaculDamage(Atk);
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
        SkillPoint = 0;
        foreach (var item in SkillPointIcon)
        {
            item.DOFade(0.5f, 0);
        }
        IsSkillReady = false;

        //动画中会执行 技能帧();
        动画播放完毕 = false;
        Anim.Play("skill", 0, 0);
    }

    //需要重写 默认正前方目标 
    public virtual void 获取技能目标() 
    {
        BattleMgr.Ins.获取正前方目标(阵营, Cell);
    }
    public virtual void 技能帧()
    {
    }
    //AttrType:Atk/Fire/Water/Wind/Thunder/Earth  AtkType:Atk/Comb
    public void TakeDamage(float Damage,ElementType elementType = ElementType.物理伤害, DamageType damageType = DamageType.攻击伤害)
    {
        Damage = Mathf.RoundToInt(Damage);
        //伤害减免
        更新伤害减免();
        switch (elementType)
        {
            case ElementType.物理伤害:
                Damage -= 物理伤害减免;
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

        if (护盾 > 0)
        {
            if (护盾 >= Damage)
            {
                护盾 -= Damage;
                Damage = 0;
            }
            else
            {
                Damage -= 护盾;
                护盾 = 0;
            }
        }
        生命值 -= Damage;
        更新生命值();
        CheckDeath();

        //出类型伤害和特效
        switch (elementType)
        {
            case ElementType.物理伤害:
                StatePoolMgr.Ins.类型伤害(this, Damage, "物理");
                break;
            case ElementType.火元素伤害:
                StatePoolMgr.Ins.类型伤害(this, Damage, "火元素");
                break;
            case ElementType.土元素伤害:
                StatePoolMgr.Ins.类型伤害(this, Damage, "土元素");
                break;
            case ElementType.燃烧伤害:
                StatePoolMgr.Ins.类型伤害(this, Damage, "燃烧");
                break;
            case ElementType.出血伤害:
                StatePoolMgr.Ins.类型伤害(this, Damage, "出血");
                break;
            case ElementType.中毒伤害:
                StatePoolMgr.Ins.类型伤害(this, Damage, "中毒");
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
        if (BuffsList.Exists(b => b.Name == BuffsEnum.燃烧)) HealValue = 0;
        HealValue = Mathf.Round(HealValue);
        if (生命值 + HealValue >= MaxHp)
        {
            HealValue = MaxHp - 生命值;
            生命值 = MaxHp;
        }
        else
        {
            生命值 += HealValue;
        }
        StatePoolMgr.Ins.类型伤害(this, HealValue, "治疗");
        更新生命值() ;
        受到治疗时();
    }

    public void CheckDeath()
    {
        if (生命值 <= 0|| IsDead == true)
        {
            IsDead = true;
            StartCoroutine(延时设置死亡());
            该单位阵亡时();
            BattleMgr.Ins.有其他单位阵亡时(阵营);
            StartCoroutine(BattleMgr.Ins.CheckBattleEnd());
        }
    }
    public IEnumerator 延时设置死亡() 
    {
        yield return new WaitForSeconds(0.5f);
        transform.SetParent(BattleMgr.Ins.DeadParent);
        transform.localPosition = Vector3.zero;
    }

    public virtual void 受到攻击时()
    {
        foreach (var item in BuffsList)
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
        foreach (var item in BuffsList)
        {
            //item.RcComb();
        }
    }
    public void RcPhs()
    {
        foreach (var item in BuffsList)
        {
            //item.RcPhs();
        }
    }
    public void RcFire()
    {
        foreach (var item in BuffsList)
        {
            //item.RcFire();
        }
    }
    public void RcWater()
    {
        foreach (var item in BuffsList)
        {
            //item.RcWater();
        }
    }
    public void RcWind()
    {
        foreach (var item in BuffsList)
        {
            //item.RcWind();
        }
    }
    public void RcThunder()
    {
        foreach (var item in BuffsList)
        {
            //item.RcThunder();
        }
    }
    public void RcEarth()
    {
        foreach (var item in BuffsList)
        {
            //item.RcEarth();
        }
    }
    public virtual void 战斗开始时()
    {
    }
    public virtual void 战斗结束时()
    {
    }
    public virtual void 回合开始时()
    {
    }
    public virtual void 回合结束时()
    {
        for (int i = 0; i < BuffsList.Count; i++)
        {
            BuffsList[i].OnTurnEnd();
        }
    }

    #endregion

    public void 行动结束()
    {
        Anim.Play("idle");
        动画播放完毕 = true;
    }

    public void 添加Buff(BuffsEnum BuffName)
    {
        Buff OldBuff = BuffsList.Find(b => b.Name == BuffName);
        if (OldBuff != null && OldBuff.IsStackable)
        {
                OldBuff.CurrStack += OldBuff.OnAddStack;
                OldBuff.层数改变时特效(OldBuff.OnAddStack);
                return;
        }

        if(OldBuff != null)
        {
            BuffsList.Remove(OldBuff);
        }

        switch (BuffName)
        {
            case BuffsEnum.燃烧:
                BuffsList.Add(new 燃烧(this));
                break;
            case BuffsEnum.盲目:
                BuffsList.Add(new 盲目(this));
                break;
            case BuffsEnum.出血:
                BuffsList.Add(new 出血(this));
                break;
            case BuffsEnum.麻痹:
                BuffsList.Add(new 麻痹(this));
                break;
            case BuffsEnum.减速:
                BuffsList.Add(new 减速(this));
                break;
            case BuffsEnum.中毒:
                BuffsList.Add(new 中毒(this));
                break;
            default:
                break;
        }
    }

    public void 临时添加血脉(魔力类型Enum type, int value)
    {
        if (Bloods.Exists(b => b.Name == type))
        {
            Bloods.Find(b => b.Name == type).Value += value;
        }
        else
        {
            Bloods.Add(new Blood(type, value));
        }
    }

    public void 永久添加血脉(魔力类型Enum type, int value) 
    {
        if(OriData. Bloods.Exists(b => b.Name == type))
        {
            OriData.Bloods.Find(b => b.Name == type).Value += value;
        }
        else
        {
            OriData.Bloods.Add(new Blood(type, value));
        }
    }

    public void 获取技能点() 
    {

        if (IsDead)
        {
            return;
        }

        if (BuffsList.Exists(b => b.Name == BuffsEnum.麻痹))
        {
            StatePoolMgr.Ins.状态(this,"麻痹 无法获取技能点");
            return;
        }

        SkillPoint += 1;
        SkillPointIcon[SkillPoint - 1].DOFade(1, 0);
        if (SkillPoint == SkillPointMax)
        {
            IsSkillReady = true;
        }
    }

    public void 获取护盾(float value)
    {
        护盾 += value;
        更新生命值();
    }
}

