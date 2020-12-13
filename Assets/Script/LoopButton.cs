using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoopButton : MonoBehaviour
{

    [SerializeField]
    int defaultIndex = 0;

    [SerializeField]
    List<string> LoopList;


    int index = 0;
    Text text;
    public void Start()
    {
        text = GetComponentInChildren<Text>();
        index = defaultIndex;
        text.text = LoopList[index];
    }

    public void Next()
    {
        index = (index + 1) % LoopList.Count;
        text.text = LoopList[index];
    }

    public string Value
    {
        get
        {
            return LoopList[index];
        }
    }

}
