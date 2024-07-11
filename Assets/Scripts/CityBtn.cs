using System;
using UnityEngine;
using UnityEngine.UI;

public class CityBtn : MonoBehaviour
{
    Button btn;

    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() => OnClick());
    }

    private void OnClick()
    {
        //int tarCityIndex = 0;
        //if (LevelManager.Ins.TargetCity != "")
        //    tarCityIndex = GameObject.Find(LevelManager.Ins.TargetCity).transform.GetSiblingIndex();
        //    LevelManager.Ins.ChooseBg[tarCityIndex].sprite = Resources.Load<Sprite>("Texture/Icon/unSel");
        //LevelManager.Ins.TargetCity = name;
        //LevelManager.Ins.CityNmaeTMP.text = name; 
        //LevelManager.Ins.CityDesctiptionTMP.text = LevelManager.Ins.CityDescription[name];
        //LevelManager.Ins.DegreeTMP.text = "当前国家探索度:" + LevelManager.Ins.Degrees[tarCityIndex] + "%";
        //LevelManager.Ins.ChooseBg[transform.GetSiblingIndex()].sprite = Resources.Load<Sprite>("Texture/Icon/sel");
        //if(LevelManager.Ins.CurrentCity != name)
        //{
        //    LevelManager.Ins.StartBtn.enabled = true;
        //    LevelManager.Ins.StartBtn.transform.GetComponent<Image>().color = Color.white;
        //    //TODO到达之后将修改当前城市
        //}
        //else
        //{
        //    LevelManager.Ins.StartBtn.enabled = false;
        //    LevelManager.Ins.StartBtn.transform.GetComponent<Image>().color = Color.grey;
        //}
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
