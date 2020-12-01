using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodItem : MonoBehaviour
{
    [SerializeField]
    int foodId;
    //表示先の設定
    [SerializeField] Text foodNameText;
    [SerializeField] Text foodPriceText;
    [SerializeField] Text foodCalText;
    [SerializeField] Text newText;

    //イメージ設定
    [SerializeField] Image foodImage;
    //表示させたいイメージ
    [SerializeField] Sprite imageItem;

    //フードIDをセットする
    public void SetFoodID(int id) {
        foodId = id;
    }

    public int GetFoodID()
    {
        return foodId;
    }

    //データ表示の差分を更新する
    public void Display() {
        DataFrame df = Database.Instance.df.Where("MenuID", foodId);
        //テキストを流し込む
        foodNameText.text = df.Get("MenuName", 0);
        foodPriceText.text = df.Get("Price", 0) + "円";
        foodCalText.text = df.Get("Calorie", 0) + "Cal";

        int flag = int.Parse(df.Get("Flag", 0));
        //フラッグにより、表示を変化させる
        if (flag == 0) {
            foodImage.sprite = null;
            newText.text = "NEW";
        }
        else if (flag == 1)
        {
            foodImage.sprite = null;
            newText.text = "";
        }
        else if(flag == 2) {
            foodImage.sprite = imageItem;
            newText.text = "";
        }

    }

    //食べた時
    void SelectDoneEat() {
        //データベースを変更する
        //データを更新する
    }
}

