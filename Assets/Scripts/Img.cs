using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Img : MonoBehaviour
{
    // Start is called before the first frame update
    public Image image;
    void Start()
    {
        image.DOFade(0,3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
