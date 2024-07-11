using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using DG.Tweening;
using UnityEngine.UI;

public class BattleMgr : MonoBehaviour
{
    public List<Unit> Team = new(), TeamMain = new(), Enemys = new(), EnemyMain = new(), AllUnit = new();
    public List<string> AnimQueue = new();
    public float TimeCout, CurrTime;
    public Dictionary<int,Unit> IDUnitPiar = new();
    public Unit player, AtkU, CombU, AtkedU;
    public GameObject unit, HurtFont, ourObj, eneObj, 击退Pos1, 击退Pos2, 布阵提示, CombDetailPrefab;
    public Transform OurCombDetail, EneCombDetail;
    public int currentDamage, AtkTotal,CombSkiIdx,IDCount = 0,CombDetailCount;
    public string bonus, 当前追打状态;
    public bool isOurTurn,正在战斗,正在追打;
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
        //unit = Resources.Load("Prefabs/Unit/Unit") as GameObject;
        HurtFont = Resources.Load("Prefabs/HurtFont/Hurt") as GameObject;
    }
    public void InitTeam()
    {
        for (int i = 0; i < TeamManager.Ins.TeamData.Count; i++)
        {
            string uname = TeamManager.Ins.TeamData[i].Name;
            Unit u = Instantiate(Resources.Load("Prefabs/Unit/" + uname + "/" + uname) as GameObject).GetComponent<Unit>();

            u.Cell = TeamManager.Ins.TeamData[i].Cell;
            u.transform.SetParent(ourObj.transform.GetChild(u.Cell - 1));
            u.transform.localPosition = Vector2.zero;
            u.TeamInitAttr(TeamManager.Ins.TeamData[i]);
            u.tag = "Our";
            u.ID = IDCount;
            u.name = uname + u.ID;
            IDCount += 1;
            IDUnitPiar.Add(u.ID, u);
            Team.Add(u);
            TeamMain.Add(u);
            AllUnit.Add(u);
        }
    }

    public void InitBattle(string monsters)
    {
        //哥布林P1,哥布林P7,投石哥布林P5

        eneObj.SetActive(true);
        eneObj.transform.localPosition = new Vector2(1300, -140);
        var enemys = monsters.Split('&');
        foreach (string enemy in enemys)
        {
            string eName = enemy.Split('P')[0];
            int Pos = int.Parse(enemy.Split('P')[1]);
            Unit u = Instantiate(Resources.Load("Prefabs/Unit/" + eName + "/" +eName) as GameObject).GetComponent<Unit>();
            u.tag = "Enemy";
            //棋盘位置
            u.Cell = Pos;
            var 棋盘 = u.CompareTag("Our") == true ? ourObj : eneObj;
            u.transform.SetParent(棋盘.transform.GetChild(u.Cell-1));
            u.transform.localPosition = Vector2.zero;
            u.transform.localScale = new Vector2(-1, 1);
            u.EnemyInitAttr(eName);
            //加进list
            Enemys.Add(u);
            AllUnit.Add(u);
            u.ID = IDCount;
            u.name = eName + u.ID;
            IDCount += 1;
            IDUnitPiar.Add(u.ID, u);
        }
        EnterBattle();
    }

    public void SetPosSlotAlpha(float alpha)
    {
        foreach (var item in PosSlots)
        {
            item.color = new Color(1, 1, 1, alpha);
        }
        if(alpha > 0)
        {
            布阵提示.SetActive(true);
        }
        else
        {
            布阵提示.SetActive(false);
        }
    }

    public void EnterBattle()
    {
        ourObj.SetActive(true);
        eneObj.SetActive(true);
    }

    public void OnBattleClick()
    {
        SetPosSlotAlpha(0);
        OnTurnStart();
        //BattleBtn.enabled = false;
    }

    public void OnTurnStart()
    {
        foreach (var item in AllUnit)
        {
            item.Reload();
        }
        SortBySpeed();
        AddAtkBySpeed();
    }

    public void SortBySpeed()
    {
        Team.Sort((x, y) => x.Speed.CompareTo(y.Speed));
        Enemys.Sort((x, y) => x.Speed.CompareTo(y.Speed));
        AllUnit.Clear();
        AllUnit.AddRange(Team);
        AllUnit.AddRange(Enemys);
        AllUnit.Sort((u1, u2) => u2.Speed.CompareTo(u1.Speed));
    }

    public void AddAtkBySpeed()
    {
        foreach (var item in AllUnit)
        {
            for (int i = 0; i < item.RemainAtkCount; i++)
            {
                //用ID遍历AllUnit找到对应的Unit调用普攻
                AnimQueue.Add(item.ID + ":NormalAtk");
            }
            item.RemainAtkCount = 0;
        }
        StartCoroutine(PlayFirsrtAnimInQueue());
    }

    public void CheckComb(Unit u,string currDebuff)
    {
        List<Unit> units;
        if (u.CompareTag("Our"))
        {
            units = Team;
        }
        else
        {
            units = Enemys;
        }
        foreach (var item in units)
        {
            if (item.CombTypes.Contains(currDebuff) && item.RemainCombCount > 0)
            {
                AnimQueue.Insert(0,item.ID + ":Comb");
                item.RemainCombCount -= 1;
                break;
            }
        }
    }

    public IEnumerator PlayFirsrtAnimInQueue(int waitfor = 0)
    {

        print("msgPlayFirsrtAnimInQueue");
        if (waitfor != 0)
        {
            yield return new WaitForSeconds(waitfor);
        }
        if (AnimQueue.Count == 0) 
        {
            BattleBtn.enabled = true;
            yield return new WaitForSeconds(1);
            OnTurnEnd();
            yield break;
        }
        TimeCout = 0;

        //判断是攻击追打被动

        string[] com = AnimQueue[0].Split(":");
        print("msg" + AnimQueue[0]);

        if (com[1] == "NormalAtk")
        {
            AnimQueue.RemoveAt(0);
            IDUnitPiar[int.Parse(com[0])].ExecuteAtk();
        }
        else if (com[1] == "Comb")
        {
            AnimQueue.RemoveAt(0);
            IDUnitPiar[int.Parse(com[0])].ExecuteComb();
        }
        //动画结束时会继续在最后一帧调用此方法
    }

    public Unit GetTeamMinHealthUnit()
    {
        Ins.TeamMain.Sort((x, y) => x.Hp.CompareTo(y.Hp));
        Ins.TeamMain.Sort();
        return Ins.TeamMain[0];
    }

    public Unit GetOppositeTarget()
    {
        GameObject SearchFor;
        if (AtkU.CompareTag("Our")) { SearchFor = eneObj; }
        else { SearchFor = ourObj; }
        if (new[] { 1, 4, 7 }.Contains(AtkU.Cell))
        {
            //受击单位第一排
            if (SearchFor.transform.GetChild(0).childCount > 0) return SearchFor.transform.GetChild(0).GetChild(0).GetComponent<Unit>();
            if (SearchFor.transform.GetChild(3).childCount > 0) return SearchFor.transform.GetChild(3).GetChild(0).GetComponent<Unit>();
            if (SearchFor.transform.GetChild(6).childCount > 0) return SearchFor.transform.GetChild(6).GetChild(0).GetComponent<Unit>();
        }
        else if (new[] { 2, 5, 8 }.Contains(AtkU.Cell))
        {
            //第二排
            if (SearchFor.transform.GetChild(1).childCount > 0) return SearchFor.transform.GetChild(1).GetChild(0).GetComponent<Unit>();
            if (SearchFor.transform.GetChild(4).childCount > 0) return SearchFor.transform.GetChild(4).GetChild(0).GetComponent<Unit>();
            if (SearchFor.transform.GetChild(7).childCount > 0) return SearchFor.transform.GetChild(7).GetChild(0).GetComponent<Unit>();
        }
        else
        {
            //第三排
            if (SearchFor.transform.GetChild(2).childCount > 0) return SearchFor.transform.GetChild(2).GetChild(0).GetComponent<Unit>();
            if (SearchFor.transform.GetChild(5).childCount > 0) return SearchFor.transform.GetChild(5).GetChild(0).GetComponent<Unit>();
            if (SearchFor.transform.GetChild(8).childCount > 0) return SearchFor.transform.GetChild(8).GetChild(0).GetComponent<Unit>();
        }
        return null;
    }

    public void ShowSkillName(Unit u,string SkillName)
    {
        // 如果正在执行fade协程，则停止它
        if (u.CompareTag("Our"))
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

    private IEnumerator FadeName(Unit u,string SkillName)
    {
        CombDetailCount += 1;
        Transform t;
        if (u.CompareTag("Our")) t = OurCombDetail;
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
        yield return new WaitForSeconds(1.5f);

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

    public void ShowDamage(Unit u, string DamageValue)
    {
        ShowFont(u, DamageValue);
    }

    public void ShowHeal(Unit u, string healValue)
    {
        ShowFont(u, healValue,"Heal");
    }

    public void ShowFont(Unit u, string value, string type = "Damage")
    {
        GameObject font = Instantiate(HurtFont);
        TextMeshProUGUI tmp = font.GetComponent<TextMeshProUGUI>();
        tmp.text = value;
        Animator a = font.GetComponent<Animator>();
        font.transform.SetParent(u.transform.Find("FontPos"));
        font.transform.localPosition = Vector2.zero;
        if (type == "Damage") a.Play("hurt");
        else a.Play("heal");
    }

    public void OnTurnEnd()
    {
        for (int i = AllUnit.Count-1; i >= 0; i--)
        {
            AllUnit[i].OnTurnEnd();
        }
    }

    public void ExitBattle()
    {
        //EventsMgr.Instance.BattleDetail.gameObject.SetActive(false);
        AnimQueue.Clear();
        ourObj.SetActive(false);
        eneObj.SetActive(false);
        //TODO 战后结算
        //Bonus.Ins.ShowBonus();
        EventsMgr.Ins.GenNewRoom();
    }
}
