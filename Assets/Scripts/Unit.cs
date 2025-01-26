using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using System;
using TMPro;

[Serializable]
public class Unit : MonoBehaviour
{
    public int Cell, Damage, AtkCountMax, AtkCountCurr, SkillPoint, SkillPointMax, 伤害减免;
    public float 生命值, MaxHp, Atk, 护盾, Speed;
    public string Element;
    public bool isBoss, IsDead,该单位是否是玩家阵营,单位是否进行了移动, IsSkillReady,IsAtkChanged,动画播放完毕,伤害字体已消失;
    public List<Buff> BuffsList = new();
    public List<Blood> Bloods = new();
    public string[] Tags = new string[4];
    public 阵营Enum 阵营;
    public TextMeshProUGUI SpeedTMP,生命值TMP,护盾TMP;
    public Image Icon;
    public Button Btn;
    public List<Image> SkillPointIcon = new(), BuffListImgs = new();
    public Transform StartParent,伤害飘字父节点,RunPosParent,DragParent,BuffListImgNode,SkillPointImgNode;
    public RectTransform TMPNameNode;
    public EventSystem _EventSystem;
    public GraphicRaycaster gra;
    public UnitData OriData;
    public 技能基类 技能;
    public void Awake()
    {
        生命值TMP = transform.Find("Canvas/生命值/生命值TMP").GetComponent<TextMeshProUGUI>();
        护盾TMP = transform.Find("Canvas/护盾/护盾TMP").GetComponent<TextMeshProUGUI>();
        Icon = transform.Find("Canvas/Icon").GetComponent<Image>();
        BuffListImgNode = transform.Find("Canvas/BuffList");
        SkillPointImgNode = transform.Find("Canvas/SkillPointIcon");
        SpeedTMP = transform.Find("Canvas/Speed/TMP").GetComponent<TextMeshProUGUI>();
        //DragParent = GameObject.Find("Canvas/UI/DragParent").transform;
        伤害飘字父节点 = transform.Find("Canvas/FontPos");

        动画播放完毕 = true;
    }

    #region====初始化方法
    public void Init(UnitData data)
    {
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
        设置角色脚本(data.Name);
    }

    public void 设置角色脚本(string name)
    {
        技能 = name switch
        {
            "史莱姆勇者" => new 史莱姆勇者(),
            "小水"       => new 小水(),
            "黄金哥布林" => new 黄金哥布林(),
            "史莱姆" => new 史莱姆(),
            "哥布林" => new 哥布林(),
            "猫妖" => new 猫妖(),
            "魔兔" => new 魔兔(),
            "牛头人" => new 牛头人(),
            "猪头人" => new 猪头人(),
            _ => null,
        };

        技能?.Init(this);
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

    #region 动画

    public void 动画待机()
    {
        // 使用DOTween进行往返重复的缩放动画
        Icon.rectTransform.DOKill();
        Icon.rectTransform.localScale = Vector2.one;
        Icon.rectTransform.DOScale(new Vector2(1.05f, 0.95f), 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    public void 动画攻击()
    {
        var queue = DOTween.Sequence();
        var 阵营动画方向 = 阵营 == 阵营Enum.我方 ? 1 : -1;
        // 后移并改变scale
        queue.Append(transform.DOLocalMoveX(-30 * 阵营动画方向, 0.3f)).Join(transform.DOScale(new Vector2(0.9f * 阵营动画方向, 1.15f), 0.3f));
        // 前移并改变scale
        queue.Append(transform.DOLocalMoveX(100 * 阵营动画方向, 0.2f).OnComplete(技能.攻击帧)).Join(transform.DOScale(new Vector2(1.1f * 阵营动画方向, 0.9f), 0.2f));
        // 回到0点并恢复scale
        queue.Append(transform.DOLocalMoveX(0, 0.2f)).Join(transform.DOScale(new Vector2(1f * 阵营动画方向, 1f), 0.2f));
        queue.Play().OnComplete(()=>动画播放完毕 = true);
    }

    public void 动画技能()
    {
        var queue = DOTween.Sequence();
        queue.Append(transform.DOLocalMoveY(30f, 0.2f));
        queue.Append(transform.DOLocalMoveY(0f, 0.1f));
        queue.Append(transform.DOLocalMoveY(30f, 0.2f));
        queue.Append(transform.DOLocalMoveY(0f, 0.1f).OnComplete(技能.技能帧));
        queue.Play().OnComplete(() => 动画播放完毕 = true); ;
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
        transform.DOMove(obj.transform.GetChild(TargetCell - 1).position, 1f).OnComplete(
            () => 
            {
                transform.SetParent(obj.transform.GetChild(TargetCell - 1));
                Cell = TargetCell;
                动画播放完毕 = true;
            }
        );
    }

    public void 移动到目标同一列()
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


    public void CheckDeath()
    {
        print("CheckDeath");
        if (生命值 <= 0|| IsDead == true)
        {
            IsDead = true;
            技能.该单位阵亡时();

            BattleMgr.Ins.有其他单位阵亡时(阵营);
            BattleMgr.Ins.放回实例到对象池(this);
            BattleMgr.Ins.CheckBattleEnd();
        }
    }

    #endregion

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

