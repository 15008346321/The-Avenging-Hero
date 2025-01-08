using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class BagMgr : MonoBehaviour
{
    public GameObject 材料背包, 遗物详情;
    public int 玩家拥有的金币,补给;
    public List<遗物基类> 遗物基类List = new();
    public List<遗物实例> 遗物实例List;
    public TextMeshProUGUI 详情名称, 详情描述,金币TMP,补给TMP;
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
    public IEnumerator 金币变动(int value)
    {
        int startCoin = 玩家拥有的金币;
        int endCoin = startCoin + value;
        for (int i = startCoin; i < endCoin; i++)
        {
            玩家拥有的金币 = i + 1;
            金币TMP.text = UIMgr.Ins.TMP图片化文字(玩家拥有的金币.ToString());
            yield return new WaitForSeconds(1f/value);
        }
        玩家拥有的金币 = endCoin;
        yield break;
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

    public IEnumerator 补给变动(int value)
    {
        int startSupply = 补给;
        int endSupply = startSupply + value;
        if (endSupply < 0)
        {
            endSupply = 0;
        }
        for (int i = startSupply; i < endSupply; i++)
        {
            补给 = i + 1;
            补给TMP.text = UIMgr.Ins.TMP图片化文字(补给.ToString());
            yield return new WaitForSeconds(1f/value);
        }
        补给 = endSupply;
        yield break;
    }
}