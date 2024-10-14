using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Statue : MonoBehaviour
{
    public static Statue Ins;
    public List<bool> StatueRequireResult = new();
    public int 
        StatueTotal = 3,
        RefreshCost;
    public int[] 
        StatueNum = new int[3],
        LockedNum = new int[3];
    public List<int>
        RepeatNum = new(),
        CantRepeatNum = new();
    public TextMeshProUGUI RefreshCostTMP;
    public TextMeshProUGUI[]
        StatueTMPs = new TextMeshProUGUI[3];
    public Button PrayBtn,RefreshBtn;
    public Button[] LockBtns = new Button[3];
    public Image[] LockImgs = new Image[6];
    public GameObject RefreshBlock;
    public GameObject[] PrayPosBlock = new GameObject[3];
    public List<Transform> PrayPos = new();
    public Dictionary<int,UnitData> PrayUnitDatas = new();

    private void Awake()
    {
        if (Ins == null) Ins = this;
        else Destroy(Ins);

        LockBtns[0].onClick.AddListener(()=> LockStatue(0));
        LockBtns[1].onClick.AddListener(()=> LockStatue(1));
        LockBtns[2].onClick.AddListener(()=> LockStatue(2));
    }

    public void SetRandomStatue()
    {
        StatueNum[0] = GetNonRepeatRandomNum();
        StatueNum[1] = GetNonRepeatRandomNum();
        StatueNum[2] = GetNonRepeatRandomNum();
        SetStatueTMPs();
        LockedNum = new int[] { 0,0,0 };
        for (int i = 0; i < LockImgs.Length; i++)
        {
            if (i%2 == 0)
            {
                LockImgs[i].enabled = true;
            }
            else
            {
                LockImgs[i].enabled = false;
            }
        }
        RefreshBlock.SetActive(false);
        RefreshCost = 0;
        RefreshCostTMP.text = RefreshCost.ToString();
        PrayBtn.enabled = true;
    }

    public int GetNonRepeatRandomNum()
    {
        bool ContinueWhile = true;
        var ran = 0;
        while (ContinueWhile)
        {
            ran = Random.Range(0, StatueTotal);
            if (!RepeatNum.Contains(ran))
            {
                ContinueWhile = false;
                RepeatNum.Add(ran);
            }
        }
        return ran;
    }

    public void LockStatue(int num)
    {
        LockedNum[num] += 1;

        //三个都锁上就激活block
        var LockCount = 0;
        foreach (var item in LockedNum)
        {
            if(item%2 == 1)LockCount++;
            if(LockCount == 3)
            {
                RefreshBlock.SetActive(true);
            }
            else
            {
                if (EventsMgr.Ins.Gold >= RefreshCost)
                {
                    RefreshBlock.SetActive(false);
                }
            }
            print(LockCount);
        }

        if (LockedNum[num]%2 == 0)
        {
            LockImgs[num*2].enabled = true;
            LockImgs[num*2+1].enabled = false;
        }
        else
        {
            LockImgs[num * 2].enabled = false;
            LockImgs[num * 2 + 1].enabled = true;
        }
    }

    public void RefreshStatue()
    {
        RefreshCost += 1;
        RefreshCostTMP.text = RefreshCost.ToString();
        for (int i = 0; i < LockedNum.Length; i++)
        { 
            if (LockedNum[i] % 2 == 0)
            {
                var cout = 0;
                bool continueWhile = true;
                int num = StatueNum[i];
                RepeatNum.Remove(StatueNum[i]);
                while(continueWhile)
                {
                    StatueNum[i] = Random.Range(0, StatueTotal);
                    if (num != StatueNum[i])
                    {
                        continueWhile = false;
                    }
                    cout += 1;
                    if (cout > 100) break;
                }
                SetStatueTMPs();
            }
        }
        EventsMgr.Ins.Gold -= RefreshCost;
        EventsMgr.Ins.UIGoldTMP.text = EventsMgr.Ins.Gold.ToString();
        if (EventsMgr.Ins.Gold < RefreshCost)
        {
            RefreshBlock.SetActive(true);
        }
        else
        {
            RefreshBlock.SetActive(false);
        }
    }

    // Start is called before the first frame update
    //交流情报与任务接取
    //魔法物品交易会
    //竞技挑战与角斗赛
    //神秘事件调查
    //文化交流与节日庆典
    //遭遇敌对势力或盗贼团伙
    public void SetStatueTMPs()
    {
        for (int i = 0; i < StatueTMPs.Length; i++)
        {
            switch (StatueNum[i])
            {
                case 0:
                    StatueTMPs[i].text = "转化1<sprite=\"元素\" name=\"以太\">以太为1<sprite=\"元素\" name=\"火元素\">火元素";
                    break;
                case 1:
                    StatueTMPs[i].text = "转化3基础元素为2<sprite=\"元素\" name=\"火元素\">火元素";
                    break;
                case 2:
                    StatueTMPs[i].text = "获得1<sprite=\"元素\" name=\"以太\">以太";
                    break;
                default:
                    break;
            }
        }
    }

    //按钮绑定调用
    public void Pray()
    {
        PrayBtn.enabled = false;
        for (int i = 0; i < PrayUnitDatas.Count; i++)
        {
            if (PrayUnitDatas[i].Name == "") continue;
            switch (StatueNum[i])
            {
                case 0:
                    ConvertFire(PrayUnitDatas[i]);
                    break;
                case 1:
                    ConvertRandomToFire(PrayUnitDatas[i]);
                    break;
                case 2:
                    AddBlood(PrayUnitDatas[i],1,"以太");
                    break;
                default:
                    break;
            }
        }
        RepeatNum.Clear();
        //展示属性变化
        //隐藏
        //出探索按钮
        foreach (var item in BattleMgr.Ins.Team)
        {
            item.transform.SetParent(item.RunPosParent);
            item.transform.localPosition = Vector2.zero;
        }
        EventsMgr.Ins.IsMoveToStatue = false;
        gameObject.SetActive(false);
        EventsMgr.Ins.ExploreBtn.gameObject.SetActive(true);
    }

    public void RequireCheck(Unit u)
    {
        StatueRequireResult.Clear();
        for (int i = 0; i < 3; i++)
        {
            StatueRequireResult.Add(true);
        }
        foreach (var item in PrayPosBlock)
        {
            item.SetActive(false);
        }

        print("StatueNum.Length " + StatueNum.Length);
        for (int i = 0; i < StatueNum.Length; i++)
        {
            switch (StatueNum[i])
            {
                case 0:

                    print("statue0");
                    if(!u.Bloods.Any(item=>item.Name == "以太" && item.Value > 0))
                     {
                        StatueRequireResult[i] = false;
                    }
                    break;
                case 1:
                    print("statue1");
                    print("");
                    if (!(u.Bloods.Where(item => item.Name != "火元素" && item.Level == 1).Sum(item=>item.Value)>= 3f))
                    {

                        print("火元素不足");
                        StatueRequireResult[i] = false;
                    }
                    break;
                default:
                    break;
            }
        }

        for (int i = 0; i < StatueRequireResult.Count; i++)
        {
            print(i + StatueRequireResult[i].ToString());

            if (StatueRequireResult[i] == false)
            {
                PrayPosBlock[i].SetActive(true);

                print(i + "激活");
            }
        }
    }

    void AddBlood(UnitData unitData, int num, string type)
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

    void ConvertYitai(UnitData unitData, int num, string type)
    {
        unitData.Bloods.Find(item => item.Name == "以太").Value -= num;
        AddBlood(unitData, num, type);
    }
    void ConvertFire(UnitData unitData,int num = 1)
    {
        ConvertYitai(unitData, num,"火元素");
    }

    void ConvertRandomToFire(UnitData unitData, int num = 2)
    {
        bool UnFinish = true;
        int Count = 0;
        var idx = 0;
        var breakcount = 0;
        while (UnFinish)
        {
            idx = Random.Range(0, unitData.Bloods.Count);
            //breakcount++;
            //if (breakcount == 100) break;
            if (unitData.Bloods[idx].Name == "火元素") continue;
            if(unitData.Bloods[idx].Level != 1) continue;
            unitData.Bloods[idx].Value -= 1;
            if (unitData.Bloods[idx].Value == 0)
                unitData.Bloods.RemoveAt(idx);
            Count++;
            if(Count == 3) UnFinish = false;
        }
        AddBlood(unitData, num, "火元素");
    }
}
