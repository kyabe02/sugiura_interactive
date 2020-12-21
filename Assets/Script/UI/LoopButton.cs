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

    [SerializeField]
    int buttonType = 0;


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
        //内部データも更新
        if (buttonType == 0)
        {
            Database.Instance.storeId = index;
        }
        else if (buttonType == 1) {
            Database.Instance.money = int.Parse(LoopList[index]);
        }
        
    }

    public string Value
    {
        get
        {
            return LoopList[index];
        }
    }

    public void SetLoopList(List<string> list, int defaultIndex=0)
    {
        LoopList = list;
        index = defaultIndex;
    }

}
