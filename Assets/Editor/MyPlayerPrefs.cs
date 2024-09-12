using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MyPlayerPrefs : MonoBehaviour
{
    // Start is called before the first frame update
    [MenuItem("Tools/Clear PlayerPrefs")]
    static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll(); // 清空所有PlayerPrefs  
        Debug.Log("PlayerPrefs have been cleared."); // 在控制台输出日志  
    }
}
