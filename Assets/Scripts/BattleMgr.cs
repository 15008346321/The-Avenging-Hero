using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using DG.Tweening;
using UnityEngine.UI;

public class BattleMgr : MonoBehaviour
{
    public List<Unit> 玩家阵营单位列表 = new(), TeamMain = new(), 敌人阵营单位列表 = new(), EnemyMain = new(), AllUnit = new(), Targets = new();
    public List<int> TopRow = new() { 1, 4, 7 }, MidRow = new() { 2, 5, 8 }, BotRow = new() { 3, 6, 9 }, Col1 = new() { 1, 2, 3 }, Col2 = new() { 4, 5, 6 }, Col3 = new() { 7, 8, 9 };
    public List<string> AnimQueue = new();
    public float TimeCout, CurrTime;
    public GameObject ourObj, eneObj, Tips, CombDetailPrefab;
    public Transform OurCombDetail, EneCombDetail,OurRunningPos, EneRunningPos,DeadParent;
    public int currentDamage, AtkTotal,CombSkiIdx,IDCount,CombDetailCount;
    public string 当前追打状态;
    public string[] EnemysStr;
    public bool isOurTurn, 正在战斗, 正在追打, 战斗中, 战斗开始时, 需要等待战斗开始协程,当前正在布阵,战斗胜利;
    public Button BattleBtn;
    public Image[] PosSlots = new Image[9], CombDetailImgs = new Image[6];
    public TextMeshProUGUI[] CombDetailTMP = new TextMeshProUGUI[6];

    public static BattleMgr Ins;
    private void Awake()
    {
        if (Ins == null) Ins = this;
        else Destroy(Ins);
    }

    private void Start()
    {
        Init();
    }
    void Update()
    {
        TimeCout += Time.deltaTime;
    }
    public void Init()
    {
        BattleBtn.onClick.AddListener(OnBattleClick);
        战斗开始时 = true;
    }

    public void 实例化敌人小队()
    {
        实例化小队(阵营Enum.敌方);
    }

    public void 实例化小队(阵营Enum _阵营 = 阵营Enum.我方)
    {
        //获取小队数据或敌人数据
        List<UnitData> Datas;
        if (_阵营 == 阵营Enum.我方)
        {
            Datas = TeamMgr.Ins.TeamData;
        }
        else
        {
            //根据读到的敌人 先生成data再生成Unit

            var enemys = EnemysStr.Skip(2).ToArray();

            foreach (var item in enemys)
            {
                if (item == "") continue;

                string Name = item.Split(':')[0];
                int Cell = int.Parse(item.Split(':')[1]);

                TeamMgr.Ins.EnemyData.Add(new UnitData(CSVMgr.Ins.Units[Name], Cell));
            }
            Datas = TeamMgr.Ins.EnemyData;
        }
        //根据data实例化
        for (int i = 0; i < Datas.Count; i++)
        {
            InitRole(Datas[i], _阵营);
        }
    }

    public Unit InitRole(UnitData data, 阵营Enum _阵营 = 阵营Enum.我方, bool 加入背包 = false)
    {
        string uname = data.Name;
        if (data.SkillDscrp[0] == "") uname = "Unit";
        GameObject g = Resources.Load("Prefabs/Unit/" + uname) as GameObject;
        
        Unit u = Instantiate(g).transform.GetComponent<Unit>();
        u.name += u.GetInstanceID();
        u.OriData = data;//修改属性 存档时修改
        u.阵营 = _阵营;

        u.Cell = data.Cell;
        u.Init(data);//把data属性赋予unit

        //购买或其他途径获得之后放进背包
        if (加入背包) 
        { 
            //TODO加入背包的功能
        }

        GameObject obj;
        if (_阵营 == 阵营Enum.我方) obj = ourObj;
        else obj = eneObj;

        //找自己阵营有没空位
        if (查找指定阵营位置上单位(u.阵营,u.Cell) != null) 
        {
            for (int i = 0; i < 9; i++)
            {
                if(obj.transform.GetChild(i).childCount == 0)
                {
                    u.Cell = i + 1;
                    u.transform.SetParent(obj.transform.GetChild(i));
                    break;
                }
            }
        }
        else
        {
            u.transform.SetParent(obj.transform.GetChild(u.Cell-1));
        }

        //ourObj.transform.GetChild(u.Cell - 1)
        if (u.阵营 == 阵营Enum.我方)
        {
            u.Icon.tag = "Our";
            玩家阵营单位列表.Add(u);
        }
        else
        {
            u.transform.localScale = new Vector2(-1, 1);
            u.生命值TMP.transform.localScale = new Vector2(-1, 1);
            u.护盾TMP.transform.localScale = new Vector2(-1, 1);
            u.BuffListImgNode.localScale = new Vector2(-1, 1);
            u.SpeedTMP.transform.parent.localScale = new Vector2(-1, 1);
            敌人阵营单位列表.Add(u);
        }
        u.transform.localPosition = Vector2.zero;
        AllUnit.Add(u);
        return u;
    }

    public void TeamRun()
    {
        foreach (var item in 玩家阵营单位列表)
        {
            item.Anim.Play("run");
        }
    }

    public void SetPosSlotAlpha(float alpha)
    {
        foreach (var item in PosSlots)
        {
            item.color = new Color(1, 1, 1, alpha);
        }
    }

    public void EnterBattle()
    {
        ourObj.SetActive(true);
        eneObj.SetActive(true);
        //TeamMoveToCell();
        实例化敌人小队();
        当前正在布阵 = true;
        UIMgr.Ins.显示角色栏();
        SetPosSlotAlpha(1f);
        BattleBtn.gameObject.SetActive(true);
        UIMgr.Ins.UIBot.SetActive(false);
    }

    //开始战斗
    public void OnBattleClick()
    {
        BattleBtn.enabled = false;
        StartCoroutine(OnBattleClick_Coro());
    }

    public IEnumerator OnBattleClick_Coro()
    {
        EventsMgr.Ins.是否去往战斗 = false;
        战斗中 = true;
        BattleBtn.enabled = false;
        Tips.SetActive(false);
        UIMgr.Ins.收起角色栏();
        UIMgr.Ins.隐藏小队();
        当前正在布阵 = false;
        SetPosSlotAlpha(0);
        //SetHpBarActive();

        if (战斗开始时 == true)
        {
            yield return StartCoroutine(OnBattleStart_Coro());
        }
        yield return StartCoroutine(OnTurnStart_Coro());
    }

    public IEnumerator OnBattleStart_Coro()
    {
        实例化小队();

        for (int i = 0; i < AllUnit.Count; i++)
        {
            AllUnit[i].战斗开始时();
            yield return new WaitUntil(() => AllUnit[i].动画播放完毕 && StatePoolMgr.Ins.提示信息父节点.childCount == 10);
        }
        战斗开始时 = false;
    }

    public IEnumerator OnTurnStart_Coro()
    {
        战斗中 = true;

        for (int i = 0; i < AllUnit.Count; i++)
        {
            AllUnit[i].ReloadAtk();
            AllUnit[i].回合开始时();
            yield return new WaitUntil(() => AllUnit[i].动画播放完毕 && StatePoolMgr.Ins.提示信息父节点.childCount == 10);
        }

        StartCoroutine(FindNextActionUnit());
    }

    public void SortBySpeed()
    {
        玩家阵营单位列表.Sort((x, y) => x.Speed.CompareTo(y.Speed));
        敌人阵营单位列表.Sort((x, y) => x.Speed.CompareTo(y.Speed));
        AllUnit.Clear();
        AllUnit.AddRange(玩家阵营单位列表);
        AllUnit.AddRange(敌人阵营单位列表);
        AllUnit.Sort((u1, u2) => u2.Speed.CompareTo(u1.Speed));
        int DeadCount = 0;
        for (int i = 0; i < AllUnit.Count; i++)
        {
            if (AllUnit[i].IsDead) 
            {
                DeadCount ++;
                continue;
            }
            //AllUnit[i].SpeedTMP.transform.parent.gameObject.SetActive(true);
            //AllUnit[i].SpeedTMP.text = (i+1-DeadCount).ToString();
        }
    }

    public IEnumerator FindNextActionUnit()
    {
        //如果有单位攻击次数或技能并且单位存活
        while (AllUnit.Find(u => ((u.AtkCountCurr > 0) ||  u.IsSkillReady) && !u.IsDead)) 
        {
            if (战斗中 == false)
            {
                yield break;
            };

            SortBySpeed();

            for (int i = 0; i < AllUnit.Count; i++)
            {
                if (AllUnit[i].IsSkillReady && !AllUnit[i].IsDead)
                {
                    //用ID遍历AllUnit找到对应的Unit调用普攻
                    //AnimQueue.Add(item.ID + ":NormalAtk");

                    AllUnit[i].ExecuteSkill();
                    yield return new WaitUntil(() => AllUnit[i].动画播放完毕 && StatePoolMgr.Ins.提示信息父节点.childCount == 10);
                    break;
                }
            }

            for (int i = 0; i < AllUnit.Count; i++)
            {
                if (AllUnit[i].AtkCountCurr > 0 && !AllUnit[i].IsDead)
                {
                    //用ID遍历AllUnit找到对应的Unit调用普攻
                    //AnimQueue.Add(item.ID + ":NormalAtk");
                    AllUnit[i].AtkCountCurr -= 1;

                    AllUnit[i].ExecuteAtk();
                    yield return new WaitUntil(() => AllUnit[i].动画播放完毕 && StatePoolMgr.Ins.提示信息父节点.childCount == 10);
                    break;
                }
            }

        }

        StartCoroutine( OnTurnEnd_Coro());
    }

    public void 有单位阵亡时(阵营Enum _阵营)
    {
        foreach (var item in AllUnit)
        {
            if (!item.IsDead)
            {
                item.有单位阵亡时(_阵营);
            }
        }
    }

    public IEnumerator CheckBattleEnd()
    {
        bool Wait = true;
        while (Wait)
        {
            yield return null;
            if (StatePoolMgr.Ins.提示信息父节点.childCount == 10)
            {
                Wait = false;
            }
        }

        if (战斗中 == false)
        {
            yield break;//防止同时死亡时重复判断
        }

        SortBySpeed();

        //胜利
        if (敌人阵营单位列表.All(item => item.IsDead == true))
        {
            战斗中 = false;
            战斗胜利 = true;
        }
        //失败
        else if(玩家阵营单位列表.All(item => item.IsDead == true))
        {
            EventsMgr.Ins.EventPoint -= 敌人阵营单位列表.Count;
            EventsMgr.Ins.EPTMP.text = EventsMgr.Ins.EventPoint.ToString();
            战斗中 = false;
            战斗胜利 = false;

        }

        if(战斗中 == false)
        {
            战斗结束时();
        }
    }

    public void 战斗结束时() 
    {
        for (int i = 0; i < AllUnit.Count; i++)
        {
            if (AllUnit[i].IsDead == true) continue;
            AllUnit[i].战斗结束时();
        }
        ResetBattle();

        BattleBtn.gameObject.SetActive(false);
        BattleBtn.enabled = true;



        if (战斗胜利)
        {
            UIMgr.Ins.显示小队();
            EventsMgr.Ins.显示战利品();
            神像管理器.Ins.显示神像();
        }
        else
        {
            //战斗失败
        }

    }

    public IEnumerator OnTurnEnd_Coro()
    {
        for (int i = 0; i < AllUnit.Count; i++)
        {
            if(AllUnit[i].IsDead == true)continue;
            AllUnit[i].回合结束时();
            yield return new WaitUntil(() => AllUnit[i].动画播放完毕 && StatePoolMgr.Ins.提示信息父节点.childCount == 10);
        }
        for (int i = 0; i < BagMgr.Ins.遗物基类List.Count; i++)
        {
            BagMgr.Ins.遗物基类List[i].回合结束时();
        }
        BattleBtn.enabled = true;
    }

    public void ResetBattle()
    {
        StopCoroutine("FindNextActionUnit");
        战斗开始时 = true;
        TeamMgr.Ins.EnemyData.Clear();
        AllUnit.Clear();
        玩家阵营单位列表.Clear();

        敌人阵营单位列表.Clear();
        AnimQueue.Clear();
        for (int i = 0; i < ourObj.transform.childCount; i++)
        {
            if (ourObj.transform.GetChild(i).childCount > 0)
            {
                Destroy(ourObj.transform.GetChild(i).GetChild(0).gameObject);
            }
        }
        for (int i = 0; i < eneObj.transform.childCount; i++)
        {
            if (eneObj.transform.GetChild(i).childCount > 0)
            {
                Destroy(eneObj.transform.GetChild(i).GetChild(0).gameObject);
            }
        }
    }

    public Unit GetTeamMinHealthUnit()
    {
        Ins.TeamMain.Sort((x, y) => x.生命值.CompareTo(y.生命值));
        Ins.TeamMain.Sort();
        return Ins.TeamMain[0];
    }

    public Unit 查找指定阵营位置上单位(阵营Enum _阵营, int Cell)
    {
        GameObject findIn;

        if (_阵营 == 阵营Enum.敌方) findIn = eneObj;
        else findIn = ourObj;

        if(Cell > 9)
        {
            return null;
        }

        if(findIn.transform.GetChild(Cell-1).childCount > 0)
        {
            return findIn.transform.GetChild(Cell - 1).GetChild(0).GetComponent<Unit>();
        }

        return null;
    }

    public void CaculDamage(float damage)
    {
        foreach (var item in Targets)
        {
            item.TakeDamage(damage);
        }
    }

    public void Heal(float damage)
    {
        foreach (var item in Targets)
        { 
            item.TakeDamage(damage); 
        }
    }

    public void 对目标群体加buff(BuffsEnum debuff)
    {
        foreach (var item in Targets)
        {
            item.添加Buff(debuff);
        }
    }

    public Transform GetNoUnitSlot(阵营Enum _阵营)
    {
        GameObject obj;
        if(_阵营 == 阵营Enum.敌方) obj = eneObj;
        else obj = ourObj;

        for (int i = 0; i < obj.transform.childCount; i++)
        {
            if(obj.transform.GetChild(i).childCount == 0)
            {
                return obj.transform.GetChild(i);
            }
        }
        return null;
    }

    public void 获取正前方目标(阵营Enum _阵营, int Cell) 
    {
        Targets.Clear();

        GameObject obj;
        if (_阵营 == 阵营Enum.我方) obj = eneObj;
        else obj = ourObj;


        // 根据Cell的值确定要搜索的行  
        List<int> row;
        if (TopRow.Contains(Cell)) row = TopRow;
        else if (MidRow.Contains(Cell)) row = MidRow;
        else row = BotRow;

        // 遍历行索引  
        foreach (var item in row)
        {
            if (obj.transform.GetChild(item - 1).childCount > 0)
            {
                if (obj.transform.GetChild(item - 1).GetChild(0).TryGetComponent<Unit>(out Unit u))
                {
                    Targets.Add(u);
                    return;
                }
            }
        }
    }

    public void 获取敌方单位最多的一行的所有目标(int cell, 阵营Enum _阵营)
    {
        Targets.Clear();
        //判断每一行多少
        GameObject obj;
        if (_阵营 == 阵营Enum.我方) obj = eneObj;
        else obj = ourObj;
        Dictionary<int, int> record = new();

        for (int i = 0; i < 3; i++)
        {
            record[i] = 0;
        }

        for (int i = 0; i < TopRow.Count; i++)
        {
            record[0] += obj.transform.GetChild(TopRow[i]-1).childCount;
        }
        for (int i = 0; i < MidRow.Count; i++)
        {
            record[1] += obj.transform.GetChild(MidRow[i]-1).childCount;
        }
        for (int i = 0; i < BotRow.Count; i++)
        {
            record[2] += obj.transform.GetChild(BotRow[i] - 1).childCount;
        }
        //判断最多并且最近的一行
        int max = record.Max(pair=>pair.Value);

        int PlayerRow = (cell - 1) / 3;
        int row = 0;
        for (int i = 0; i < record.Count; i++)
        {
            if (record[PlayerRow] == max) 
            {
                row = PlayerRow;
                break;
            }

            if (PlayerRow == 0)
                if (record[1] == max) row = 1;
                else row = 2;
            else if (PlayerRow == 1)
            {
                if (record[0] == max) row = 0;
                else row = 2;
            }
            else if (PlayerRow == 2)
            {
                if (record[1] == max) row = 1;
                else row = 0;
            }
        }

        //把这一行加进目标
        for (int i = 0; i < 3; i++)
        {
            if(obj.transform.GetChild(i*3+row).childCount >0)
            {
                Targets.Add(obj.transform.GetChild(i * 3 + row).GetChild(0).GetComponent<Unit>());
            }
        }
    }

    public 行列Enum 获取有单位的最前排(阵营Enum _阵营)
    {
        GameObject obj;
        if (_阵营 == 阵营Enum.我方) obj = ourObj;
        else obj = eneObj;

        for (int i = 0; i < 3; i++)
        {
            if (obj.transform.GetChild(i).childCount > 0)
            {
                return 行列Enum.列1;
            }
        }
        for (int i = 4; i < 6; i++)
        {
            if (obj.transform.GetChild(i).childCount > 0)
            {
                return 行列Enum.列2;
            }
        }
        for (int i = 7; i < 9; i++)
        {
            if (obj.transform.GetChild(i).childCount > 0)
            {
                return 行列Enum.列3;
            }
        }
        return 行列Enum.列1;
    }

    public void 获取敌方指定行列所有目标(行列Enum rc, 阵营Enum _阵营)
    {
        Targets.Clear();
        GameObject obj;
        if (_阵营 == 阵营Enum.我方) obj = eneObj;
        else obj = ourObj;

        switch (rc)
        {
            case 行列Enum.行1:
                for (int i = 0; i < 3; i++)
                {
                    if (obj.transform.GetChild(i * 3).childCount > 0)
                    {
                        Targets.Add(obj.transform.GetChild(i * 3).GetChild(0).GetComponent<Unit>());
                    }
                }
                break;
            case 行列Enum.行2:
                for (int i = 0; i < 3; i++)
                {
                    if (obj.transform.GetChild(i * 3+1).childCount > 0)
                    {
                        Targets.Add(obj.transform.GetChild(i * 3+1).GetChild(0).GetComponent<Unit>());
                    }
                }
                break;
            case 行列Enum.行3:
                for (int i = 0; i < 3; i++)
                {
                    if (obj.transform.GetChild(i * 3+2).childCount > 0)
                    {
                        Targets.Add(obj.transform.GetChild(i * 3+2).GetChild(0).GetComponent<Unit>());
                    }
                }
                break;
            case 行列Enum.列1:
                for (int i = 0; i < 3; i++)
                {
                    if(obj.transform.GetChild(i).childCount > 0)
                    {
                        Targets.Add(obj.transform.GetChild(i).GetChild(0).GetComponent<Unit>());
                    }
                }
                break;
            case 行列Enum.列2:
                for (int i = 0; i < 3; i++)
                {
                    if (obj.transform.GetChild(i+3).childCount > 0)
                    {
                        Targets.Add(obj.transform.GetChild(i+3).GetChild(0).GetComponent<Unit>());
                    }
                }
                break;
            case 行列Enum.列3:
                for (int i = 0; i < 3; i++)
                {
                    if (obj.transform.GetChild(i+6).childCount > 0)
                    {
                        Targets.Add(obj.transform.GetChild(i+6).GetChild(0).GetComponent<Unit>());
                    }
                }
                break;
            default:
                break;
        }

    }

    internal void 获取敌方阵营血量最低目标(阵营Enum _阵营)
    {
        Targets.Clear();
        List<Unit> Units;
        if (_阵营 == 阵营Enum.我方) Units = 敌人阵营单位列表;
        else Units = 玩家阵营单位列表;

        Targets.Add(Units.Where(u=>u.IsDead == false).OrderBy(u => u.生命值).FirstOrDefault());
    }

    public void 获取敌方前排单位(阵营Enum _阵营) 
    {
        获取敌方指定行列所有目标(行列Enum.列1, _阵营);
        if(Targets.Count == 0)
        {
            获取敌方指定行列所有目标(行列Enum.列2, _阵营);
        }
        if (Targets.Count == 0)
        {
            获取敌方指定行列所有目标(行列Enum.列3, _阵营);
        }
    }
}
