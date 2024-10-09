using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Statue : MonoBehaviour
{
    public static Statue Ins;
    public int 
        StatueTotal = 3,
        RefreshCost;
    public int[] 
        StatueNum = new int[3],
        RefreshCount,
        LockedNum = new int[3];
    public List<int>
        RepeatNum = new(),
        CantRepeatNum = new();
    public TextMeshProUGUI RefeshCostTMP;
    public TextMeshProUGUI[]
        StatueTMPs = new TextMeshProUGUI[3];
    public Button PrayBtn,RefreshBtn;
    public Button[] LockBtns = new Button[3];
    public Image[] LockImgs = new Image[6];
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
        RefreshCount = new int[] { 1, 1, 1 };
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
        RefeshCostTMP.text = RefreshCost.ToString();
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
            RefreshBtn.enabled = false;
            RefreshBtn.GetComponent<Image>().color = Color.gray;
            //todo 把tmp也灰了
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
                    StatueTMPs[i].text = "转化1以太为1火元素";
                    break;
                case 1:
                    StatueTMPs[i].text = "转化3基础元素为2火元素";
                    break;
                case 2:
                    StatueTMPs[i].text = "获得1以太";
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
        for (int i = 0; i < EventsMgr.Ins.UnitDatas.Length; i++)
        {
            if (EventsMgr.Ins.UnitDatas[i].Name == "") continue;
            switch (StatueNum[i])
            {
                case 0:
                    ConvertFire(EventsMgr.Ins.UnitDatas[i]);
                    break;
                case 1:
                    ConvertRandomToFire(EventsMgr.Ins.UnitDatas[i]);
                    break;
                case 2:
                    AddBlood(EventsMgr.Ins.UnitDatas[i],1,"以太");
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
        while (UnFinish)
        {
            idx = Random.Range(0, unitData.Bloods.Count);

            if (unitData.Bloods[idx].Name == "火元素") continue;
            if(unitData.Bloods[idx].Level != 1) continue;
            unitData.Bloods[idx].Value -= 1;
            if (unitData.Bloods[idx].Value == 0)
                unitData.Bloods.RemoveAt(idx);
            Count++;
            if(Count == 5) UnFinish = false;
        }
        AddBlood(unitData, num, "火元素");
    }
}
