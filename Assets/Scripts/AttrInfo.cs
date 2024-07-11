using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AttrInfo : MonoBehaviour
{
    public TextMeshProUGUI NameLabel, SPLabel, HpLabel, AtkLabel, DefLabel, SpeedLabel, SpellLabel, ResistLabel,
        FireLabel, WaterLabel, WindLabel, ThunderLabel, EarthLabel, IceLabel, HolyLabel, ChaosLabel;
    //public TextureProgressBar HpBar, ShieldBar;
    //public TextureRect AttrIcon;
    //public TextureButton[] btns = new TextureButton[12];
    //public Label[] btnTexts = new Label[12];
    public static AttrInfo Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(Instance);
    }

    //public void Init()
    //{
    //    NameLabel    = GetNode<Label>("NameLabel");
    //    HpLabel      = GetNode<Label>("HpLabel");
    //    AtkLabel     = GetNode<Label>("AtkLabel");
    //    DefLabel     = GetNode<Label>("DefLabel");
    //    SpeedLabel   = GetNode<Label>("SpeedLabel");
    //    SpellLabel   = GetNode<Label>("SpellLabel");
    //    ResistLabel  = GetNode<Label>("ResistLabel");
    //    FireLabel    = GetNode<Label>("FireLabel");
    //    WaterLabel   = GetNode<Label>("WaterLabel");
    //    WindLabel    = GetNode<Label>("WindLabel");
    //    ThunderLabel = GetNode<Label>("ThunderLabel");
    //    EarthLabel   = GetNode<Label>("EarthLabel");
    //    IceLabel     = GetNode<Label>("IceLabel");
    //    HolyLabel    = GetNode<Label>("HolyLabel");
    //    ChaosLabel   = GetNode<Label>("ChaosLabel");

    //    HpBar     = GetNode<TextureProgressBar>("HpBar");
    //    ShieldBar = GetNode<TextureProgressBar>("ShieldBar");

    //    AttrIcon = GetNode<TextureRect>("AttrIcon");

    //    var skills = GetNode("Skills");
    //    for (int i = 0; i < skills.GetChildCount(); i++)
    //    {
    //        btns[i]     = skills.GetChild<TextureButton>(i);
    //        btnTexts[i] = (Label)btns[i].GetChild(0);
    //        btns[i].Pressed += () => GD.Print(i);
    //    }
    //}

    public void ShowInfo(Unit u)
    {
        //NameLabel.Text = u.Name;
        //HpLabel.Text = (int)u.Hp + "/" + u.MaxHp;
        //HpBar.MaxValue = u.MaxHp;
        //HpBar.Value = u.Hp;
        //ShieldBar.MaxValue = u.MaxHp;
        //ShieldBar.Value = u.Shield;
        //AtkLabel.Text = "攻击:" + u.Atk;
        //DefLabel.Text = "防御:" + u.Def;
        //SpeedLabel.Text = "速度" + u.Speed;
        //SpellLabel.Text = "法术:" + u.Spell;
        //ResistLabel.Text = "魔抗:" + u.Resist;
        //FireLabel.Text = "火元素:" + u.Fire;
        //WaterLabel.Text = "水元素:" + u.Water;
        //WindLabel.Text = "风元素:" + u.Wind;
        //ThunderLabel.Text = "雷元素:" + u.Thunder;
        //EarthLabel.Text = "土元素:" + u.Earth;
        //IceLabel.Text = "冰元素:" + u.Ice;
        //HolyLabel.Text = "光元素:" + u.Holy;
        //ChaosLabel.Text = "暗元素:" + u.Chaos;
        ////todo 元素属性图标
        ////switch (u.Attr)
        //btnTexts[0].Text = u.普攻详情[0];
        //int i, j;
        //GD.Print("u.追打.Length" + u.追打.Length);
        //for (i = 0; i < u.追打详情列表.Count; i++)
        //{
        //    btnTexts[i + 1].Text = u.追打详情列表[i][0];
        //}
        //for (j = 0; j < u.被动详情列表.Count; j++)
        //{
        //    if (u.被动详情列表[j] == null) break;
        //    btnTexts[j + i + 1].Text = u.被动详情列表[j][0];
        //}
    }
}
