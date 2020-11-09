using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UniBlock;

public class Database : MonoBehaviour
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
    [Header("ユーザー外部入力")]
    [SerializeField]
    Dropdown storeDropDown;
    [SerializeField]
    InputField calorieInputField;
    [SerializeField]
    InputField moneyInputField;

    //データベース
    private int storeId = 0;//お店番号(DropDownから取得される)
    private int calorie = 1000;
    private int money = 1000;

    //内部用変数
    PanelManager next;              //次に遷移する用のパネル
    PanelManager hover;             //重ねて表示する用のパネル

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

    //================================
    // データベース関連
    //================================
    public void SetStoreID() {
        storeId = storeDropDown.value;
        Debug.Log("idが"+ storeId + "に設定されました");
    }

    public void SetCalorie() {
        calorie = int.Parse(calorieInputField.text);
    }

    public void SetMoney()
    {
        money = int.Parse(moneyInputField.text);
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
