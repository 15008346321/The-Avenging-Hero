using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class 遗物实例 : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,IPointerEnterHandler,IPointerExitHandler
{
    public 遗物基类 遗物;
    public bool isDragging = false; // 标记是否正在拖放
    public Image 遗物图片,遗物的拖动图片;

    private void Update()
    {
        if (isDragging && 遗物.可装备)
        {
            Vector3 globalMousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100));
            遗物的拖动图片.transform.position = globalMousePos;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isDragging && 遗物.可装备)
        {
            遗物的拖动图片.sprite = 遗物.遗物图片;
            遗物的拖动图片.raycastTarget = false;
            isDragging = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isDragging && 遗物.可装备)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject.CompareTag("布阵位置"))
            {
            }
            isDragging = false;
            遗物的拖动图片.raycastTarget = true;
        }
    }

    public void 刷新()
    {
        遗物图片.sprite = 遗物.遗物图片;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        BagMgr.Ins.显示遗物详情(遗物);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        BagMgr.Ins.遗物详情.SetActive(false);
    }
}
