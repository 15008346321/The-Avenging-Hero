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
    public string 当前追打状态, EnemysStr;
    public bool isOurTurn, 正在战斗, 正在追打, isBattling, 战斗开始时特效已触发, 需要等待战斗开始协程;
    public Button BattleBtn;
    public Image[] PosSlots = new Image[9], CombDetailImgs = new Image[6];
    public TextMeshProUGUI[] CombDetailTMP = new TextMeshProUGUI[6];
    private Coroutine fadeCoroutine1 = null, fadeCoroutine2 = null;

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
    }

    public void InitTeam(bool 是否是玩家阵营 = true)
    {
        //获取小队数据或敌人数据
        List<UnitData> Datas;
        if (是否是玩家阵营)
        {
            Datas = TeamManager.Ins.TeamData;
        }
        else
        {
            //根据读到的敌人 先生成data再生成Unit
            var enemys = EnemysStr.Split('&');

            foreach (var item in enemys)
            {

                string Name = item.Split('P')[0];
                int Cell = int.Parse(item.Split('P')[1]);

                TeamManager.Ins.EnemyData.Add(new UnitData(CSVManager.Ins.Units[Name], Cell));
            }
            Datas = TeamManager.Ins.EnemyData;
        }
        //根据data实例化
        for (int i = 0; i < Datas.Count; i++)
        {
            InitRole(Datas[i], 是否是玩家阵营);
        }
        //如果是小队放到移动位置上
        if (是否是玩家阵营)
        {
            for (int i = 0; i < 玩家阵营单位列表.Count; i++)
            {
                玩家阵营单位列表[i].StartParent = OurRunningPos.GetChild(i);
                玩家阵营单位列表[i].transform.SetParent(玩家阵营单位列表[i].StartParent);
                玩家阵营单位列表[i].RunPosParent = 玩家阵营单位列表[i].transform.parent;
                玩家阵营单位列表[i].transform.localPosition = Vector2.zero;
            }
        }

    }

    public Unit InitRole(UnitData data, bool _该单位是否是玩家阵营)
    {
        string uname = data.Name;

        GameObject g = Resources.Load("Prefabs/Unit/" + uname) as GameObject;
        Unit u = Instantiate(g).transform.GetComponent<Unit>();
        u.name += u.GetInstanceID();
        u.OriData = data;//修改属性 存档时修改
        u.该单位是否是玩家阵营 = _该单位是否是玩家阵营;

        u.Cell = data.Cell;
        GameObject obj;
        if (_该单位是否是玩家阵营) obj = ourObj;
        else obj = eneObj;

        //找自己阵营有没空位
        if (查找敌对阵营指定位置上单位(u.Cell, !_该单位是否是玩家阵营) != null) 
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

        u.Init(data);//把data属性赋予unit
        //ourObj.transform.GetChild(u.Cell - 1)
        if (_该单位是否是玩家阵营)
        {
            u.TMP.tag = "Our";
            玩家阵营单位列表.Add(u);
        }
        else
        {
            u.transform.localScale = new Vector2(-1, 1);
            u.TMP.transform.localScale = new Vector2(-1, 1);
            u.HpBar.transform.localScale = new Vector2(-1, 1);
            u.ShieldBar.transform.localScale = new Vector2(-1, 1);
            u.SpeedTMP.transform.parent.localScale = new Vector2(-1, 1);
            敌人阵营单位列表.Add(u);
        }
        u.transform.localPosition = Vector2.zero;
        AllUnit.Add(u);
        return u;
    }

    public Unit SummonCreator(GameObject obj,bool IsEnemy, float ScaleRate)
    {
        var Summon = Instantiate(obj);
        Transform t = GetNoUnitSlot(IsEnemy);
        if (t == null)
        {
            return null;
        }

        Summon.transform.SetParent(GetNoUnitSlot(IsEnemy));
        Summon.transform.localPosition = Vector3.zero;
        Summon.transform.localScale *= ScaleRate;
        Unit u = Summon.GetComponent<Unit>();
        u.Cell = u.transform.parent.GetSiblingIndex() + 1;
        if (IsEnemy)
        {
            敌人阵营单位列表.Add(u);
        }
        else
        {
            玩家阵营单位列表.Add(u);
        }
        AllUnit.Add(u);
        
        SortBySpeed();
        return u;
    }


    public void TeamIdle()
    {
        foreach (var item in 玩家阵营单位列表)
        {
            item.Anim.Play("idle");
        }
    }

    public void TeamRun()
    {
        foreach (var item in 玩家阵营单位列表)
        {
            item.Anim.Play("run");
        }
    }

    public void TeamMoveToCell()
    {
        List<Vector2> v2 = new();
        foreach (var item in 玩家阵营单位列表)
        {
            item.Anim.enabled = false;
            item.transform.DOMove(ourObj.transform.GetChild(item.Cell - 1).position, 1f).OnComplete
                (
                    () =>
                    {
                        item.Anim.enabled = true;
                        item.Anim.Play("idle");
                        item.transform.SetParent(ourObj.transform.GetChild(item.Cell - 1));
                        Tips.SetActive(true);
                        SortBySpeed();
                        BattleBtn.gameObject.SetActive(true);
                    }
                );
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
        TeamMoveToCell();
    }

    //开始战斗
    public void OnBattleClick()
    {
        EventsMgr.Ins.IsMoveToBattle = false;
        isBattling = true;
        BattleBtn.enabled = false;
        Tips.SetActive(false);
        SetPosSlotAlpha(0);
        //SetHpBarActive();

        if (战斗开始时特效已触发 == false)
        {
            StartCoroutine(OnBattleStart_Coro(() => OnTurnStart()));
        }
        else
        {
            OnTurnStart();
        }
        //BattleBtn.enabled = false;
    }

    public IEnumerator OnBattleStart_Coro(System.Action onComplete)
    {
        战斗开始时特效已触发 = true;
        for (int i = 0; i < AllUnit.Count; i++)
        {
            print(AllUnit[i].name + " 战斗开始时");
            AllUnit[i].战斗开始时();
            yield return new WaitUntil(()=>AllUnit[i].动画播放完毕);
        }
        onComplete.Invoke();
    }

    public void OnTurnStart()
    {
        isBattling = true;

        for (int i = 0; i < AllUnit.Count; i++)
        {
            AllUnit[i].ReloadAtk();
            AllUnit[i].OnTurnStart();
        }
        FindNextActionUnit();
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
            AllUnit[i].HpBar.gameObject.SetActive(true);
            AllUnit[i].SpeedTMP.transform.parent.gameObject.SetActive(true);
            AllUnit[i].SpeedTMP.text = (i+1-DeadCount).ToString();
        }
    }

    public void FindNextActionUnit(float wait = 1f)
    {
        SortBySpeed();

        foreach (var item in AllUnit)
        {
            if (item.IsSkillReady && !item.IsDead)
            {
                //用ID遍历AllUnit找到对应的Unit调用普攻
                //AnimQueue.Add(item.ID + ":NormalAtk");

                print(item.name + "skill is ready");
                item.ExecuteSkill();
                return;
            }
        }
        foreach (var item in AllUnit)
        {
            if(item.AtkCountCurr > 0 && !item.IsDead)
            {
                //用ID遍历AllUnit找到对应的Unit调用普攻
                //AnimQueue.Add(item.ID + ":NormalAtk");
                item.AtkCountCurr -= 1;

                print( item.name + "atk");
                item.ExecuteAtk();
                return;
            }
        }

        OnTurnEnd();
        //StartCoroutine(PlayFirsrtAnimInQueue(wait));
    }

    //public IEnumerator PlayFirsrtAnimInQueue(float waitfor = 0)
    //{
    //    if (waitfor != 0)
    //    {
    //        yield return new WaitForSeconds(waitfor);
    //    }
    //    if (AnimQueue.Count == 0) 
    //    {
    //        BattleBtn.enabled = true;
    //        yield return new WaitForSeconds(1);
    //        OnTurnEnd();
    //        yield break;
    //    }
    //    TimeCout = 0;

    //    //判断是攻击追打被动

    //    string[] com = AnimQueue[0].Split(":");
    //    //死了的就不管
    //    if (IDUnitPiar[int.Parse(com[0])].isDead)
    //    {
    //        AnimQueue.RemoveAt(0);
    //        FindNextActionUnit();
    //    }
    //    else if (com[1] == "NormalAtk")
    //    {
    //        AnimQueue.RemoveAt(0);
    //        IDUnitPiar[int.Parse(com[0])].ExecuteAtk();
    //    }
    //    else if (com[1] == "Comb")
    //    {
    //        AnimQueue.RemoveAt(0);
    //        IDUnitPiar[int.Parse(com[0])].ExecuteComb();
    //    }
    //    //动画结束时会继续在最后一帧调用此方法
    //}

    public Unit GetTeamMinHealthUnit()
    {
        Ins.TeamMain.Sort((x, y) => x.Hp.CompareTo(y.Hp));
        Ins.TeamMain.Sort();
        return Ins.TeamMain[0];
    }

    public void ShowSkillName(Unit u,string SkillName)
    {
        // 如果正在执行fade协程，则停止它
        //if (!u.OnLeftSide)
        //{
        //    if (fadeCoroutine1 != null)
        //    {
        //        StopCoroutine(fadeCoroutine1);
        //    }
        //    // 开始新的fade协程  
        //    fadeCoroutine1 = StartCoroutine(FadeName(u, SkillName));
        //}
        //else
        //{
        //    if (fadeCoroutine2 != null)
        //    {
        //        StopCoroutine(fadeCoroutine2);
        //    }
        //    // 开始新的fade协程  
        //    fadeCoroutine2 = StartCoroutine(FadeName(u, SkillName));
        //}
        
    }

    public bool TrySuccess(float successRate)
    {
        // 生成一个0到1之间的随机数（不包括1）  
        float randomValue = Random.Range(0f, 1f);

        // 如果随机数小于或等于成功率，则返回true，表示成功  
        if (randomValue <= successRate)
        {
            return true;
        }
        // 否则返回false，表示失败  
        return false;
    }

    private IEnumerator FadeName(Unit u,string SkillName)
    {
        //CombDetailCount += 1;
        //Transform t;
        //if (!u.OnLeftSide) t = OurCombDetail;
        //else t = EneCombDetail;
        //// 设置alpha为1  
        //GameObject detail = Instantiate(CombDetailPrefab);
        //detail.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = SkillName + CombDetailCount;
        //detail.transform.SetParent(t);
        //detail.transform.localPosition = Vector2.zero;
        //detail.gameObject.SetActive(true);

        ////保持只有三个
        //if (t.childCount > 3)
        //{
        //    Destroy(t.GetChild(0).gameObject);
        //}
        //// 等待3秒  
        //yield return new WaitForSeconds(2f);

        //// 渐变到alpha为0
        //for (int i = 0; i < t.childCount -1; i++)
        //{
        //    t.GetChild(i).GetComponent<Image>().DOFade(0, 0.5f);
        //    t.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().DOFade(0, 0.5f);
        //    yield return new WaitForSeconds(0.5f);
        //}

        //for (int i = t.childCount -1 ; i >=0; i--)
        //{
        //    Destroy(t.GetChild(i).gameObject);
        //}
        //CombDetailCount = 0;

        yield return null;
    }

    public void 单位死亡时(bool 死亡单位是否是玩家阵营)
    {
        foreach (var item in AllUnit)
        {
            if (!item.IsDead)
            {
                item.单位死亡时(死亡单位是否是玩家阵营);
            }
        }
    }

    public IEnumerator CheckBattleEnd()
    {
        bool Wait = true;
        while (Wait)
        {
            yield return null;
            if (StatePoolMgr.Ins.transform.childCount == 10)
            {
                Wait = false;
            }
        }

        if (isBattling == false)
        {
            yield break;//防止同时死亡时重复判断
        }

        SortBySpeed();

        //胜利
        if (敌人阵营单位列表.All(item => item.IsDead == true))
        {
            EventsMgr.Ins.SetBonusGold(TeamManager.Ins.EnemyData.Count);
            EventsMgr.Ins.ShowBonus();
            ResetBattle();
            InitTeam();
            isBattling = false;
            BattleBtn.gameObject.SetActive(false);
            战斗结束时();
        }
        //失败
        else if(玩家阵营单位列表.All(item => item.IsDead == true))
        {
            EventsMgr.Ins.EventPoint -= 敌人阵营单位列表.Count;
            EventsMgr.Ins.EPTMP.text = EventsMgr.Ins.EventPoint.ToString();
            EventsMgr.Ins.ExploreBtn.gameObject.SetActive(true);
            InitTeam();
            isBattling = false;
            ResetBattle();
            BattleBtn.gameObject.SetActive(false);
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
    }


    public void OnTurnEnd()
    {
        for (int i = 0; i < AllUnit.Count; i++)
        {
            if(AllUnit[i].IsDead == true)continue;
            AllUnit[i].OnTurnEnd();
        }
        BattleBtn.enabled = true;
    }

    public void ResetBattle()
    {
        战斗开始时特效已触发 = false;
        TeamManager.Ins.EnemyData.Clear();
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
    public void SetHpBarActive()
    {
        foreach (var item in AllUnit)
        {
            item.HpBar.gameObject.SetActive(true);
        }
    }

    public Unit 查找敌对阵营指定位置上单位(int Cell,bool 调用者是否玩家阵营)
    {
        GameObject findIn;

        if (调用者是否玩家阵营) findIn = eneObj;
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

    public void SetDebuff(BuffsEnum debuff)
    {
        foreach (var item in Targets)
        {
            item.AddBuff(debuff);
        }
    }

    public Transform GetNoUnitSlot(bool isEnemy)
    {
        GameObject obj;
        if(isEnemy) obj = eneObj;
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

    public void 获取正前方目标(bool 调用者是否是玩家阵营,int Cell) 
    {
        Targets.Clear();

        GameObject obj;
        if (调用者是否是玩家阵营) obj = eneObj;
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

    public void 获取敌方单位最多的一行的所有目标(int cell, bool 是否是玩家阵营)
    {
        Targets.Clear();
        //判断每一行多少
        GameObject obj;
        if (是否是玩家阵营) obj = eneObj;
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

        print("max: " + max);
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

        print("row" + row);
        //把这一行加进目标
        for (int i = 0; i < 3; i++)
        {
            if(obj.transform.GetChild(i*3+row).childCount >0)
            {
                Targets.Add(obj.transform.GetChild(i * 3 + row).GetChild(0).GetComponent<Unit>());
            }
        }
    }

    public RowColumn 获取有单位的最前排(bool isEnemy)
    {
        GameObject obj;
        if (isEnemy) obj = ourObj;
        else obj = eneObj;

        for (int i = 0; i < 3; i++)
        {
            if (obj.transform.GetChild(i).childCount > 0)
            {
                return RowColumn.Clm1;
            }
        }
        for (int i = 4; i < 6; i++)
        {
            if (obj.transform.GetChild(i).childCount > 0)
            {
                return RowColumn.Clm2;
            }
        }
        for (int i = 7; i < 9; i++)
        {
            if (obj.transform.GetChild(i).childCount > 0)
            {
                return RowColumn.Clm3;
            }
        }
        return RowColumn.Clm1;
    }

    public void 获取指定行列所有目标(RowColumn rc,bool IsEnemy)
    {
        Targets.Clear();
        GameObject obj;
        if (IsEnemy) obj = ourObj;
        else obj = eneObj;

        switch (rc)
        {
            case RowColumn.Row1:
                for (int i = 0; i < 3; i++)
                {
                    Targets.Add(obj.transform.GetChild(i * 3).GetChild(0).GetComponent<Unit>());
                }
                break;
            case RowColumn.Row2:
                for (int i = 0; i < 3; i++)
                {
                    Targets.Add(obj.transform.GetChild(i * 3 + 1).GetChild(0).GetComponent<Unit>());
                }
                break;
            case RowColumn.Row3:
                for (int i = 0; i < 3; i++)
                {
                    Targets.Add(obj.transform.GetChild(i * 3 + 2).GetChild(0).GetComponent<Unit>());
                }
                break;
            case RowColumn.Clm1:
                for (int i = 0; i < 3; i++)
                {
                    Targets.Add(obj.transform.GetChild(i).GetChild(0).GetComponent<Unit>());
                }
                break;
            case RowColumn.Clm2:
                for (int i = 0; i < 3; i++)
                {
                    Targets.Add(obj.transform.GetChild(i+3).GetChild(0).GetComponent<Unit>());
                }
                break;
            case RowColumn.Clm3:
                for (int i = 0; i < 3; i++)
                {
                    Targets.Add(obj.transform.GetChild(i+6).GetChild(0).GetComponent<Unit>());
                }
                break;
            default:
                break;
        }

    }

    internal void 获取敌方阵营血量最低目标(bool 是否是玩家阵营)
    {
        Targets.Clear();
        List<Unit> Units;
        if (是否是玩家阵营) Units = 敌人阵营单位列表;
        else Units = 玩家阵营单位列表;

        Targets.Add(Units.Where(u=>u.IsDead == false).OrderBy(u => u.Hp).FirstOrDefault());
    }
}

public enum RowColumn
{
    Row1,
    Row2,
    Row3,
    Clm1,
    Clm2,
    Clm3,
}
