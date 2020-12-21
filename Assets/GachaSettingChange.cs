using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaSettingChange : MonoBehaviour
{
    //変更テキスト
    [SerializeField]
    Text numZext;

    [SerializeField]
    InputField fieldText;

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

    public void UpdateDatabase() {
        print("アップデートしました");
        if (shopZext.text == "デニーズ") {
            Database.Instance.storeId = 0;
        }else if (shopZext.text == "松屋")
        {
            Database.Instance.storeId = 1;
        }
        else if (shopZext.text == "マクドナルド")
        {
            Database.Instance.storeId = 2;
        }
        if ((numZext.text != null)) {
            Database.Instance.money = int.Parse(numZext.text);
        }
        
    }

    public void OnEnable()
    {
        //Onになる
        print("Onになりました");
        //表示を更新する
        shopZext.text = Database.Instance.GetShopName();
        numZext.text = Database.Instance.money.ToString();
        fieldText.text = Database.Instance.money.ToString();
        
    }
}
