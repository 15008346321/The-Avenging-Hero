using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class BagMgr : MonoBehaviour
{
    public GameObject 材料背包, 遗物详情;
    public int 玩家拥有的金币;
    public List<遗物基类> 遗物基类List = new();
    public List<遗物实例> 遗物实例List;
    public TextMeshProUGUI 详情名称, 详情描述;
    public Image 详情图片;

    public static BagMgr Ins;
    private void Awake()
    {
        if (Ins == null) Ins = this;
        else Destroy(Ins);
    }

    private void Start()
    {
        
    }
    public void 获取金币(int value)
    {
        玩家拥有的金币 += value;
        UIMgr.Ins.金币TMP.text = 玩家拥有的金币.ToString();
    }

    public void 获得遗物(string name)
    {
        遗物基类List.Add(创建遗物(name));
        刷新遗物背包();
    }

    public void 刷新遗物背包()
    {
        for (int i = 0; i < 遗物基类List.Count; i++)
        {
            遗物实例List[i].遗物 = 遗物基类List[i];
            遗物实例List[i].gameObject.SetActive(true);
            遗物实例List[i].刷新();
        }
    }

    public void 显示遗物详情(遗物基类 遗物)
    {
        遗物详情.SetActive(true);
        详情名称.text = 遗物.Name;
        详情描述.text = 遗物.Dscrp;
        详情图片.sprite = 遗物.遗物图片;
    }

    public 遗物基类 创建遗物 (string name)
    {
        遗物基类 遗物 = name switch
        {
            "村长的传家宝" => new 村长的治愈护符(),
            _ => null,
        };
        return 遗物;
    }
}