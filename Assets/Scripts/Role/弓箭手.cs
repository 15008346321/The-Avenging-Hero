using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 弓箭手 : Unit
{
    // Start is called before the first frame update
    List<int> BehindCells = new() { 7,8,9 };
    public bool repelTarget = false;

    public override void OnAtkMonent()
    {
        base.OnAtkMonent();
        穿透剑();
    }

    public void 穿透剑()
    {
        SkillPoint++;
        SkillPointIcon[SkillPoint - 1].DOFade(1, 0);
        if (SkillPoint == 3)
        {
            isSkillReady = true;
        }
    }

    public override void ExecuteSkill()
    {
        BattleMgr.Ins.AddMaxUnitRowToTarget(Cell, IsEnemy);
        SkillPoint = 0;
        foreach (var item in SkillPointIcon)
        {
            item.DOFade(0.5f, 0);
        }
        Animator.Play("atk");//后面两方法在动画帧后段调用
    }
}
