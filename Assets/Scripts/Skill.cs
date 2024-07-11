[System.Serializable]
public class Skill
{
    public string Name { get; set; }
    public string Type { get; set; }
    public string Dscrp { get; set; }
    public string Code { get; set; }
    public string AtkType { get; set; }
    public int 普攻计数 { get; set; }
    public int 普攻总数 { get; set; }
    public int 追打计数 { get; set; }
    public int 追打总数 { get; set; }
    public int 充能计数 { get; set; }
    public int 充能总数 { get; set; }
    public int 被动计数 { get; set; }
    public int 被动总数 { get; set; }
    public string 追击状态 { get; set; }
    public string 造成状态 { get; set; }

    public Skill(string[] skillInfo)
    {
        // 解析传入的字符串并初始化属性
        Name = skillInfo[1];
        Type = skillInfo[2];
        AtkType = skillInfo[3];
        Dscrp = skillInfo[5];
        Code = skillInfo[6];
        if (Type == "普攻")
        {
            普攻总数 = skillInfo[7] == "" ? 1 : int.Parse(skillInfo[7]);
            普攻计数 = 普攻总数;
            if(skillInfo[10]!="") 造成状态 = skillInfo[10];
        }

        if (Type == "追打")
        {

            追打总数 = skillInfo[8] == "" ? 1 : int.Parse(skillInfo[8]);
            追打计数 = 追打总数;
            追击状态 = skillInfo[9];
            造成状态 = skillInfo[10];
        }

        if (Type == "被动")
        {
            被动总数 = skillInfo[11] == "" ? 1 : int.Parse(skillInfo[11]);
            被动计数 = 追打总数;
            充能总数 = skillInfo[12] == "" ? -1 : int.Parse(skillInfo[12]);
            充能计数 = 0;
        }
    }
}