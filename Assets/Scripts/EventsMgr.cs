using DG.Tweening;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventsMgr : MonoBehaviour
{
    Dictionary<string, string[]> Events=new(), Battles=new(), Relics=new();
    public List<魔力类型Enum> Level0Blood = new() { 魔力类型Enum.以太 };
    public List<魔力类型Enum> Level1Blood = new() { 魔力类型Enum.火元素, 魔力类型Enum.水元素, 魔力类型Enum.风元素, 魔力类型Enum.雷元素, 魔力类型Enum.土元素 };
    public List<魔力类型Enum> Level2Blood = new() { 魔力类型Enum.火元素, 魔力类型Enum.水元素, 魔力类型Enum.风元素, 魔力类型Enum.雷元素, 魔力类型Enum.土元素 };
    public List<魔力类型Enum> Level3Blood = new() { 魔力类型Enum.火元素, 魔力类型Enum.水元素, 魔力类型Enum.风元素, 魔力类型Enum.雷元素, 魔力类型Enum.土元素 };
    List<string[]> RelicsList = new();
    string[] currentEvent;
    public Image[] 路线Imgs = new Image[2];
    public Image FadeImg;
    public Button[]
        RoadBtns = new Button[2],
        EventBtns = new Button[2],
        StatueBtns = new Button[3];
    public TextMeshProUGUI[]
        路线TMPs     = new TextMeshProUGUI[3],
        EventChooseTMPs    = new TextMeshProUGUI[2],
        BounusRelicsName   = new TextMeshProUGUI[3],
        BounusRelicsEffect = new TextMeshProUGUI[3];
    readonly float[] 事件出现率 = {50,60,10};//战斗，酒馆，商店
    public int EventPoint, MaxEventPoint, StatueIdx, StatueMbrIdx, BonusGold, 玩家拥有的金币, 危险级别, 战利品金币数;
    public string monsters, bonus;
    public TextMeshProUGUI EPTMP, TitleTMP, ContentTMP, ResultTMP, MainTMP,UIGoldTMP;
    public Transform RoadParentNode, EventParentNode, EventContentNode, EventResultNode,DailyNode,
        ShopNode, StatueNode, StatueParent,侧边栏父级;
    public GameObject 神像;
    public static EventsMgr Ins;
    public bool 是否去往战斗, 是否去往神像,是否去往酒馆;
    public List<TextMeshProUGUI> 侧边栏池;
    public List<Button> 战利品;

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
            路线TMPs[i] = RoadBtns[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            路线Imgs[i] = RoadBtns[i].transform.GetChild(1).GetComponent<Image>();
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

        //初始化奖励侧边栏
        for (int i = 0; i < 侧边栏父级.childCount; i++)
        {
            TextMeshProUGUI tmp = 侧边栏父级.GetChild(i).GetComponent<TextMeshProUGUI>();
            侧边栏池.Add(tmp);
        }
    }

    public void 获取空侧边栏并展示(string str,Action action = null)
    {
        for (int i = 0; i < 侧边栏池.Count; i++)
        {
            if (侧边栏池[i].text == "")
            {
                侧边栏动画(侧边栏池[i], str, action);
                break;
            }
        }
    }

    private void 侧边栏动画(TextMeshProUGUI tmp,string str, Action action)
    {
        if (str.StartsWith("金币")) 
        {
            str = str.Replace("金币", "<sprite=\"coin\" index=0>");
        }
        tmp.text = str;

        tmp.rectTransform.DOLocalMoveX(210, 0.5f).OnComplete(
            () =>
            {
                action?.Invoke();
                tmp.rectTransform.DOLocalMoveX(0, 0.5f).SetDelay(0.5f).OnComplete
                (
                    () => {
                        tmp.text = "";
                        }
                );
            }
        );
    }

    public void 生成新路线()
    {
       // ScrollBg.Ins.MoveBg = true;

        RoadParentNode.gameObject.SetActive(true);
        //RoadParentNode.transform.localPosition = new Vector2(1200, 0);
        var ran = UnityEngine.Random.Range(0, 100);
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
        bool 战斗事件已出现 = false, 酒馆已出现 = false, 商店已出现 = false;
        var count = 0;
        for (var i = 0; i < roomCount; i++)
        {
            print("第"+ i + "个=============================================");
            while (true)
            {
                count += 1;
                if(count == 1000) 
                {
                    Debug.LogError("已循环1000次 break");
                    break; 
                }

                ran = UnityEngine.Random.Range(0, (int)事件出现率.Sum());
                print(ran);
                if (ran < 事件出现率[0])
                {
                    print("战斗事件已出现");
                    if (战斗事件已出现) continue;
                    路线TMPs[i].text = "战斗";
                    路线Imgs[i].sprite = Resources.Load<Sprite>("Texture/Icon/002_battle");
                    战斗事件已出现 = true;
                    break;
                }
                else if (ran < (事件出现率[0] + 事件出现率[1]))
                {
                    print("酒馆已出现");
                    if (酒馆已出现) continue;
                    路线TMPs[i].text = "酒馆";
                    路线Imgs[i].sprite = Resources.Load<Sprite>("Texture/Icon/004_camp");
                    酒馆已出现 = true;
                    break;
                }
                else if (ran < 事件出现率[0] + 事件出现率[1] + 事件出现率[2])
                {
                    print("商店已出现");
                    if (商店已出现) continue;
                    路线TMPs[i].text = "商店";
                    路线Imgs[i].sprite = Resources.Load<Sprite>("Texture/Icon/003_shop");
                    商店已出现 = true;
                    break;
                }
            }
        }
    }

    public void 生成一个战斗路线()
    {
        RoadParentNode.gameObject.SetActive(true);
        RoadBtns[1].gameObject.SetActive(false);
        RoadBtns[0].transform.localPosition = new Vector2(0, -50);
        路线TMPs[0].text = "战斗";
        路线Imgs[0].sprite = Resources.Load<Sprite>("Texture/Icon/002_battle");
    }

    public void RoadChoose(int i)
    {
        RoadParentNode.gameObject.SetActive(false);
        switch (路线TMPs[i].text)
        {
            case "战斗":
                去往战斗();
                break;
            case "酒馆":
                去往酒馆();
                break;
            case "商店":
                SetRoadToShop();
                break;
            default:
                break;
        }
    }

    private void 去往酒馆()
    {
        BattleMgr.Ins.TeamRun();
        是否去往酒馆 = true;
        Fade();
    }

    public void 去往战斗()
    {
        //设置敌人
        //战斗全打了就出boss
        if(LevelManager.Ins.CurrentLevel.Battles.Count == 0)
        {
            BattleMgr.Ins.EnemysStr = LevelManager.Ins.CurrentLevel.Boss;
        }
        //没打完就随机出小怪
        else
        {
            var ranNum = UnityEngine.Random.Range(0, LevelManager.Ins.CurrentLevel.Battles.Count);
            BattleMgr.Ins.EnemysStr = LevelManager.Ins.CurrentLevel.Battles[ranNum];
            LevelManager.Ins.CurrentLevel.Battles.Remove(LevelManager.Ins.CurrentLevel.Battles[ranNum]);
        }

        危险级别 = int.Parse( BattleMgr.Ins.EnemysStr[1]);

        //生成
        是否去往战斗 = true;
        Fade();
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
        int random = UnityEngine.Random.Range(0, LevelManager.Ins.CurrentLevel.Events.Count - 1);
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
            //UIMgr.Ins.背景滚动();
            Fade();
        }
    }

    public void Fade()
    {
        FadeImg.raycastTarget = true;
        //人往前走 变黑
        BattleMgr.Ins.OurRunningPos.DOLocalMoveX(600, 2);
        //用一秒变黑

        FadeImg.DOFade(1f, 1f).OnComplete(
            () =>
                {
                    //生成事件 人走出来 变亮

                    //进入战斗 或者  探索
                    if (是否去往战斗)
                    {
                        RoadParentNode.gameObject.SetActive(false);
                        BattleMgr.Ins.EnterBattle();
                    }
                    else if (是否去往酒馆)
                    {
                        UIMgr.Ins.隐藏小队();
                        Inn.Ins.展示可招募单位();
                    }
                    else
                    {
                        生成新路线();
                        BattleMgr.Ins.OurRunningPos.DOKill();
                        RoadParentNode.gameObject.SetActive(true);
                    }

                    //BattleMgr.Ins.OurRunningPos.DOLocalMoveX(-1000, 0f);
                    //BattleMgr.Ins.OurRunningPos.DOLocalMoveX(-579, 1f);

                    if (!是否去往酒馆)
                    {
                        UIMgr.Ins.显示小队();
                    }

                    //if (!是否去往战斗)
                    //{
                    //    UIMgr.Ins.显示小队();
                    //}

                    //变亮
                    FadeImg.DOFade(0f, 1f).OnComplete(
                        () =>
                            {
                                FadeImg.raycastTarget = false;
                            }
                    );
                }
        );
    }

    public void 显示战利品()
    {
        设置战利品();
        for (int i = 0; i < 战利品.Count; i++)
        {
            战利品[i].gameObject.SetActive(true);
        }
    }

    public void 设置战利品()
    {
        switch (危险级别)
        {
            case 1:
                战利品金币数 = 6;
                break;
            case 2:
                战利品金币数 = 10;
                break;
            default:
                break;
        }
    }

    public void 获取战利品金币()
    {
        ShowGetBonus(战利品金币数);
        战利品[0].gameObject.SetActive(false);
    }

    public void ShowGetBonus(int value)
    {
        获取空侧边栏并展示("金币+" + value, () => 
        {
            获取金币(value);
        });
    }

    public void 获取金币(int value) 
    {
        玩家拥有的金币 += value;
        UIGoldTMP.text = 玩家拥有的金币.ToString();
    }
}