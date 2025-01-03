using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class 神像管理器 : MonoBehaviour
{
    public static 神像管理器 Ins;
    public int 
        StatueTotal = 3,
        剩余刷新次数;
    public int[]
        神像位置对应的神像编号 = new int[3];
    public bool 当前正在神像;
    public List<int>
        当前出现了的神像编号 = new(),
        不会在出现的神像编号 = new();
    public TextMeshProUGUI RefreshCostTMP;
    public TextMeshProUGUI[]
        StatueTMPs = new TextMeshProUGUI[3];
    public Button 神像按钮, PrayBtn;
    public Button[] 刷新按钮列表;
    public GameObject 神像节点,RefreshBlock;
    public List<Transform> PrayPos = new();
    public List<Image> PrayPosImgs;

    private void Awake()
    {
        if (Ins == null) Ins = this;
        else Destroy(Ins);

        刷新按钮列表[0].onClick.AddListener(()=> 刷新当前神像(0));
        刷新按钮列表[1].onClick.AddListener(()=> 刷新当前神像(1));
        刷新按钮列表[2].onClick.AddListener(()=> 刷新当前神像(2));
    }

    public void 显示神像()
    {
        神像按钮.gameObject.SetActive(true);
        神像按钮.transform.parent.SetAsLastSibling();
    }

    public void 隐藏神像()
    {
        神像按钮.gameObject.SetActive(false);
    }

    public void 当点击神像()
    {
        神像节点.SetActive(true);
        当前正在神像 = true;
        剩余刷新次数 = 3;
        设置三个神像();
        UIMgr.Ins.角色栏.SetActive(true);
        UIMgr.Ins.角色栏.transform.SetAsLastSibling();
    }

    public void 设置三个神像()
    {
        神像位置对应的神像编号[0] = 获取一个不重复的神像编号();
        神像位置对应的神像编号[1] = 获取一个不重复的神像编号();
        神像位置对应的神像编号[2] = 获取一个不重复的神像编号();
        设置所有神像描述();
        重置神像下角色图片();
        PrayBtn.enabled = true;
    }

    public void 重置神像下角色图片()
    {
        for (int i = 0; i < PrayPosImgs.Count; i++)
        {
            PrayPosImgs[i].gameObject.SetActive(false);
            PrayPosImgs[i].sprite = null;
        }
    }

    public int 获取一个不重复的神像编号()
    {
        bool ContinueWhile = true;
        var ran = 0;
        while (ContinueWhile)
        {
            ran = Random.Range(0, StatueTotal);
            if (!当前出现了的神像编号.Contains(ran))
            {
                ContinueWhile = false;
                当前出现了的神像编号.Add(ran);
            }
        }
        return ran;
    }

    public void 刷新当前神像(int num)
    {
        剩余刷新次数 -= 1;
        if (剩余刷新次数 <= 0) 
        { 
            //按钮弄黑并且enable false
        }

        神像位置对应的神像编号[num] = 获取一个不重复的神像编号();
        设置所有神像描述();
    }

    //public void RefreshStatue()
    //{
    //    剩余刷新次数 += 1;
    //    RefreshCostTMP.text = 剩余刷新次数.ToString();
    //    for (int i = 0; i < LockedNum.Length; i++)
    //    { 
    //        //没有锁的格子
    //        if (LockedNum[i] % 2 == 0)
    //        {
    //            //把单位放回去
    //            if (PrayPos[i].childCount != 0)
    //            {
    //                Unit u = PrayPos[i].GetChild(0).GetComponent<Unit>();
    //                u.transform.SetParent(u.StartParent);
    //                u.transform.localPosition = Vector2.zero;
    //            }
    //            var TestCout = 0;
    //            bool continueWhile = true;
    //            //记录当前神像 刷个不一样的
    //            int num = StatueNum[i];
    //            当前出现了的神像编号.Remove(StatueNum[i]);
    //            while(continueWhile)
    //            {
    //                StatueNum[i] = Random.Range(0, StatueTotal);
    //                if (num != StatueNum[i])
    //                {
    //                    continueWhile = false;
    //                }
    //                TestCout += 1;
    //                if (TestCout > 100) break;
    //            }
    //            设置所有神像描述();
    //        }
    //    }

    //    RequireCheck();

    //    //刷新花钱
    //    BagMgr.Ins.玩家拥有的金币 -= 剩余刷新次数;
    //    EventsMgr.Ins.UIGoldTMP.text = BagMgr.Ins.玩家拥有的金币.ToString();
    //    //没钱不能刷新
    //    if (BagMgr.Ins.玩家拥有的金币 < 剩余刷新次数)
    //    {
    //        RefreshBlock.SetActive(true);
    //    }
    //    else
    //    {
    //        RefreshBlock.SetActive(false);
    //    }
    //}

    // Start is called before the first frame update
    //交流情报与任务接取
    //魔法物品交易会
    //竞技挑战与角斗赛
    //神秘事件调查
    //文化交流与节日庆典
    //遭遇敌对势力或盗贼团伙
    public void 设置所有神像描述()
    {
        for (int i = 0; i < StatueTMPs.Length; i++)
        {
            switch (神像位置对应的神像编号[i])
            {
                case 0:
                    StatueTMPs[i].text = "转化1以太为1火元素";
                    break;
                case 1:
                    StatueTMPs[i].text = "转化3以太为2火元素";
                    break;
                case 2:
                    StatueTMPs[i].text = "获得2最大生命值";
                    break;
                case 3:
                    StatueTMPs[i].text = "获得1攻击力";
                    break;
                case 4:
                    StatueTMPs[i].text = "获得3金币";
                    break;
                default:
                    break;
            }
        }
    }

    //按钮绑定调用
    public void 当点击信仰()
    {
        PrayBtn.enabled = false;
        for (int i = 0; i < TeamMgr.Ins.拥有角色数据.Count; i++)
        {
            var ud = TeamMgr.Ins.拥有角色数据[i];
            if (ud.在神像位置 == -1) continue;
            switch (神像位置对应的神像编号[ud.在神像位置])
            {
                case 0:
                    ConvertYitai(ud, 1, 魔力类型Enum.火元素, 1);
                    break;
                case 1:
                    ConvertYitai(ud, 3, 魔力类型Enum.火元素, 2);
                    break;
                case 2:
                    ud.MaxHp += 2;
                    break;
                case 3:
                    ud.Atk += 1;
                    break;
                case 4:
                    EventsMgr.Ins.ShowGetBonus(2);
                    break;
                default:
                    break;
            }
        }
        当前出现了的神像编号.Clear();
        //展示属性变化
        //隐藏
        //出探索按钮
        神像节点.SetActive(false);
        UIMgr.Ins.收起角色栏();
        UIMgr.Ins.显示小队();
        当前正在神像 = false;
        隐藏神像();
        EventsMgr.Ins.生成一个战斗路线();

    }

#region ====具体神像效果====
    void AddBlood(UnitData unitData, int num, 魔力类型Enum type)
    {
        if (unitData.Bloods.Exists(item=>item.Name == type))
        {
            unitData.Bloods.Find(item => item.Name == type).Value += num;
        }
        else
        {
            unitData.Bloods.Add(new Blood(type,num));
        }
    }

    void ConvertYitai(UnitData unitData, int 要消耗的以太数目, 魔力类型Enum type, int Tnum)
    {
        var 已有以太数目 = unitData.Bloods.Find(item => item.Name == 魔力类型Enum.以太)?.Value;
        if (已有以太数目 == null)
        {
            AddBlood(unitData, 1, 魔力类型Enum.以太);
        }
        else if(已有以太数目 < 要消耗的以太数目) 
        {
            unitData.Bloods.Find(item => item.Name == 魔力类型Enum.以太).Value += 1;
        }
        else
        {
            unitData.Bloods.Find(item => item.Name == 魔力类型Enum.以太).Value -= 要消耗的以太数目;
            AddBlood(unitData, Tnum, type);
        }
    }
    void ConvertRandomToFire(UnitData unitData, int num = 2)
    {
        bool UnFinish = true;
        int Count = 0;
        var idx = 0;
        while (UnFinish)
        {
            idx = Random.Range(0, unitData.Bloods.Count);
            //breakcount++;
            //if (breakcount == 100) break;
            if (unitData.Bloods[idx].Name == 魔力类型Enum.火元素) continue;
            if(unitData.Bloods[idx].Level != 1) continue;
            unitData.Bloods[idx].Value -= 1;
            if (unitData.Bloods[idx].Value == 0)
                unitData.Bloods.RemoveAt(idx);
            Count++;
            if(Count == 3) UnFinish = false;
        }
        AddBlood(unitData, num, 魔力类型Enum.火元素);
    }
#endregion
}
