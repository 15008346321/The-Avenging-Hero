using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventsMgr : MonoBehaviour
{
    Dictionary<string, string[]> Events=new(), Battles=new(), Relics=new();
    public List<string> Level0Blood = new() { "以太" };
    public List<string> Level1Blood = new() { "火元素", "水元素", "风元素", "雷元素", "土元素" };
    public List<string> Level2Blood = new() { "火元素", "水元素", "风元素", "雷元素", "土元素" };
    public List<string> Level3Blood = new() { "火元素", "水元素", "风元素", "雷元素", "土元素" };
    List<string[]> RelicsList = new();
    string[] currentEvent;
    Image[] RoadImage = new Image[2];
    public Image FadeImg;
    public Button ExploreBtn,StatuePrayBtn;
    public Button[]
        RoadBtns   = new Button[2],
        EventBtns  = new Button[2],
        BattleBtns = new Button[2],
        StatueBtns = new Button[8],
        StatueMbrBtns = new Button[4],
        RelicsBtns = new Button[3];
    TextMeshProUGUI[]
        RoadsTitleTMPs     = new TextMeshProUGUI[3],
        EventChooseTMPs    = new TextMeshProUGUI[2],
        BounusRelicsName   = new TextMeshProUGUI[3],
        BounusRelicsEffect = new TextMeshProUGUI[3];
    float[] RoadRate = {0,50,0,10};//神像，战斗，公会，商店
    public int EventPoint, MaxEventPoint,shopCount = 0,campCount = 0, StatueIdx,StatueMbrIdx,PrayCount,BonusGold,Gold;
    public string monsters, bonus;
    public TextMeshProUGUI EPTMP, TitleTMP, ContentTMP, ResultTMP, MainTMP, PrayCountTMP,UIBonusGoldTMP,UIGoldTMP;
    public Transform RoadParentNode, EventParentNode, EventContentNode, EventResultNode,DailyNode,
        ShopNode, StatueNode, StatueParent, StatueMbrParent;
    public static EventsMgr Ins;
    public bool IsMoveToBattle, IsMoveToStatue;

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

        //初始化神像按钮
        for (int i = 0; i < 3; i++)
        {
            StatueBtns[i] = StatueParent.GetChild(i).GetComponent<Button>();
            var j = i;
            StatueBtns[i].onClick.AddListener(() => StatueIdx = j);
        }
        //for (int i = 0; i < 4; i++)
        //{
        //    StatueMbrBtns[i] = StatueMbrParent.GetChild(i).GetComponent<Button>();
        //    var j = i;
        //    StatueMbrBtns[i].onClick.AddListener(() => StatueMbrIdx = j);
        //}
    }

    private void GenNewRoom()
    {
       // ScrollBg.Ins.MoveBg = true;

        RoadParentNode.gameObject.SetActive(true);
        //RoadParentNode.transform.localPosition = new Vector2(1200, 0);
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

        //避免误触
        RoadBtns[0].enabled = false;
        RoadBtns[1].enabled = false;

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
                    RoadsTitleTMPs[i].text = "神像";
                    RoadImage[i].sprite = Resources.Load<Sprite>("Texture/Icon/001");
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
                    RoadsTitleTMPs[i].text = "公会";
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
            case "神像":
                StatueNode.gameObject.SetActive(true);
                PrayCount +=1;
                PrayCountTMP.text = "(祈祷次数:" + PrayCount + ")";
                break;
            case "战斗":
                SetRoadToBattle();
                break;
            case "公会":
                SetRoadToGuild();
                break;
            case "商店":
                SetRoadToShop();
                break;
            default:
                break;
        }
    }

    private void SetRoadToGuild()
    {
        throw new System.NotImplementedException();
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
        ExploreBtn.gameObject.SetActive(true);
    }
    //public void AddRelicsIcon()
    //{

    //}

    public void SetRoadToBattle()
    {
        //设置敌人
        var ranNum = Random.Range(0, LevelManager.Ins.CurrentLevel.Battles.Count);
        BattleMgr.Ins.EnemysStr = LevelManager.Ins.CurrentLevel.Battles[ranNum][1];
        LevelManager.Ins.CurrentLevel.Battles.Remove(LevelManager.Ins.CurrentLevel.Battles[ranNum]);
        //生成
        BattleMgr.Ins.TeamRun();
        IsMoveToBattle = true;
        FadeIn();
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
        string code = index switch
        {
            0 => currentEvent[3],
            1 => currentEvent[5],
            _ => "",
        };
        //TODO道具属性飞入动画
        BattleMgr.Ins.TeamRun();
        //ExecuteMgr.Ins.ExecuteCode(code);
        EventContentNode.gameObject.SetActive(false);
        RoadParentNode.gameObject.SetActive(true);
        ExploreBtn.gameObject.SetActive(true);
    }

    public void OnClickExplore()
    {
        if(EventPoint == 0)
        {
            //GameOver
        }
        else
        {
            EventPoint -= 1;
            EPTMP.text = "" + EventPoint;
            BattleMgr.Ins.TeamRun();
            FadeIn();
        }
    }

    public void FadeIn()
    {
        FadeImg.raycastTarget = true;
        //人往前走 变黑
        BattleMgr.Ins.OurRunningPos.DOLocalMoveX(600, 2);
        //用一秒变黑

        FadeImg.DOFade(1f, 1f).OnComplete(() =>
        {
            //生成事件 人走出来 变亮

            //进入战斗 或者  探索
            if (IsMoveToBattle)
            {
                BattleMgr.Ins.InitTeam(true);
                RoadParentNode.gameObject.SetActive(false);
            }
            else if (IsMoveToStatue)
            {
                StatueNode.gameObject.SetActive(true);
                ExploreBtn.gameObject.SetActive(false);
                Statue.Ins.SetRandomStatue();
            }
            else
            {
                ExploreBtn.gameObject.SetActive(false);
                GenNewRoom();
                BattleMgr.Ins.OurRunningPos.DOKill();
                RoadParentNode.gameObject.SetActive(true);
            }

            BattleMgr.Ins.OurRunningPos.DOLocalMoveX(-1000, 0f);
            BattleMgr.Ins.OurRunningPos.DOLocalMoveX(-579, 1f);

            //变亮
            FadeImg.DOFade(0f, 1f).OnComplete(() =>
            {
                BattleMgr.Ins.TeamIdle();
                FadeImg.raycastTarget = false;
                RoadBtns[0].enabled = true;
                RoadBtns[1].enabled = true;
                if (IsMoveToBattle)
                {
                    IsMoveToBattle = false;
                    BattleMgr.Ins.EnterBattle();
                    SetUnitCanDrag(true);
                }
                if (IsMoveToStatue)
                {
                    SetUnitCanDrag(true);
                }
            }
            );
        }
        );
    }

    public void SetBonusGold(int num)
    {
        BonusGold = 0;
        for (int i = 0; i < num; i++)
        {
            BonusGold += 2;
            BonusGold += Random.Range(0, 2);
        }
    }

    public void ShowBonus()
    {
        SetBonusGold(TeamManager.Ins.EnemyData.Count);
        UIBonusGoldTMP.text = "<sprite=\"coin\" index=0>+" + BonusGold;
        UIBonusGoldTMP.transform.DOLocalMoveX(72, 0.5f).OnComplete(
            () =>
                {
                    UIBonusGoldTMP.transform.DOLocalMoveX(-100, 0.5f).SetDelay(1f);
                }
        );
        ExploreBtn.gameObject.SetActive(true);
        Gold += BonusGold;
        UIGoldTMP.text = Gold.ToString();
        IsMoveToStatue = true;
    }

    public void SetUnitCanDrag(bool CanDrag = false)
    {
        foreach (var item in BattleMgr.Ins.Team)
        {
            item.TMP.raycastTarget = CanDrag;
        }
    }
}
