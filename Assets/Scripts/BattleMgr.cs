using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using DG.Tweening;
using UnityEngine.UI;

public class BattleMgr : MonoBehaviour
{
    public List<Unit> Team = new(), TeamMain = new(), Enemys = new(), EnemyMain = new(), AllUnit = new(), Targets = new();
    public List<string> AnimQueue = new();
    public float TimeCout, CurrTime;
    public Unit MainTarget;
    public GameObject HurtFont, ourObj, eneObj, Tips, CombDetailPrefab;
    public Transform OurCombDetail, EneCombDetail,OurRunningPos, EneRunningPos,DeadParent;
    public int currentDamage, AtkTotal,CombSkiIdx,IDCount,CombDetailCount;
    public string 当前追打状态, EnemysStr;
    public bool isOurTurn,正在战斗,正在追打,isBattling,isBattleStart;
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
        HurtFont = Resources.Load("Prefabs/HurtFont/Hurt") as GameObject;
    }

    public void InitTeam(bool IsEnemy = false)
    {

        List<UnitData> Datas;
        if (!IsEnemy)
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
                int Pos = int.Parse(item.Split('P')[1]);

                TeamManager.Ins.EnemyData.Add(new UnitData(CSVManager.Ins.Units[Name], Pos));
            }
            Datas = TeamManager.Ins.EnemyData;
        }

        for (int i = 0; i < Datas.Count; i++)
        {
            string uname = Datas[i].Name;

            GameObject g = Resources.Load("Prefabs/Unit/" + uname) as GameObject;
            Unit u = Instantiate(g).transform.GetComponent<Unit>();
            u.name =  u.name + u.GetInstanceID();
            u.OriData = Datas[i];//修改属性 存档时修改
            u.Cell = Datas[i].Cell;
            u.Init(Datas[i]);//把data属性赋予unit
            //ourObj.transform.GetChild(u.Cell - 1)
            if (!IsEnemy)
            {
                u.transform.SetParent(OurRunningPos.GetChild(i));
                u.RunPosParent = u.transform.parent;
                u.transform.localScale = new Vector2(1,1);
                u.IsEnemy = false;
                u.TMP.tag = "Our";
                Team.Add(u);
            }
            else
            {
                u.transform.SetParent(eneObj.transform.GetChild(u.Cell - 1));
                u.transform.localScale = new Vector2(-1,1);

                u.TMP.transform.localScale = new Vector2(-1,1);
                u.HpBar.transform.localScale = new Vector2(-1, 1);
                u.SpeedTMP.transform.parent.localScale = new Vector2(-1, 1);
                u.IsEnemy = true;
                Enemys.Add(u);
            }
            u.transform.localPosition = Vector2.zero;
            AllUnit.Add(u);
        }
    }
    
    public void TeamIdle()
    {
        foreach (var item in Team)
        {
            item.Animator.Play("idle");
        }
    }

    public void TeamRun()
    {
        foreach (var item in Team)
        {
            item.Animator.Play("run");
        }
    }

    public void TeamMoveToCell()
    {
        List<Vector2> v2 = new();
        foreach (var item in Team)
        {
            item.Animator.enabled = false;
            item.transform.DOMove(ourObj.transform.GetChild(item.Cell - 1).position, 1f).OnComplete
                (
                    () =>
                    {
                        item.Animator.enabled = true;
                        item.Animator.Play("idle");
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
        isBattling = true;
        EventsMgr.Ins.SetUnitCanDrag();
        Tips.SetActive(false);
        SetPosSlotAlpha(0);
        //SetHpBarActive();
        if (isBattleStart == false)
        {
            //InitUnitAttr();
            OnBattleStart();
        }
        OnTurnStart();
        //BattleBtn.enabled = false;
    }

    public void OnBattleStart()
    {
        isBattleStart = true;
        foreach (var item in AllUnit)
        {
            item.OnBattleStart();
        }
    }

    public void OnTurnStart()
    {
        isBattling = true;
        foreach (var item in AllUnit)
        {
            item.ReloadAtk();
        }
        FindNextActionUnit();
    }

    public void SortBySpeed()
    {
        Team.Sort((x, y) => x.Speed.CompareTo(y.Speed));
        Enemys.Sort((x, y) => x.Speed.CompareTo(y.Speed));
        AllUnit.Clear();
        AllUnit.AddRange(Team);
        AllUnit.AddRange(Enemys);
        AllUnit.Sort((u1, u2) => u2.Speed.CompareTo(u1.Speed));
        int DeadCount = 0;
        for (int i = 0; i < AllUnit.Count; i++)
        {
            if (AllUnit[i].isDead) 
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
        var count = 0;
        foreach (var item in AllUnit)
        {
            if(item.AtkCountCurr > 0 && !item.isDead)
            {
                //用ID遍历AllUnit找到对应的Unit调用普攻
                //AnimQueue.Add(item.ID + ":NormalAtk");
                item.AtkCountCurr -= 1;
                item.ExecuteAtk();
                break;
            }
            count++;
        }

        if(count == AllUnit.Count)
        {
            StartCoroutine(TurnEnd());
        }
        //PlayFirsrtAnimInQueue会判断没有行动 则回合结束
        //StartCoroutine(PlayFirsrtAnimInQueue(wait));
    }

    public IEnumerator TurnEnd()
    {
        yield return OnTurnEnd();
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
        if (!u.IsEnemy)
        {
            if (fadeCoroutine1 != null)
            {
                StopCoroutine(fadeCoroutine1);
            }
            // 开始新的fade协程  
            fadeCoroutine1 = StartCoroutine(FadeName(u, SkillName));
        }
        else
        {
            if (fadeCoroutine2 != null)
            {
                StopCoroutine(fadeCoroutine2);
            }
            // 开始新的fade协程  
            fadeCoroutine2 = StartCoroutine(FadeName(u, SkillName));
        }
        
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
        CombDetailCount += 1;
        Transform t;
        if (!u.IsEnemy) t = OurCombDetail;
        else t = EneCombDetail;
        // 设置alpha为1  
        GameObject detail = Instantiate(CombDetailPrefab);
        detail.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = SkillName + CombDetailCount;
        detail.transform.SetParent(t);
        detail.transform.localPosition = Vector2.zero;
        detail.gameObject.SetActive(true);

        //保持只有三个
        if (t.childCount > 3)
        {
            Destroy(t.GetChild(0).gameObject);
        }
        // 等待3秒  
        yield return new WaitForSeconds(2f);

        // 渐变到alpha为0
        for (int i = 0; i < t.childCount -1; i++)
        {
            t.GetChild(i).GetComponent<Image>().DOFade(0, 0.5f);
            t.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().DOFade(0, 0.5f);
            yield return new WaitForSeconds(0.5f);
        }

        for (int i = t.childCount -1 ; i >=0; i--)
        {
            Destroy(t.GetChild(i).gameObject);
        }
        CombDetailCount = 0;
    }

    public void CheckBattleEnd()
    {
        if (isBattling == false) return;//防止同时死亡时重复判断
        //胜利
        if (Enemys.All(item => item.isDead == true))
        {
            EventsMgr.Ins.ShowBonus();
            ResetBattle();
            InitTeam(false);
            isBattling = false;
            BattleBtn.gameObject.SetActive(false);
        }
        //失败
        else if(Team.All(item => item.isDead == true))
        {
            EventsMgr.Ins.EventPoint -= Enemys.Count;
            EventsMgr.Ins.EPTMP.text = EventsMgr.Ins.EventPoint.ToString();
            EventsMgr.Ins.ExploreBtn.gameObject.SetActive(true);
            InitTeam(false);
            isBattling = false;
            ResetBattle();
            BattleBtn.gameObject.SetActive(false);
        }
    }

    public IEnumerator OnTurnEnd()
    {
        for (int i = 0; i < AllUnit.Count; i++)
        {
            AllUnit[i].OnTurnEnd();
        }
        yield return null;
    }

    public void ResetBattle()
    {
        isBattleStart = false;
        TeamManager.Ins.EnemyData.Clear();
        AllUnit.Clear();
        Team.Clear();

        Enemys.Clear();
        AnimQueue.Clear();
        for (int i = 0; i < ourObj.transform.childCount - 1; i++)
        {
            if (ourObj.transform.GetChild(i).childCount > 0)
            {
                Destroy(ourObj.transform.GetChild(i).GetChild(0).gameObject);
            }
        }
        for (int i = 0; i < eneObj.transform.childCount - 1; i++)
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

    public Unit FindUnitOnCell(int Cell,bool IsEnemy)
    {
        GameObject side;

        if (!IsEnemy) side = ourObj;
        else side = eneObj;

        if(Cell > 9)
        {
            return null;
        }

        if(side.transform.GetChild(Cell-1).childCount > 0)
        return side.transform.GetChild (Cell-1).GetChild(0).GetChild(0).GetComponent<Unit>();

        return null;
    }

    public void CaculDamage(float damage)
    {
        foreach (var item in Targets)
        {
            item.TakeDamage(damage);
        }
    }
}
