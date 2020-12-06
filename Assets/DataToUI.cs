using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UniBlock;

public class DataToUI : MonoBehaviour
{
    //外部設定必要
    [Header("パネル")]
    [SerializeField]
    PanelManager currentPanel;      //現在表示しているパネル

    [Header("UIエフェクト")]
    [SerializeField]
    Image fade;                     //フェード機能用のイメージ
    [SerializeField]
    Image hoverBlack;
    [SerializeField]
    GameObject effectPlace;          //エフェクト置き場

    //外部連携のUI
    [Header("ユーザー入力")]
    [SerializeField]
    Dropdown storeDropDown;
    [SerializeField]
    InputField calorieInputField;
    [SerializeField]
    InputField moneyInputField;

    [Header("ガチャ結果関連")]
    [SerializeField]
    VerticalLayoutGroup gachaResultBox;
    [SerializeField]
    FoodItem foodItem;
    [SerializeField]
    Text SumPriceText;
    [SerializeField]
    Text SumCalText;
    [SerializeField]
    Text SumSaltText;

    [Header("図鑑関連")]
    [SerializeField]
    VerticalLayoutGroup liblaryBox;
    private int zukanStoreId=0;
    private int currentDisplayType = 0;//0が図鑑、１がガチャ


    [Header("フードホバー関連")]
    [SerializeField]
    Text EatDecideText;
    [SerializeField]
    PanelManager EatDecidePanel;
    [SerializeField]
    Text EatCancelText;
    [SerializeField]
    PanelManager EatCancelPanel;
    [SerializeField]
    PanelManager NotEatPanel;

    //データベース
    private int storeId = 0;//お店番号(DropDownから取得される)
    private int calorie = 1000;
    private int money = 1000;

    //内部用変数
    PanelManager next;              //次に遷移する用のパネル
    PanelManager hover;             //重ねて表示する用のパネル
    int currentMenuId = 0;

    void Start()
    {
        //ここに全体の初期化を記入したい
    }

    void Update()
    {
        
    }


    //================================
    // UI関連
    //================================

    //指定したパネルへ遷移する
    public void Transition(PanelManager next) {
        this.next = next;

        //フローチャート作成
        ProcessBlock block = new Sequence(
            new FadeOn(fade ,0.2f),//0.2秒かけて黒くする
            new Function(ChangePanel),//パネルを変更する
            new Wait(0.2f),//0.2秒待機する
            new FadeOff(fade, 0.2f)//0.2秒かけて透明にする
            );

        //フローチャート実行
        block.Activate();
    }

    //指定したパネルへ遷移する
    public void TransitionWithGacha(PanelManager next)
    {
        this.next = next;

        //フローチャート作成
        ProcessBlock block = new Sequence(
            new FadeOn(fade, 0.2f),//0.2秒かけて黒くする
            new Function(ChangePanel),//パネルを変更する
            new Function(SetGachaResule),
            new Wait(0.2f),//0.2秒待機する
            new FadeOff(fade, 0.2f)//0.2秒かけて透明にする
            );

        //フローチャート実行
        block.Activate();
    }

    public void TransitionWithZukan(PanelManager next)
    {
        this.next = next;

        //フローチャート作成
        ProcessBlock block = new Sequence(
            new FadeOn(fade, 0.2f),//0.2秒かけて黒くする
            new Function(ChangePanel),//パネルを変更する
            new Function(SetLiblary),
            new Wait(0.2f),//0.2秒待機する
            new FadeOff(fade, 0.2f)//0.2秒かけて透明にする
            );

        //フローチャート実行
        block.Activate();
    }

    //指定したパネルをホバーさせる
    public void Hover(PanelManager hover) {
        this.hover = hover;

        //半透明を表示中のパネルの上にセット
        hoverBlack.color = new Color(0, 0, 0, 0.5f);
        hoverBlack.raycastTarget = true;
        hoverBlack.transform.parent = currentPanel.transform;

        //ホバーを表示
        hover.gameObject.SetActive(true);
    }

    //ホバーを解除する
    public void UnHover() {
        if (hover != null) {
            //半透明を解除
            hoverBlack.color = new Color(0, 0, 0, 0);
            hoverBlack.raycastTarget = false;
            hoverBlack.transform.parent = effectPlace.transform;

            //ホバーを非表示
            hover.gameObject.SetActive(false);

            hover = null;
        }
    }

    private void ChangePanel() {
        if (next != null) {
            //表示を切替
            currentPanel.gameObject.SetActive(false);
            next.gameObject.SetActive(true);
            //データを切替
            currentPanel = next;
            next = null;
        }
    }

    //contentにガチャ結果を送る
    public void SetGachaResule() {
        //リザルトを空にする
        DestroyChild(gachaResultBox.transform);
        currentDisplayType = 0;

        int sum = 0;
        int sumCal = 0;
        int sumSalt = 0;
        int retryCounter = 0;
        while (true) {
            //フードIDをランダムで取得
            int foodID = Database.Instance.RandomMenu(storeId);
            
            //金額をメモ
            int price = Database.Instance.GetPrice(foodID);

            //金額によってリトライするか設定
            if (price + sum > money)
            {
                retryCounter += 1;
            }
            else {
                //フードプレハブを生成
                FoodItem instance = Instantiate(foodItem);
                instance.gameObject.SetActive(true);
                //プレハブにフードIDを流し込み
                instance.SetFoodID(foodID);
                instance.itemType = 0;
                //プレハブの表示準備OKにする
                instance.Display();
                //Contentに突っ込む
                instance.transform.parent = gachaResultBox.transform;
                instance.transform.localScale = new Vector3(1, 1, 1);
                //金額の更新
                sum += price;
                sumCal += Database.Instance.GetCalorie(foodID);
                sumSalt += Database.Instance.GetCalorie(foodID);
                //Newの更新
                if (Database.Instance.GetFlag(foodID)==0) {
                    Database.Instance.SetWatched(foodID);
                }
            }

            //10回試してもダメなら終了
            if (retryCounter >= 10) {
                break;
            }
        }

        SumPriceText.text = sum.ToString() + "円";
        SumCalText.text = sumCal.ToString() + "Cal";
        SumSaltText.text = "塩分" + sumSalt.ToString();

    }

    public void SetZukan(int zukanStoreId) {
        this.zukanStoreId = zukanStoreId;
    }

    void SetLiblary() {
        //図鑑を空にする
        DestroyChild(liblaryBox.transform);
        currentDisplayType = 1;

        var items = Database.Instance.df.Where("RestaurantID", zukanStoreId);
        for (int i = 0; i < items.LineSize(); i++) {
            //フードプレハブを生成
            FoodItem instance = Instantiate(foodItem);
            instance.gameObject.SetActive(true);
            instance.itemType = 1;
            //プレハブにフードIDを流し込み
            var currentFoodId = int.Parse(items.Get("MenuID", i));
            instance.SetFoodID(currentFoodId);
            //プレハブの表示準備OKにする
            instance.Display();
            //Contentに突っ込む
            instance.transform.parent = liblaryBox.transform;
            instance.transform.localScale = new Vector3(1, 1, 1);
            //金額の更新
        }
    }

    private void DestroyChild(Transform parentTransform) {
        foreach (Transform childTransform in parentTransform)
        {
            Destroy(childTransform.gameObject);
        }
    }

    public void SetCurrentFoodID(FoodItem foodItem) {
        currentMenuId = foodItem.GetFoodID();
    }

    public void DisplayFoodHover() {
        if (currentDisplayType == 0)
        {
            if (Database.Instance.GetFlag(currentMenuId) == 1)
            {
                EatDecideText.text = Database.Instance.GetName(currentMenuId);
                Hover(EatDecidePanel);
            }
            else if (Database.Instance.GetFlag(currentMenuId) == 2)
            {
                EatCancelText.text = Database.Instance.GetName(currentMenuId);
                Hover(EatCancelPanel);
            }
        }
        else if (currentDisplayType == 1)
        {
            if (Database.Instance.GetFlag(currentMenuId) == 0)
            {
                Hover(NotEatPanel);
            }
            else if (Database.Instance.GetFlag(currentMenuId) == 1)
            {
                EatDecideText.text = Database.Instance.GetName(currentMenuId);
                Hover(EatDecidePanel);
            }
            else if (Database.Instance.GetFlag(currentMenuId) == 2)
            {
                EatCancelText.text = Database.Instance.GetName(currentMenuId);
                Hover(EatCancelPanel);
            }
        }

    }

    public void SetEaten() {
        Database.Instance.SetEaten(currentMenuId);
    }

    public void SetWatched()
    {
        Database.Instance.SetWatched(currentMenuId);
    }

    //表示を更新する
    public void ResetDisplay() {
        //全ての子供を取得する
        foreach (FoodItem child in gachaResultBox.transform.GetComponentsInChildren<FoodItem>())
        {
            var act = child.gameObject.GetComponent<FoodItem>();
            act.Display();
        }
        foreach (FoodItem child in liblaryBox.transform.GetComponentsInChildren<FoodItem>())
        {
            var act = child.gameObject.GetComponent<FoodItem>();
            act.Display();
        }
    }

    //================================
    // データベース関連
    //================================
    public void SetStoreID(Dropdown dropDown) {
        storeId = dropDown.value;
        Debug.Log("idが"+ storeId + "に設定されました");
    }

    public void SetCalorie() {
        calorie = int.Parse(calorieInputField.text);
    }

    public void SetMoney(InputField inputField)
    {
        money = int.Parse(inputField.text);
        Debug.Log("金額が" + money + "に設定されました");
    }

    public void Save() {
        Database.Instance.Save();
    }

    public void Reset()
    {
        Database.Instance.Reset();
    }
}

/*
 自作のフローチャート機能の解説
機能は以下の階層で成り立っている。下に行くほど制約が厳しい
BlockBase：全てのブロックの親。
- ProcessBlock：確実な終了が保証されている、実行可能なブロック
  - FiniteBlock：確実かつ有限時間での終了が保証されているブロック
    - TimerBlock：有限時間かつ決められた時間での終了が保証されているブロック
      -InstantBlock：１フレーム内での終了が保証されているブロック
- 
 */
