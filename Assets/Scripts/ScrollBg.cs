using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBg : MonoBehaviour
{
    public Transform bg1, bg2, roadsNode,enemyNode;
    public float BgSpeed,EnemySpeed;
    public bool MoveBg = true, MoveEnemy = false;
    public static ScrollBg Ins;
    // Start is called before the first frame update
    void Start()
    {
        if (Ins == null) Ins = this;
        else Destroy(Ins);
    }

    // Update is called once per frame
    void Update()
    {

        if (MoveBg)
        {
            float offset = Time.deltaTime * BgSpeed;
            bg1.transform.localPosition = new Vector2(bg1.transform.localPosition.x - offset, 0);
            bg2.transform.localPosition = new Vector2(bg2.transform.localPosition.x - offset, 0);
            roadsNode.transform.localPosition = new Vector2(roadsNode.transform.localPosition.x - offset, 0);

            if (bg1.transform.localPosition.x <= -1919)
            {
                bg1.transform.localPosition = new Vector2(1919, 0);
            }
            if (bg2.transform.localPosition.x <= -1919)
            {
                bg2.transform.localPosition = new Vector2(1919, 0);
            }
            if (roadsNode.gameObject.activeInHierarchy && roadsNode.transform.localPosition.x <= 700)
            {
                MoveBg = false;
            }
            
        }
        if (MoveEnemy)
        {
            float offset = Time.deltaTime * EnemySpeed;
            enemyNode.transform.localPosition = new Vector2(enemyNode.transform.localPosition.x - offset, -140);
            if (enemyNode.gameObject.activeInHierarchy && enemyNode.transform.localPosition.x <= 550)
            {
                BattleMgr.Ins.SetPosSlotAlpha(0.4f);
                MoveEnemy = false;
                BattleMgr.Ins.BattleBtn.gameObject.SetActive(true);
            }
        }
    }
}
