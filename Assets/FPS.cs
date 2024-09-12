using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPS : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI tmp;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float FPS = 1 / Time.deltaTime;
        tmp.text = ((int)FPS).ToString();
    }
}
