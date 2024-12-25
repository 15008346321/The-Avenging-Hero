using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Menu : MonoBehaviour
{
    public Button CtnBtn, NewGameBtn, SettingBtn, ExitBtn;
    public bool NoRecord;
    public TextMeshProUGUI AreaName;
    public Image AreaLine1,AreaLine2;
    public static Menu Ins;

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

    public void OnClickNewGame()
    {
        InitUnion();
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

    private void InitUnion()
    {
        PlayerPrefs.SetInt("HaveRecord?", 1);
        //添加当前角色 添加任务 添加目的地

    }

    private void ShowSetting()
    {
        
    }

    public void EnterNewLevel()
    {
        AreaName.text = "后山";
        LevelManager.Ins.SetLevel("test");
        AreaName.DOFade(1, 1f).OnComplete(()=>AreaName.DOFade(0, 1f));
        AreaLine1.DOFade(1, 1f).OnComplete(()=> AreaLine1.DOFade(0, 1f));
        AreaLine2.DOFade(1, 1f).OnComplete(()=> AreaLine2.DOFade(0, 1f));
    }

    // Update is called once per frame
    void Update()
    {
    }
}
