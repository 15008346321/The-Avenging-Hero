using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteMgr : MonoBehaviour
{
    string currentCode;
    public List<Unit> targets = new(); 

    public static ExecuteMgr Ins;
    private void Awake()
    {
        if (Ins == null) Ins = this;
        else Destroy(Ins);
    }

    //public void ExecuteCode(string code, string log = null)
    //{
    //    //commands:"T1+Ran3E:Atk+Fire&T1:大概率击退"
    //    //&分割出每一个指令
    //    //:分割出所有目标和具体
    //    //+分割单个目标和内容
    //    string[] commands = code.Replace("\"","").Split('&');
    //    //command: "T1+RanE3:Atk+Fire" "T1:大概率击退"
    //    foreach (string command in commands)
    //    {

    //        currentCode = command;
    //        string Type = "";
    //        //获取资源
    //        if (command.Contains("攻击"))
    //        {
    //            currentCode = command.Replace("攻击", "");
    //            Type = "攻击";
    //        }
    //        if (command.Contains("起手"))
    //        {
    //            ExecuteStartComb();
    //            continue;
    //        }
    //        if (command.Contains("伤害"))
    //        {
    //            currentCode = command.Replace("伤害", "");
    //            Type = "伤害";
    //        }
    //        if (command.Contains("治疗"))
    //        {
    //            currentCode = command.Replace("治疗", "");
    //            Type = "治疗";
    //        }
    //        if (command.StartsWith("状态"))
    //        {
    //            currentCode = command.Replace("状态", "");
    //            Type = "状态";
    //        }
    //        if (command.StartsWith("面板属性")) 
    //        {
    //            currentCode = command.Replace("面板属性", "");
    //            Type = "面板属性";
    //        }
    //        if (command.StartsWith("属性"))
    //        {
    //            currentCode = command.Replace("属性", "");
    //            Type = "属性";
    //        }
    //        if (command.StartsWith("结晶"))
    //        {
    //            currentCode = command.Replace("结晶", "");
    //            BagManager.Ins.AddAttrStoneToBag(currentCode[0].ToString(), currentCode[1]);
    //            continue;
    //        }
    //        if (command.StartsWith("荣誉"))
    //        {
    //            currentCode = command.Replace("荣誉", "");
    //            BagManager.Ins.AddPoint(currentCode[0].ToString(), currentCode[1]);
    //            continue;
    //        }
    //        if (command.StartsWith("技能书"))
    //        {
    //            currentCode = command.Replace("技能书", "");
    //            BagManager.Ins.AddSkillToBag(currentCode, 1);
    //            continue;
    //        }
    //        if (command.StartsWith("金币"))
    //        {
    //            currentCode = command.Replace("金币", "");
    //            BagManager.Ins.AddGold(int.Parse(currentCode));
    //            continue;
    //        }
    //        if (command.StartsWith("遗物"))
    //        {
    //            currentCode = command.Replace("遗物", "");
    //            BagManager.Ins.AddRelics(currentCode);
    //            continue;
    //        }

    //        var TC = currentCode.Split(":");
    //        var 所有目标 = TC[0].Split("+");
    //        var 所有效果 = TC[1].Split("+");

    //        targets.Clear();
    //        foreach (var 单一目标 in 所有目标)
    //        {
    //            if (单一目标.StartsWith("Self")) targets.Add(BattleMgr.Ins.AtkU);
    //            if (单一目标.StartsWith("T1")) targets.Add(BattleMgr.Ins.TeamMain[0]);
    //            if (单一目标.StartsWith("T2")) targets.Add(BattleMgr.Ins.TeamMain[1]);
    //            if (单一目标.StartsWith("T3")) targets.Add(BattleMgr.Ins.TeamMain[2]);
    //            if (单一目标.StartsWith("T4")) targets.Add(BattleMgr.Ins.TeamMain[3]);
    //            if (单一目标.StartsWith("E1")) targets.Add(BattleMgr.Ins.EnemyMain[0]);
    //            if (单一目标.StartsWith("E2")) targets.Add(BattleMgr.Ins.EnemyMain[1]);
    //            if (单一目标.StartsWith("E3")) targets.Add(BattleMgr.Ins.EnemyMain[2]);
    //            if (单一目标.StartsWith("E4")) targets.Add(BattleMgr.Ins.EnemyMain[3]);
    //            //if (单一目标.StartsWith("Tar"))targets.Add(BattleMgr.Ins.获取正面单个目标());
    //            if (单一目标.StartsWith("Ran")) GetRanUnit(单一目标.Replace("Ran",""));
    //            if (单一目标.StartsWith("小队生命值最低")) GetRanUnit(单一目标.Replace("Ran",""));
    //            if (单一目标.StartsWith("ALLTM")) foreach(var item in BattleMgr.Ins.TeamMain) targets.Add(item);
    //            else if (单一目标.StartsWith("ALLEM")) foreach (var item in BattleMgr.Ins.EnemyMain) targets.Add(item);
    //            else if (单一目标.StartsWith("ALLT")) foreach (var item in BattleMgr.Ins.Team) targets.Add(item);
    //            else if (单一目标.StartsWith("ALLE")) foreach (var item in BattleMgr.Ins.Enemys) targets.Add(item);
    //            else if (单一目标.StartsWith("ALL")) foreach (var item in BattleMgr.Ins.AllUnit) targets.Add(item);
    //        }
    //        if(Type != "治疗") BattleMgr.Ins.AtkedU = targets[0];

    //        if (Type == "攻击") ExecuteAtk();
    //        if (Type == "造成状态") ExecuteDebuff();
    //        if (Type == "战时属性") ExecuteBattleAttr();
    //        if (Type == "面板属性") ExecuteBaseAttr();

    //        //Tstr: "T1+RanE3"

    //        //t: "T1"小队一号位 "E2"敌人二号位 "Ran3E"随机敌人3个 "AllET"所有敌人和小队单位
    //        //"Bag" 背包资源 包括金钱体力等 "P"玩家主角单位
            

            
    //    }
    //    //AttrInfo.Instance.ShowInfo(BattleMgr.Instance.player);
    //}

    public void ExecuteAtk()
    {
        //if (currentCode.Split(":").Length > 1)
        //{
        //    var Cstr = currentCode.Split(":")[1];
        //    var codes = Cstr.Split("+");
        //    foreach (var c in codes)
        //    {
        //        //计算伤害
        //        int damage = 0;
        //        if (c.Contains("Atk")) damage += BattleMgr.Ins.AtkU.Atk;
        //        if (c.Contains("Fire")) damage += BattleMgr.Ins.AtkU.Fire;
        //        if (c.Contains("Water")) damage += BattleMgr.Ins.AtkU.Water;
        //        if (c.Contains("Wind")) damage += BattleMgr.Ins.AtkU.Wind;
        //        if (c.Contains("Thunder")) damage += BattleMgr.Ins.AtkU.Thunder;
        //        if (c.Contains("Earth")) damage += BattleMgr.Ins.AtkU.Earth;
        //        BattleMgr.Ins.currentDamage = damage;
        //        if (targets.Count > 0)
        //        {
        //            foreach (var item in targets)
        //            {
        //                item.Hp -= damage;
        //            }
        //            break;
        //        }
        //    }
        //}
    }

    public void GetRanUnit(string tar)
    {
        int num = tar[0];
        List<Unit> T;
        T = tar[1] == 'T' ? BattleMgr.Ins.玩家阵营单位列表 : BattleMgr.Ins.敌人阵营单位列表;
        if (num >= T.Count) foreach (var item in T) targets.Add(item);
        else
        {
            int i = 0;
            while (i<num)
            {
                int ran = Random.Range(0, T.Count);
                if (!targets.Contains(T[ran]))
                {
                    targets.Add(T[ran]);
                    i++;
                }
            }
        }
    }

    public IEnumerator Wait(float f)
    {
        yield return new WaitForSeconds(f); 
    }
    public void ExecuteStartComb()
    {
        currentCode = currentCode.Replace("起手", "");
        float maxNum = 0;
        if (currentCode == "必定") maxNum = 1;
        if (currentCode == "大概率") maxNum = 0.75f;
        if (currentCode == "概率") maxNum = 0.5f;
        if (currentCode == "小概率") maxNum = 0.25f;
        var c = Jugde(maxNum);

        if (c)
        {
            //BattleMgr.Ins.当前追打状态 = BattleMgr.Ins.AtkU.NormalAtk.造成状态;
            //StartCoroutine(BattleMgr.Ins.进行追打());
        }
    }

    public void ExecuteComb()
    {
        //BattleMgr.Ins.当前追打状态 = BattleMgr.Ins.CombU.Comb.造成状态;
        print("msg 当前追打状态" + BattleMgr.Ins.当前追打状态);
    }

    public void ExecuteDebuff()
    {
        string a = currentCode.Replace("造成状态", "");
        string[] b = a.Split("*");
        var c = Jugde(float.Parse(b[1]));
        if (c)
        {
            BattleMgr.Ins.当前追打状态 = b[0];
        }
    }

    public void ExecuteBattleAttr()
    {
        string a = currentCode.Replace("战时属性", "");
        string[] parts = a.Split(new char[] { '+', '-' }, 2);
        string attributeName = parts[0];
        char operation = a.Replace(parts[0], "")[0];
        int numericValue = int.Parse(parts[1]);

        foreach (Unit u in targets)
        {
            switch (attributeName)
            {
                case "Hp":
                    u.Hp += operation == '+' ? numericValue : -numericValue;
                    break;
                case "MaxHp":
                    u.MaxHp += operation == '+' ? numericValue : -numericValue;
                    break;
                case "Atk":
                    u.Atk += operation == '+' ? numericValue : -numericValue;
                    break;
                //case "Fire":
                //    u.Fire += operation == '+' ? numericValue : -numericValue;
                //    break;
                //case "Water":
                //    u.Water += operation == '+' ? numericValue : -numericValue;
                //    break;
                //case "Wind":
                //    u.Wind += operation == '+' ? numericValue : -numericValue;
                //    break;
                //case "Thunder":
                //    u.Thunder += operation == '+' ? numericValue : -numericValue;
                //    break;
                //case "Earth":
                //    u.Earth += operation == '+' ? numericValue : -numericValue;
                //    break;
            }
            //AttrInfo.Instance.ShowInfo(u);
        }
    }

    public void ExecuteBaseAttr()
    {
        //string[] code = currentCode.Split(":");
        //string[] parts = code[1].Split(new char[] { '+', '-' }, 2);
        //string attributeName = parts[0];
        //char operation = code[1].Replace(parts[0], "")[0];
        //int numericValue = int.Parse(parts[1]);

        //foreach (Unit u in targets)
        //{
        //    foreach (var item in TeamManager.Ins.TeamData)
        //    {
        //        if (item.Name == u.name)
        //        {
        //            switch (attributeName)
        //            {
        //                case "MaxHp":
        //                    item.MaxHp += operation == '+' ? numericValue : -numericValue;
        //                    break;
        //                case "Atk":
        //                    item.Atk += operation == '+' ? numericValue : -numericValue;
        //                    break;
        //                case "Fire":
        //                    item.Fire += operation == '+' ? numericValue : -numericValue;
        //                    break;
        //                case "Water":
        //                    item.Water += operation == '+' ? numericValue : -numericValue;
        //                    break;
        //                case "Wind":
        //                    item.Wind += operation == '+' ? numericValue : -numericValue;
        //                    break;
        //                case "Thunder":
        //                    item.Thunder += operation == '+' ? numericValue : -numericValue;
        //                    break;
        //                case "Earth":
        //                    item.Earth += operation == '+' ? numericValue : -numericValue;
        //                    break;
        //            }
        //        }
        //    }
        //}
    }

    public bool Jugde(float percentage)
    {
        var ranNum = Random.Range(1, 100);
        if (ranNum < percentage * 100) return true;
        else return false;
    }
}
