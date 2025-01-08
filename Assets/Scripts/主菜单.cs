using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class 主菜单 : MonoBehaviour
{
    public Button CtnBtn, NewGameBtn, SettingBtn, ExitBtn;
    public bool NoRecord;
    public static 主菜单 Ins;

    private void Awake()
    {
        if (Ins == null) Ins = this;
        else Destroy(Ins);
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        if (PlayerPrefs.GetInt("HaveRecord?") != 1)
        {
            CtnBtn.gameObject.SetActive(false);
        }
    }

    public void OnClickCtn()
    {
        InitRecord();
    }

    public void 当点击出发()
    {
        初始化村庄();
    }

    public void OnClickSetting()
    {
        ShowSetting();
    }


    public void OnClickExit()
    {

    }

    private void InitRecord()
    {
        
    }

    private void 初始化村庄()
    {
        PlayerPrefs.SetInt("HaveRecord?", 1);
        TeamMgr.Ins.生成初始角色();
        UIMgr.Ins.显示小队();
        UIMgr.Ins.更换地图("村庄");
        LevelMgr.Ins.SetLevel("后山");
        UIMgr.Ins.提示OBJ.SetActive(true);
        UIMgr.Ins.村长BTN.gameObject.SetActive(true);
        //添加当前角色 添加任务 添加目的地
    }

    private void ShowSetting()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
}
