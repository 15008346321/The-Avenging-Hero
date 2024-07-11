using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class StartPage : MonoBehaviour
{
    public static StartPage Instance;
    public Button btn;
    public TextMeshProUGUI start, text;
    public int index;
    public Transform tips;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(Instance);
    }

    private void Start()
    {
        InitStartPage();
    }

    public void InitStartPage()
    {

        print("msg InitStartPage 1");
        start = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        start.DOFade(0, 1).SetLoops(-1, LoopType.Yoyo);
        print("msg InitStartPage 2");

        text = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        index = 0;

        tips = transform.GetChild(3);

        btn = GetComponent<Button>();
        btn.onClick.AddListener(ShowText);
    }

    public void ShowText()
    {
        if (index == CSVManager.Ins.StartText.Count)
        {
            transform.gameObject.SetActive(false);
            EventsMgr.Ins.DailyNode.gameObject.SetActive(true);
        }
        if (index == 0)
        {
            start.DOKill();
            start.gameObject.SetActive(false);

            tips.gameObject.SetActive(true);
            tips.DOMoveY(150,0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);

            transform.GetChild(0).gameObject.SetActive(false);
            text.gameObject.SetActive(true);
        }
        if (index > CSVManager.Ins.StartText.Count - 1) return;
        text.text = CSVManager.Ins.StartText[index][0];
        index += 1;
    }
}
