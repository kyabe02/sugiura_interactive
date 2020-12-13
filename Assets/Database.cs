using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Database : SingletonMonoBehaviour<Database>
{
    //データフレーム形式
    public DataFrame df;

    private void Start()
    {
        if (Utility.ExistJson("セーブデータ"))
        {
            Load();
        }
        else {
            Reset();
        }
    }

    //データをロードする
    public void Load() {
        df = Utility.ReadJson("セーブデータ");
    }

    //データをセーブする
    public void Save() {
        df.SaveJson("セーブデータ");
    }

    public void Reset()
    {
        df = Utility.ReadCSV("menu");
        Save();
    }

    //あるレストランIDのメニューIDをランダムに取得する
    public int RandomMenu(int restaurantID)
    {
        //特定のレストランだけを抽出する
        var copy = df.Where("RestaurantID", restaurantID);
        //ランダムな整数値
        int randomValue = Random.Range(0, copy.LineSize());
        Debug.Log(randomValue);
        //ランダム行のMenuIDを取得する
        string item = copy.GetRow(randomValue).Get("MenuID", 0);
        return int.Parse(item);
    }
    public string GetName(int foodID)
    {
        return df.Where("MenuID", foodID).Get("MenuName", 0);
    }

    public int GetPrice(int foodID) {
        return int.Parse(df.Where("MenuID", foodID).Get("Price", 0));
    }

    public int GetCalorie(int foodID)
    {
        return int.Parse(df.Where("MenuID", foodID).Get("Calorie", 0));
    }

    public float GetSalt(int foodID)
    {
        print(df.Where("MenuID", foodID).Get("Salt", 0));
        return float.Parse(df.Where("MenuID", foodID).Get("Salt", 0));
    }

    public int GetFlag(int foodID)
    {
        return int.Parse(df.Where("MenuID", foodID).Get("Flag", 0));
    }

    public void SetWatched(int foodID)
    {
        df.Set("Flag",foodID,"1");
    }

    public void SetEaten(int foodID)
    {
        df.Set("Flag", foodID, "2");
    }

    public int GetEatenCount(int restaurantID, int eatFlag) {
        //特定のレストランだけを抽出する
        return df.Where("RestaurantID", restaurantID).Where("Flag", eatFlag).LineSize();
    }
}


//フードリスト
[System.Serializable]
public class Restaurant
{
    //レストランの名称
    public string name;
    //フードリスト
    public List<FoodItem> food = new List<FoodItem>();
}
