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
    string[] currentEvent;
    public Image[] 路线Imgs = new Image[2];
    public Image FadeImg,世界等级填充IMG;
    public Button 探索按钮;
    public Button[]
        RoadBtns = new Button[2],
        EventBtns = new Button[2],
        StatueBtns = new Button[3];
    public TextMeshProUGUI[]
        EventChooseTMPs    = new TextMeshProUGUI[2],
        BounusRelicsName   = new TextMeshProUGUI[3],
        BounusRelicsEffect = new TextMeshProUGUI[3];
    public int EventPoint, MaxEventPoint, StatueIdx, StatueMbrIdx, BonusGold, 玩家拥有的金币, 危险级别, 战利品金币数, 世界等级INT, 世界等级进度INT, 敌人数量INT, 敌人数量Max;
    public string monsters, bonus;
    public TextMeshProUGUI EPTMP, TitleTMP, ContentTMP, ResultTMP, MainTMP,UIGoldTMP,探索按钮TMP,世界等级进度TMP,世界等级TMP;
    public Transform RoadParentNode, EventParentNode, EventContentNode, EventResultNode,DailyNode,
        ShopNode, StatueNode, StatueParent,侧边栏父级;
    public GameObject 神像,世界等级OBJ;
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
    }

    public void 生成一个战斗路线()
    {
        探索按钮.gameObject.SetActive(true);
    }

    public void 当点击探索按钮(int i)
    {
        if(BagMgr.Ins.补给 > 0)
        {
            StartCoroutine(BagMgr.Ins.补给变动(-5));
            世界等级进度增加();
            设置战斗();
            是否去往战斗 = true;
            Fade();
        }
        else
        {
            UIMgr.Ins.文字提示("补给不足,回城镇修整吧");
        }
        
    }

    public void 世界等级进度增加()
    {
        世界等级进度INT += 6;
        if (世界等级进度INT >= 100)
        {
            世界等级进度INT = 100;
            世界等级INT += 1;
            世界等级TMP.text = UIMgr.Ins.TMP图片化文字(世界等级INT.ToString());
        }
        世界等级进度TMP.text = UIMgr.Ins.TMP图片化文字(世界等级进度INT.ToString()) + "<sprite=\"百分号\" index=0>";
        世界等级填充IMG.fillAmount = 世界等级进度INT / 100f;
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
        var ranNum = UnityEngine.Random.Range(0, 10);
        敌人数量Max = 世界等级INT switch
        {
            0 => 1,
            1 => 2,
            2 => 3,
            _ => 4,
        };
        敌人数量INT = UnityEngine.Random.Range(1, 敌人数量Max+1);
        BattleMgr.Ins.EnemysStr.Clear();
        string[] 怪物;
        int ran;
        //Boss
        if (世界等级进度INT >= 100)
        {
            //表结构:
            //级别  怪1         怪2   怪3
            //小怪  哥布林
            //小怪  史莱姆
            //精英  巨魔
            //稀有  黄金哥布林  牛头
            //Boss  剑士        弓手  法师

            //一个战斗Arr中0为级别 循环从1开始
            怪物 = LevelMgr.Ins.CurrentLevel.Boss表[0].Split(',');
            for (int i = 0; i < 怪物.Length; i++)
            {
                BattleMgr.Ins.EnemysStr.Add(怪物[i]);
            }
        }
        //稀有
        else if( ranNum == 0 && LevelMgr.Ins.CurrentLevel.稀有表.Count > 0)
        {
            ran = UnityEngine.Random.Range(0, LevelMgr.Ins.CurrentLevel.稀有表.Count);

            怪物 = LevelMgr.Ins.CurrentLevel.稀有表[ran].Split(',');
            for (int i = 0; i < 怪物.Length; i++)
            {
                BattleMgr.Ins.EnemysStr.Add(怪物[i]);
            }
            LevelMgr.Ins.CurrentLevel.稀有表.RemoveAt(ran);
        }
        else if (ranNum >0 && ranNum <7)
        {
            for (int i = 0; i < 敌人数量INT; i++)
            {
                ran = UnityEngine.Random.Range(0, LevelMgr.Ins.CurrentLevel.小怪表.Count);
                BattleMgr.Ins.EnemysStr.Add(LevelMgr.Ins.CurrentLevel.小怪表[ran]);
            }
        }
        else if(ranNum >=7)
        {
            for (int i = 0; i < 敌人数量INT; i++)
            {
                if(i == 0)
                {
                    ran = UnityEngine.Random.Range(0, LevelMgr.Ins.CurrentLevel.精英表.Count);
                    BattleMgr.Ins.EnemysStr.Add(LevelMgr.Ins.CurrentLevel.精英表[ran]);
                }else
                {
                    ran = UnityEngine.Random.Range(0, LevelMgr.Ins.CurrentLevel.小怪表.Count);
                    BattleMgr.Ins.EnemysStr.Add(LevelMgr.Ins.CurrentLevel.小怪表[ran]);
                }
                
            }
        }
        else
        {
            //稀有怪那可能判定不上 在调用一次
            设置战斗();
        }

    }

    public void SetRoadToEvent()
    {
        EventContentNode.gameObject.SetActive(true);
        //SetRandomEvents();
    }

    public void SetRoadToShop()
    {
        ShopNode.gameObject.SetActive(true);
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
        //根据打的怪来
    }

    public void 获取战利品金币()
    {
        StartCoroutine( BagMgr.Ins.金币变动(战利品金币数));
        战利品[0].gameObject.SetActive(false);
    }

}