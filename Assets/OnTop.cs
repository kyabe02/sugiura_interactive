using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnTop : MonoBehaviour
{

    [SerializeField]
    Text numZext;

    [SerializeField]
    Text shopZext;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        //表示を更新する
        shopZext.text = Database.Instance.GetShopName();
        numZext.text =  Database.Instance.money.ToString();
    }
}
