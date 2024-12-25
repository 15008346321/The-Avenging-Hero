using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class 布阵UI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnitData unitData;
    public bool isDragging = false; // 标记是否正在拖放
    public Image 角色图片; // 用于记录拖放过程中实例化出来的角色图片对象

    private void Update()
    {
        if (isDragging && 角色图片 != null)
        {
            Vector3 globalMousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100));
            角色图片.transform.position = globalMousePos;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isDragging)
        {
            角色图片.sprite = unitData.角色图片;
            角色图片.raycastTarget = false;
            isDragging = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isDragging)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject.CompareTag("布阵位置"))
            {
                UnitData ud = TeamManager.Ins.TeamData.Find(td => td.Cell == (hit.collider.gameObject.transform.GetSiblingIndex() + 1));
                //交换位置
                if (ud != null)
                {
                    (unitData.Cell, ud.Cell) = (ud.Cell, unitData.Cell);
                    transform.position = BattleMgr.Ins.ourObj.transform.GetChild(unitData.Cell - 1).position;
                    UIMgr.Ins.布阵UI列表.Find(img => img.角色图片.sprite == ud.角色图片).transform.position = BattleMgr.Ins.ourObj.transform.GetChild(ud.Cell - 1).position;
                }
                //拖动位置
                else
                {
                    unitData.Cell = hit.collider.gameObject.transform.GetSiblingIndex() + 1;
                    transform.position = BattleMgr.Ins.ourObj.transform.GetChild(unitData.Cell - 1).position;
                }
            }
            isDragging = false;
            角色图片.raycastTarget = true;
        }
    }
}
