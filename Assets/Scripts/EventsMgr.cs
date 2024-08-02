using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventsMgr : MonoBehaviour
{
    Dictionary<string, string[]> Events=new(), Battles=new(), Relics=new();
    List<string[]> RelicsList = new();
    string[] currentEvent;
    Image[] RoadImage = new Image[2];
    public Button[]
        RoadBtns   = new Button[2],
        EventBtns  = new Button[2],
        BattleBtns = new Button[2],
        CampBtns   = new Button[6],
        RelicsBtns = new Button[3];
    TextMeshProUGUI[]
        RoadsTitleTMPs     = new TextMeshProUGUI[3],
        EventChooseTMPs    = new TextMeshProUGUI[2],
        BounusRelicsName   = new TextMeshProUGUI[3],
        BounusRelicsEffect = new TextMeshProUGUI[3];
    float[] RoadRate = {30,50,10,10};//事件，战斗，营地，商店
    public int EventPoint, MaxEventPoint,shopCount = 0,campCount = 0;
    public string monsters, bonus;
    public TextMeshProUGUI EPTMP, TitleTMP, ContentTMP, ResultTMP, MainTMP;
    public Transform RoadParentNode, EventParentNode, EventContentNode, EventResultNode,DailyNode,
        ShopNode, CampNode;
    public static EventsMgr Ins;
   
    private void Awake()
    {
        if (Ins == null) Ins = this;
        else Destroy(Ins);
    }

    private void Start()
    {
        Init();
    }

    public void Init(string Area = "Area0")
    {
        //初始化行动力
        EventPoint = MaxEventPoint = 10;

        //初始化路线按钮
        for (var i = 0; i < 2; i++)
        {
            RoadsTitleTMPs[i] = RoadBtns[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            RoadImage[i] = RoadBtns[i].transform.GetChild(1).GetComponent<Image>();
            var j = i;
            RoadBtns[i].onClick.AddListener(() => RoadChoose(j));
        }

        //初始化事件按钮
        TitleTMP     = EventContentNode.Find("Bg/TitleBg/TMP").GetComponent<TextMeshProUGUI>();
        ContentTMP   = EventContentNode.Find("Bg/TextBg/TMP").GetComponent<TextMeshProUGUI>();
        EventBtns[0] = EventContentNode.Find("Bg/Button1").GetComponent<Button>();
        EventBtns[1] = EventContentNode.Find("Bg/Button2").GetComponent<Button>();
        EventChooseTMPs[0] = EventBtns[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        EventChooseTMPs[1] = EventBtns[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        EventBtns[0].onClick.AddListener(() => Option(0));
        EventBtns[1].onClick.AddListener(() => Option(1));
        ResultTMP = EventResultNode.Find("TextBg/TMP").GetComponent<TextMeshProUGUI>();
        MainTMP = DailyNode.Find("Bg/TextBg/TMP").GetComponent<TextMeshProUGUI>();

        //初始化营地按钮
        for (int i = 0; i < 5; i++)
        {
            CampBtns[i] = CampNode.transform.Find("camp/Bg").GetChild(i).GetComponent<Button>();
            var j = i;
            CampBtns[i].onClick.AddListener(() => CampChoose(j));
        }
    }

    public void GenNewRoom()
    {
        ScrollBg.Ins.MoveBg = true;

        RoadParentNode.gameObject.SetActive(true);
        RoadParentNode.transform.localPosition = new Vector2(1200, 0);
        var ran = Random.Range(0, 100);
        int roomCount;
        //30概率只有一个房间
        if (ran < 30)
        {
            roomCount = 1;
            RoadBtns[0].transform.localPosition = new Vector2(0, -50);
            RoadBtns[1].gameObject.SetActive(false);
        }
        else
        {
            roomCount = 2;
            RoadBtns[0].transform.localPosition = new Vector2(0,100);
            RoadBtns[1].gameObject.SetActive(true);
        }
        //避免二选一是同样的选项
        bool done1 = false, done2 = false, done3 = false, done4 = false;

        for (var i = 0; i < roomCount; i++)
        {
            while (true)
            {
                ran = Random.Range(0, 100);
                if (ran < RoadRate[0])
                {
                    if (done1) continue;
                    RoadsTitleTMPs[i].text = "事件";
                    RoadImage[i].sprite = Resources.Load<Sprite>("Texture/Icon/001_event");
                    done1 = true;
                    break;
                }
                else if (ran < RoadRate[0] + RoadRate[1])
                {
                    if (done2) continue;
                    RoadsTitleTMPs[i].text = "战斗";
                    RoadImage[i].sprite = Resources.Load<Sprite>("Texture/Icon/002_battle");
                    done2 = true;
                    break;
                }
                else if (ran < (RoadRate[0] + RoadRate[1] + RoadRate[2]))
                {
                    if (done3) continue;
                    if (campCount == 2) continue;
                    RoadsTitleTMPs[i].text = "营地";
                    RoadImage[i].sprite = Resources.Load<Sprite>("Texture/Icon/004_camp");
                    campCount += 1;
                    done3 = true;
                    break;
                }
                else if (ran < RoadRate[0] + RoadRate[1] + RoadRate[2] + RoadRate[3])
                {
                    if (done4) continue;
                    if (shopCount == 2) continue;
                    RoadsTitleTMPs[i].text = "商店";
                    RoadImage[i].sprite = Resources.Load<Sprite>("Texture/Icon/003_shop");
                    shopCount += 1;
                    done4 = true;
                    break;
                }
            }
        }
    }

    public void RoadChoose(int i)
    {
        RoadParentNode.gameObject.SetActive(false);
        switch (RoadsTitleTMPs[i].text)
        {
            case "事件":
                SetRoadToEvent();
                break;
            case "战斗":
                SetRoadToBattle();
                break;
            case "营地":
                SetRoadToCamp();
                break;
            case "商店":
                SetRoadToShop();
                break;
            default:
                break;
        }
        消耗行动力();
        //GenNewRoom();
    }

    public void CampChoose(int i)
    {
        switch (i)
        {
            case 0:
                //交流情报与任务接取
                //魔法物品交易会
                //竞技挑战与角斗赛
                //神秘事件调查
                //文化交流与节日庆典
                //遭遇敌对势力或盗贼团伙
                break;
            case 1:
                EventPoint += 1;
                EPTMP.text = "" + EventPoint;
                break;
            case 2:
                //BagManager.Ins.Hp += (int)BagManager.Ins.MaxHp * 0.3f;
                break;
            case 3:
                FeelMagic(Random.Range(0, 5));
                FeelMagic(Random.Range(0, 5));
                break;
            case 4:
                //BattleMgr.Ins.player.Atk += 1;
                break;
            default:
                break;
        }
        //AttrInfo.Instance.ShowInfo(BattleMgr.Instance.player);
        CampNode.gameObject.SetActive(false);
        GenNewRoom();
    }

    public void FeelMagic(int j)
    {
        //if (j == 0) BattleMgr.Ins.player.Fire    += 1;
        //if (j == 1) BattleMgr.Ins.player.Water   += 1;
        //if (j == 2) BattleMgr.Ins.player.Wind    += 1;
        //if (j == 3) BattleMgr.Ins.player.Thunder += 1;
        //if (j == 4) BattleMgr.Ins.player.Earth   += 1;
    }

    public void RelicsChoose(int idx)
    {
        //ExecuteMgr.Ins.ExecuteCode(RelicsList[idx][5]);
        //TODO添加遗物ui并做一些动效
        //AddRelicsIcon();
        GenNewRoom();
    }
    //public void AddRelicsIcon()
    //{

    //}

    public void SetRoadToBattle()
    {
        SetRandomBattles();
        BattleMgr.Ins.InitEnemys(monsters);
        ScrollBg.Ins.MoveEnemy = true;
    }

    public void SetRoadToCamp()
    {
        CampNode.gameObject.SetActive(true);
    }

    public void SetRoadToEvent()
    {
        EventContentNode.gameObject.SetActive(true);
        SetRandomEvents();
    }

    public void SetRoadToShop()
    {
        ShopNode.gameObject.SetActive(true);
    }

    public void SetRandomRelics()
    {

        //print("msg"+ CSVManager.Ins.Relics.Count);
        //RelicsList.Clear();
        //var ran1 = Random.Range(0, CSVManager.Ins.Relics.Count);
        //while (BagManager.Ins.GotRelics.Contains(ran1)) ran1 = Random.Range(0, CSVManager.Ins.Relics.Count);
        //print("msg1");
        //RelicsList.Add(CSVManager.Ins.Relics[ran1]);
        //var ran2 = Random.Range(0, CSVManager.Ins.Relics.Count);
        //while (BagManager.Ins.GotRelics.Contains(ran2) || ran1 == ran2) ran2 = Random.Range(0, CSVManager.Ins.Relics.Count);
        //print("msg2");
        //RelicsList.Add(CSVManager.Ins.Relics[ran2]);
        //var ran3 = Random.Range(0, CSVManager.Ins.Relics.Count);
        //while (BagManager.Ins.GotRelics.Contains(ran3) || ran3 == ran1 || ran3 == ran2) ran3 = Random.Range(0, CSVManager.Ins.Relics.Count);
        //print("msg3");
        //RelicsList.Add(CSVManager.Ins.Relics[ran3]);

        //for (int i = 0; i < RelicsList.Count; i++)
        //{
        //    BounusRelicsName[i].text = RelicsList[i][1];
        //    BounusRelicsEffect[i].text = RelicsList[i][3];
        //}
    }

    public void 消耗行动力()
    {
        EventPoint -= 1;
        EPTMP.text = "" + EventPoint;
        if (EventPoint <= 0)
        {
            NextDay();
        }
    }

    private void NextDay()
    {
        //弹出昨夜总结面板
        //新一天事件
        EventPoint = MaxEventPoint;
        EPTMP.text = "" + EventPoint;
    }
    public void SetRandomBattles()
    {
        var ranNum = Random.Range(0,LevelManager.Ins.CurrentLevel.Battles.Count);
        monsters = LevelManager.Ins.CurrentLevel.Battles[ranNum][1];
        LevelManager.Ins.CurrentLevel.Battles.Remove(LevelManager.Ins.CurrentLevel.Battles[ranNum]);
    }


    public void SetRandomEvents()
    {
        //if (EventsList.Count == 0)
        //{
        //    Content.Text = "这里已经没有什么可探索的了，还是离开吧";
        //    EventChooseLabel[0].Text = "好的";
        //    EventChooseLabel[1].Visible = false;
        //    AreaFinished = true;
        //    return;
        //}
        int random = Random.Range(0, LevelManager.Ins.CurrentLevel.Events.Count - 1);
        //0id,1事件,21选择,31结果,41结算code,52选择,62结果,72结算code
        //Option方法读取currentEvent中数据
        currentEvent = LevelManager.Ins.CurrentLevel.Events[random];
        LevelManager.Ins.CurrentLevel.Events.RemoveAt(random);

        SetEventsUIAndContent();
    }

    public void SetEventsUIAndContent()
    {
        ContentTMP.text = currentEvent[1];
        EventChooseTMPs[0].text = currentEvent[2];
        if (currentEvent[2] == "")
        {
            EventChooseTMPs[1].transform.parent.gameObject.SetActive(false);
            EventChooseTMPs[0].transform.parent.localPosition = new Vector2(0,-130);
            EventChooseTMPs[0].text = "确认";
        }
        else
        {
            EventChooseTMPs[1].transform.parent.gameObject.SetActive(true);
            EventChooseTMPs[0].transform.parent.localPosition = new Vector2(0, -70);
            EventChooseTMPs[1].text = currentEvent[4];
        }
    }


    public void Option(int index)
    {
        string code;
        switch (index)
        {
            case 0:
                code = currentEvent[3];
                break;
            case 1:
                code = currentEvent[5];
                break;
            default:
                code = "";
                break;
        }
        //TODO道具属性飞入动画
        BattleMgr.Ins.OurTeamRun();
        //ExecuteMgr.Ins.ExecuteCode(code);
        EventContentNode.gameObject.SetActive(false);
        RoadParentNode.gameObject.SetActive(true);
        GenNewRoom();
    }
}
