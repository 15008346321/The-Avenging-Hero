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
    public Button 探索按钮;
    public Button[]
        RoadBtns = new Button[2],
        EventBtns = new Button[2],
        StatueBtns = new Button[3];
    public TextMeshProUGUI[]
        EventChooseTMPs    = new TextMeshProUGUI[2],
        BounusRelicsName   = new TextMeshProUGUI[3],
        BounusRelicsEffect = new TextMeshProUGUI[3];
    public int EventPoint, MaxEventPoint, StatueIdx, StatueMbrIdx, BonusGold, 玩家拥有的金币, 危险级别, 战利品金币数;
    public string monsters, bonus;
    public TextMeshProUGUI EPTMP, TitleTMP, ContentTMP, ResultTMP, MainTMP,UIGoldTMP,探索按钮TMP;
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
    public void 生成一个战斗路线()
    {
        探索按钮.gameObject.SetActive(true);
    }

    public void 当点击探索按钮(int i)
    {
        设置战斗();
    }

    private void 去往酒馆()
    {
        BattleMgr.Ins.TeamRun();
        是否去往酒馆 = true;
        Fade();
    }

    public void 设置战斗()
    {
        //设置敌人
        //战斗全打了就出boss
        if(LevelMgr.Ins.CurrentLevel.Battles.Count == 0)
        {
            BattleMgr.Ins.EnemysStr = LevelMgr.Ins.CurrentLevel.Boss;
        }
        //没打完就随机出小怪
        else
        {
            var ranNum = UnityEngine.Random.Range(0, LevelMgr.Ins.CurrentLevel.Battles.Count);
            BattleMgr.Ins.EnemysStr = LevelMgr.Ins.CurrentLevel.Battles[ranNum];
            LevelMgr.Ins.CurrentLevel.Battles.Remove(LevelMgr.Ins.CurrentLevel.Battles[ranNum]);
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
        int random = UnityEngine.Random.Range(0, LevelMgr.Ins.CurrentLevel.Events.Count - 1);
        //0id,1事件,21选择,31结果,41结算code,52选择,62结果,72结算code
        //Option方法读取currentEvent中数据
        currentEvent = LevelMgr.Ins.CurrentLevel.Events[random];
        LevelMgr.Ins.CurrentLevel.Events.RemoveAt(random);

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

    public void Fade(Action Callback = null)
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
                        探索按钮.gameObject.SetActive(false);
                        BattleMgr.Ins.EnterBattle();
                    }
                    else if (是否去往酒馆)
                    {
                        UIMgr.Ins.隐藏小队();
                        Inn.Ins.展示可招募单位();
                    }
                    else
                    {
                        探索按钮.gameObject.SetActive(true);
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
                                Callback?.Invoke();
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
            BagMgr.Ins.获取金币(value);
        });
    }


}