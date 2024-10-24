using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using System;
using TMPro;


public class Unit : MonoBehaviour, IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public int Cell,Damage,AtkCountMax,AtkCountCurr,SkillPoint,SkillPointMax;
    public float Hp, MaxHp, Atk, Shield, Speed;
    public string Element;
    public bool isBoss, isDead,IsEnemy,IsEnterUnitMove, isSkillTriggered, isSkillReady;
    public List<Buff> Buffs = new();
    public List<Blood> Bloods = new();
    public string[] Tags = new string[4];

    public TextMeshProUGUI TMP,SpeedTMP;
    public Animator Anim;
    public Image HpBar, Icon,ClickImage;
    public Button Btn;
    public GameObject ClickBlock;
    public List<Image> SkillPointIcon = new();
    public Transform StartParent,StatePos,RunPosParent,DragParent;
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
        HpBar = transform.Find("Canvas/HpBar").GetComponent<Image>();
        TMP = transform.Find("Canvas/TMP").GetComponent<TextMeshProUGUI>();
        SpeedTMP = transform.Find("Canvas/Speed/TMP").GetComponent<TextMeshProUGUI>();
        Anim = transform.GetComponent<Animator>();
        DragParent = GameObject.Find("Canvas/UI/DragParent").transform;
        StatePos = transform.Find("Canvas/FontPos");
        Btn = transform.Find("Canvas/Click").GetComponent<Button>();
        Btn.onClick.AddListener(OnClick);
        ClickImage = transform.Find("Canvas/Click").GetComponent<Image>();
        ClickImage.enabled = false;
        ClickBlock = transform.Find("Canvas/ClickBlock").gameObject;
        ClickBlock.SetActive(false);
    }

    #region====初始化方法
    public void Init(UnitData data)
    {
        Hp = MaxHp = data.MaxHp;
        Atk = data.Atk;
        Speed = data.Speed;
        AtkCountMax = 1;
        Bloods = data.Bloods;
        SkillPointMax = data.SkillPointMax;
        InitSkillPointIcon();
    }

    public void InitSkillPointIcon()
    {
        if (SkillPointMax == 0)
        {
            transform.Find("Canvas/SkillPointIcon").gameObject.SetActive(false);
            return;
        }

        for (int i = 0; i < transform.Find("Canvas/SkillPointIcon").childCount; i++)
        {
            if (i >= SkillPointMax)
            {
                transform.Find("Canvas/SkillPointIcon").GetChild(i).gameObject.SetActive(false);
            }
            else
            {
                SkillPointIcon.Add(transform.Find("Canvas/SkillPointIcon").GetChild(i).GetComponent<Image>());
                SkillPointIcon[i].DOFade(0.5f, 0);
            }
        }
        transform.Find("Canvas/SkillPointIcon").GetComponent<HorizontalLayoutGroup>().spacing = (transform.Find("Canvas/SkillPointIcon").childCount - SkillPointMax) * -12.5f;
    }
   
    #endregion
    #region ====拖动方法
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (EventsMgr.Ins.IsMoveToBattle)
        {
            StartParent = transform.parent;
            transform.SetParent(DragParent);
            BattleMgr.Ins.SetPosSlotAlpha(0.4f);
        }
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (EventsMgr.Ins.IsMoveToBattle)
        {
            Anim.enabled = false;
            TMP.raycastTarget = false;
            //100是Canvas.planeDistance
            Vector3 globalMousePos = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, 100));
            // 设置对象的位置  
            transform.position = globalMousePos;
            //神像时
            if (EventsMgr.Ins.IsMoveToStatue)
            {
            }
            //战斗时
            else
            {
                BattleMgr.Ins.SetPosSlotAlpha(0.4f);
                if (eventData.pointerEnter.CompareTag("Pos"))
                {
                    eventData.pointerEnter.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                }
            }

            //用tmp判断 路劲结构:slot/Unit/canvas/tmp
            if (eventData.pointerEnter.transform.parent.parent.CompareTag("Our"))
            {
                //设置slot的alpha
                eventData.pointerEnter.transform.parent.parent.parent.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
        }
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        if (EventsMgr.Ins.IsMoveToBattle)
        {
            BattleMgr.Ins.SetPosSlotAlpha(0f);

            print(eventData.pointerEnter.tag);
            if (eventData.pointerEnter.CompareTag("Pos"))
            {
                transform.SetParent(eventData.pointerEnter.transform);
                OriData.Cell = Cell = int.Parse(transform.parent.name);
                transform.localPosition = Vector2.zero;
            }
            else if (eventData.pointerEnter.CompareTag("Our"))
            {
                transform.parent = eventData.pointerEnter.transform.parent.parent.parent;
                eventData.pointerEnter.transform.parent.parent.parent = StartParent;
                transform.localPosition = Vector2.zero;
                eventData.pointerEnter.transform.parent.parent.transform.localPosition = Vector2.zero;

                Cell = transform.parent.GetSiblingIndex()+1;
                eventData.pointerEnter.transform.parent.parent.GetComponent<Unit>().Cell = eventData.pointerEnter.transform.parent.parent.parent.GetSiblingIndex()+1;
            }
            else
            {
                transform.SetParent(StartParent);
                transform.localPosition = Vector2.zero;
            }
            TMP.raycastTarget = true;
            Anim.enabled = true;
        }
    }

    public void OnClick()
    {
        if (EventsMgr.Ins.IsMoveToStatue)
        {
            if (transform.parent.CompareTag("RunPos"))
            {
                for (int i = 0; i < Statue.Ins.PrayPos.Count; i++)
                {
                    print(i + " " + Statue.Ins.PrayPos[i].name );
                    if (Statue.Ins.PrayPos[i].childCount == 0)
                    {
                        transform.SetParent(Statue.Ins.PrayPos[i]);
                        transform.localPosition = Vector2.zero;
                        Statue.Ins.PrayUnitDatas[i] = OriData;
                        break;
                    }
                }
            }
            else if (transform.parent.CompareTag("PrayPos"))
            {
                Statue.Ins.PrayUnitDatas.Remove(transform.parent.GetSiblingIndex());
                transform.SetParent(StartParent);
                transform.localPosition = Vector2.zero;
            }
            Statue.Ins.RequireCheck();
        }
    }

    private void ChangePos(Unit unit1, Unit unit2)
    {
        (unit2.Cell, unit1.Cell) = (unit1.Cell, unit2.Cell);
        (unit2.OriData.Cell, unit1.OriData.Cell) = (unit1.OriData.Cell, unit2.OriData.Cell);
    }

    private List<RaycastResult> GraphicRaycaster(Vector2 pos)
    {
        var mPointerEventData = new PointerEventData(_EventSystem);

        mPointerEventData.position = pos;

        List<RaycastResult> results = new();

        gra.Raycast(mPointerEventData, results);

        return results;
    }
    #endregion
    #region ====战斗中移动
    public void UnitMove(int TargetCell)
    {
        IsEnterUnitMove = true;
        GameObject Our;
        GameObject Ene;
        if (!IsEnemy)
        {
            Our = BattleMgr.Ins.ourObj;
            Ene = BattleMgr.Ins.eneObj;
        }
        else
        {
            Our = BattleMgr.Ins.eneObj;
            Ene = BattleMgr.Ins.ourObj;
        }
        Anim.enabled = false;
        transform.DOMove(Our.transform.GetChild(TargetCell - 1).position, 1f).OnComplete(
            () => 
            {
                transform.SetParent(Our.transform.GetChild(TargetCell - 1));
                Cell = TargetCell;
                Anim.enabled = true;
                BattleMgr.Ins.FindNextActionUnit();
            }
        );
    }

    internal void MoveToEnemyFrontRow()
    {
        IsEnterUnitMove = false;
        GameObject Our;
        GameObject Ene;
        if (!IsEnemy)
        {
            Our = BattleMgr.Ins.ourObj;
            Ene = BattleMgr.Ins.eneObj;
        }
        else
        {
            Our = BattleMgr.Ins.eneObj;
            Ene = BattleMgr.Ins.ourObj;
        }
        //GetChild(Cell) 1位置移动到2 2cell的index是1
        if (Cell == 1)
        {
            //2有空去2
            if (Our.transform.GetChild(1).childCount == 0)
            {
                UnitMove(2);
            }
            //4有空去4
            else if (Our.transform.GetChild(3).childCount == 0)
            {
                UnitMove(4);
            }
        }
        else if (Cell == 3)
        {
            //2有空去2
            if (Our.transform.GetChild(1).childCount == 0)
            {
                UnitMove(2);
            }
            //6有空去6
            else if (Our.transform.GetChild(5).childCount == 0)
            {
                UnitMove(6);
            }
        }
        else if (Cell == 7)
        {
            //8有空去8
            if (Our.transform.GetChild(7).childCount == 0)
            {
                UnitMove(8);
            }
            //4有空去4
            else if (Our.transform.GetChild(3).childCount == 0)
            {
                UnitMove(4);
            }
        }
        else if (Cell == 9)
        {
            //8有空去8
            if (Our.transform.GetChild(7).childCount == 0)
            {
                UnitMove(8);
            }
            //6有空去6
            else if (Our.transform.GetChild(5).childCount == 0)
            {
                UnitMove(6);
            }
        }
        else if (Cell == 4)
        {
            //5有空去5
            if (Our.transform.GetChild(4).childCount == 0)
            {
                UnitMove(5);
            }
            //12有空 移动到1
            else if (Our.transform.GetChild(0).childCount == 0 && Our.transform.GetChild(1).childCount == 0)
            {
                UnitMove(1);
            }
            //78有空 移动到7
            else if (Our.transform.GetChild(6).childCount == 0 && Our.transform.GetChild(7).childCount == 0)
            {
                UnitMove(7);
            }
        }
        else if (Cell == 6)
        {
            //5有空去5
            if (Our.transform.GetChild(4).childCount == 0)
            {
                UnitMove(5);
            }
            //23有空 移动到3
            if (Our.transform.GetChild(2).childCount == 0 && Our.transform.GetChild(1).childCount == 0)
            {
                UnitMove(3);
            }
            //89有空 移动到9
            else if (Our.transform.GetChild(8).childCount == 0 && Our.transform.GetChild(7).childCount == 0)
            {
                UnitMove(9);
            }
        }
        else if (Cell == 2)
        {
            //1有空 上路有敌人去1
            if (Our.transform.GetChild(0).childCount == 0 
                && (Ene.transform.GetChild(0).childCount > 0 || Ene.transform.GetChild(3).childCount > 0 || Ene.transform.GetChild(6).childCount > 0))
            {
                UnitMove(1);
            }
            //3有空下路有敌人 去3
            else if (Our.transform.GetChild(2).childCount == 0 
                && (Ene.transform.GetChild(2).childCount > 0 || Ene.transform.GetChild(5).childCount > 0 || Ene.transform.GetChild(8).childCount > 0))
            {
                UnitMove(3);
            }
            //上面都不满足 5有空去5
            else if (Our.transform.GetChild(4).childCount == 0)
            {
                UnitMove(5);
            }
        }
        else if (Cell == 8)
        {
            //7有空 上路有敌人去7
            if (Our.transform.GetChild(6).childCount == 0
                && (Ene.transform.GetChild(0).childCount > 0 || Ene.transform.GetChild(3).childCount > 0 || Ene.transform.GetChild(6).childCount > 0))
            {
                UnitMove(7);
            }
            //9有空下路有敌人 去9
            else if (Our.transform.GetChild(8).childCount == 0
                && (Ene.transform.GetChild(2).childCount > 0 || Ene.transform.GetChild(5).childCount > 0 || Ene.transform.GetChild(8).childCount > 0))
            {
                UnitMove(9);
            }
            //上面都不满足 5有空去5
            else if (Our.transform.GetChild(4).childCount == 0)
            {
                UnitMove(5);
            }
        }
        else if (Cell == 5)
        {
            //4有空 上路有敌人去4
            if (Our.transform.GetChild(3).childCount == 0 && (Ene.transform.GetChild(0).childCount > 0 || Ene.transform.GetChild(3).childCount > 0 || Ene.transform.GetChild(6).childCount > 0))
            {
                UnitMove(4);
            }
            //6有空 下路有敌人去6
            else if (Our.transform.GetChild(5).childCount == 0 && (Ene.transform.GetChild(2).childCount > 0 || Ene.transform.GetChild(5).childCount > 0 || Ene.transform.GetChild(8).childCount > 0))
            {
                UnitMove(6);
            }
            //4没空 上路有敌人 12有空 去2
            else if (Our.transform.GetChild(3).childCount > 0 && Our.transform.GetChild(0).childCount == 0 && Our.transform.GetChild(1).childCount == 0
                &&(Ene.transform.GetChild(0).childCount > 0 || Ene.transform.GetChild(3).childCount > 0 || Ene.transform.GetChild(6).childCount > 0))
            {
                UnitMove(2);
            }
            //4没空 上路有敌人 78有空 去8
            else if (Our.transform.GetChild(3).childCount > 0 && Our.transform.GetChild(8).childCount == 0 && Our.transform.GetChild(7).childCount == 0
                && (Ene.transform.GetChild(0).childCount > 0 || Ene.transform.GetChild(3).childCount > 0 || Ene.transform.GetChild(6).childCount > 0))
            {
                UnitMove(8);
            }
            //6没空 下路有敌人 23有空 去2
            else if (Our.transform.GetChild(5).childCount > 0 && Our.transform.GetChild(2).childCount == 0 && Our.transform.GetChild(1).childCount == 0
                && (Ene.transform.GetChild(2).childCount > 0 || Ene.transform.GetChild(5).childCount > 0 || Ene.transform.GetChild(8).childCount > 0))
            {
                UnitMove(2);
            }
            //6没空 下路有敌人 89有空 去8
            else if (Our.transform.GetChild(5).childCount > 0 && Our.transform.GetChild(8).childCount == 0 && Our.transform.GetChild(7).childCount == 0
                && (Ene.transform.GetChild(2).childCount > 0 || Ene.transform.GetChild(5).childCount > 0 || Ene.transform.GetChild(8).childCount > 0))
            {
                UnitMove(8);
            }
        }
        if (!IsEnterUnitMove)
        {
            FindNextActionUnit();
        }

    }
    #endregion
    #region====流程方法
    public void ReloadAtk()
    {
        AtkCountCurr = AtkCountMax;
    }
    public void FindNextActionUnit()
    {
        BattleMgr.Ins.FindNextActionUnit();
    }
    #endregion
    #region====单位通用方法


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
    public void UpdateHpBar()
    {
        //c# int 1/int 3 = 0
        HpBar.fillAmount = Hp / MaxHp;
    }
    
    public void ShowSkillName(string SkillName)
    {
        BattleMgr.Ins.ShowSkillName(this,SkillName);
    }
    
    #endregion
#region====战斗方法
    public virtual void ExecuteAtk()
    {

        print(name + "ExecuteAtk");
        Buff 盲目 = Buffs.Find(item => item.Name == BuffsEnum.盲目);
        if (盲目 != null)
        {
            StatePoolMgr.Ins.状态(this, "盲目-行动失败");
            盲目.层数减少(1);
            BattleMgr.Ins.FindNextActionUnit();
            return;
        }

        BattleMgr.Ins.获取正前方目标(IsEnemy, Cell);
        if (BattleMgr.Ins.Targets.Count == 0)//没有目标就走位 然后进行下一个
        {
            MoveToEnemyFrontRow();
        }
        else
        {
            Anim.Play("atk");//后面两方法在动画帧后段调用
        }
    }

    //特殊攻击目标需要重写 默认正前方 
    public virtual void 获取攻击目标()
    {
        BattleMgr.Ins.获取正前方目标(IsEnemy,Cell);
    }

    //如果攻击有特殊逻辑则重写 动画上调用
    public virtual void 攻击帧()
    {
        BattleMgr.Ins.CaculDamage(Atk);
        攻击特效();
    }

    //攻击时特效重写实现(获取技能点 附加伤害...)
    public virtual void 攻击特效()
    {
        //
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
        isSkillReady = false;

        //动画中会执行 技能帧();
        Anim.Play("skill", 0, 0);
    }

    //需要重写 默认正前方目标 
    public virtual void 获取技能目标() 
    {
        BattleMgr.Ins.获取正前方目标(IsEnemy, Cell);
    }
    public virtual void 技能帧()
    {
    }
    ////输出的追打伤害
    //public void CaculDamageOnComb()
    //{
    //    Comb.CombTargets();
    //}
    ////在动画帧攻击之后调用 加追打时特效(Debuff 追打养成等)
    //public void AddAtkEffectOnComb()
    //{
    //    Comb.OnComb();
    //}
    //AttrType:Atk/Fire/Water/Wind/Thunder/Earth  AtkType:Atk/Comb
    public void TakeDamage(float Damage,DamageType damageType = DamageType.物理伤害)
    {

        print(name + "takle damage");
        Hp -= Damage;
        UpdateHpBar();
        CheckDeath();
        //float damageReduce = 1, damageRate = 1;
        switch (damageType)
        {
            case DamageType.物理伤害:
                StatePoolMgr.Ins.物理伤害(this, Damage);
                break;
            case DamageType.火元素伤害:
                StatePoolMgr.Ins.火元素伤害(this, Damage);
                break;
            case DamageType.燃烧伤害:
                StatePoolMgr.Ins.燃烧伤害(this, Damage);
                break;
        }
        受到攻击时();
        //switch (damageType)
        //{
        //    case "物理伤害":
        //        StatePoolMgr.Ins.物理伤害(this, Damage);
        //        break;
        //    case "火元素伤害":
        //        StatePoolMgr.Ins.火元素伤害(this, Damage);
        //        break;
        //    case "燃烧伤害":
        //        StatePoolMgr.Ins.燃烧伤害(this, Damage);
        //        break;
        //case "Water":
        //    damageReduce = DamageFrom.Water / (DamageFrom.Water + Water);
        //    if (Element == "火") damageRate = 1.5f;
        //    Damage = Mathf.RoundToInt(DamageFrom.Water * damageReduce * damageRate);
        //    break;
        //case "Wind":
        //    damageReduce = DamageFrom.Wind / (DamageFrom.Wind + Wind);
        //    if (Element == "土") damageRate = 1.5f;
        //    Damage = Mathf.RoundToInt(DamageFrom.Wind * damageReduce * damageRate);
        //    break;
        //case "Thunder":
        //    damageReduce = DamageFrom.Thunder / (DamageFrom.Thunder + Thunder);
        //    if (Element == "水") damageRate = 1.5f;
        //    Damage = Mathf.RoundToInt(DamageFrom.Thunder * damageReduce * damageRate);
        //    break;
        //case "Earth":
        //    damageReduce = DamageFrom.Earth / (DamageFrom.Earth + Earth);
        //    if (Element == "雷") damageRate = 1.5f;
        //    Damage = Mathf.RoundToInt(DamageFrom.Earth * damageReduce * damageRate);
        //    break;
        //default:
        //    damageReduce = DamageFrom.Atk / (DamageFrom.Atk + Atk);
        //    Damage = Mathf.RoundToInt(DamageFrom.Atk * damageReduce);
        //    break;
        //}
        //Damage = Mathf.RoundToInt(Damage * rate);
        //if(AtkType == "Atk") RcAtk();
        //if(AtkType == "Comb") RcComb();
        //if (AttrType == "Atk") RcAtk();
        //if (AttrType == "Fire") RcFire();
        //if (AttrType == "Water") RcComb();
        //if (AttrType == "Wind") RcAtk();
        //if (AttrType == "Thunder") RcComb();
        //if (AttrType == "Earth") RcComb();

        //BattleMgr.Ins.ShowFont(this, Damage, "Hurt");
    }//被攻击时特效 减伤 养成

    public void TakeHeal(float HealValue)
    {
        Hp += HealValue;
        StatePoolMgr.Ins.治疗(this, HealValue);
        if (Hp >= MaxHp)
        {
            Hp = MaxHp;
        }
        UpdateHpBar() ;
        受到治疗时();
    }
    public void CheckDeath()
    {
        if (Hp <= 0)
        {
            isDead = true;
            transform.SetParent(BattleMgr.Ins.DeadParent);
            BattleMgr.Ins.CheckBattleEnd();
            if (BattleMgr.Ins.isBattling == false) return;
            BattleMgr.Ins.SortBySpeed();
        }
    }

    public virtual void 受到攻击时()
    {
        foreach (var item in Buffs)
        {
            //item.RcAtk();
        }
    }

    public virtual void 受到治疗时()
    {

    }

    public void RcComb()
    {
        foreach (var item in Buffs)
        {
            //item.RcComb();
        }
    }
    public void RcPhs()
    {
        foreach (var item in Buffs)
        {
            //item.RcPhs();
        }
    }
    public void RcFire()
    {
        foreach (var item in Buffs)
        {
            //item.RcFire();
        }
    }
    public void RcWater()
    {
        foreach (var item in Buffs)
        {
            //item.RcWater();
        }
    }
    public void RcWind()
    {
        foreach (var item in Buffs)
        {
            //item.RcWind();
        }
    }
    public void RcThunder()
    {
        foreach (var item in Buffs)
        {
            //item.RcThunder();
        }
    }
    public void RcEarth()
    {
        foreach (var item in Buffs)
        {
            //item.RcEarth();
        }
    }
    public void OnBattleStart()
    {
    }
    public void OnTurnEnd()
    {
        for (int i = 0; i < Buffs.Count; i++)
        {
            Buffs[i].OnTurnEnd();
        }
    }

    public virtual void OnTurnStart()
    {

    }
    #endregion

    public void 行动结束()
    {
        Anim.Play("idle");
        BattleMgr.Ins.FindNextActionUnit();
    }

    public void AddBuff(BuffsEnum buff)
    {
        switch (buff)
        {
            case BuffsEnum.燃烧:
                Buffs.Add(new 燃烧(this));
                break;
            case BuffsEnum.盲目:
                Buffs.Add(new 盲目(this));
                break;
            default:
                break;
        }
    }
}