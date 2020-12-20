using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public static class Utility
{
    //CSVを読み込み、二次元配列データ(DataFrame)に格納する
    public static DataFrame ReadCSV(string csvName)
    {
        DataFrame df = new DataFrame();
        // csvをロード
        TextAsset csv = Resources.Load(csvName) as TextAsset;
        //リーダをテキストとして読み込み
        StringReader reader = new StringReader(csv.text);

        //横の一行ずつで処理する
        for (int y = 0; reader.Peek() > -1; y++)
        {
            //Debug.Log(y);
            // ','ごとに区切って配列へ格納
            string line = reader.ReadLine();
            string[] cell = line.Split(',');
            for (int x = 0; x < cell.Length; x++)
            {
                //Debug.Log(cell[x]);
                if (y == 0)
                {
                    CsvDataLine varticalLine = new CsvDataLine();
                    varticalLine.name = cell[x];
                    df.lines.Add(varticalLine);
                }
                else
                {
                    df.lines[x].items.Add(cell[x]);
                }
            }
        }

        //データを返却する
        return df;
    }

    //Resourcesフォルダ内にあるJsonファイルを読み込み、Fataframe型に変換する
    public static DataFrame ReadJson(string name) {

        //データ読み込み
        StreamReader streamReader;
        string path = UnityEngine.Application.persistentDataPath + "/" + name + ".json";
        //string path = Application.dataPath + "/Resources/" + name + ".json";
        streamReader = new StreamReader(path);
        string inputString = streamReader.ReadToEnd();
        streamReader.Close();

        //JSONを文字列に変換
        inputString = inputString.Replace("[{", "").Replace("}]", "");
        DataFrame df = new DataFrame();
        string[] groups = inputString.Split(new string[] { "},{" }, StringSplitOptions.None);
        for (int y = 0; y < groups.Length; y++) {
            var group = groups[y];
            string[] items = group.Split(new string[] { "\",\"" }, StringSplitOptions.None);
            if (y == 0) {
                //列を作成する
                for (int x = 0; x < items.Length; x++) {
                    CsvDataLine line = new CsvDataLine();
                    line.name = GetKey(items[x]);
                    df.lines.Add(line);

                }
            }

            //要素を代入する
            for (int x = 0; x < items.Length; x++)
            {
                df.lines[x].items.Add(GetValue(items[x]));
            }
        }

        return df;
    }

    public static bool ExistJson(string name) {
        string path = UnityEngine.Application.persistentDataPath + "/" + name + ".json";
        //string path = Application.dataPath + "/Resources/" + name + ".json";
        if (System.IO.File.Exists(path)) {
            return true;
        }
        return false;
    }

    private static string GetKey(string text) {
        string item = text.Split(new string[] { "\":\"" }, StringSplitOptions.None)[0].Replace("\"","");
        return item;
    }
    private static string GetValue(string text)
    {
        string item = text.Split(new string[] { "\":\"" }, StringSplitOptions.None)[1].Replace("\"", "");
        return item;
    }

}

public class DataFrame
{
    public List<CsvDataLine> lines = new List<CsvDataLine>();

    //要素番号はインデックス内か
    private bool IsRange(int x, int y) {
        return ((x >= 0) && (y >= 0) && (x < ColumnSize()) && (y < LineSize()));
    }

    public string Name(int x) {
        return lines[x].name;
    }

    //要素をセットする
    public void Set(int x, int y, string item)
    {
        if (IsRange(x,y))
        {
            lines[x].items[y] = item;
        }
        else
        {
            Debug.Log("範囲外を指定しています");
        }
    }

    public void Set(string name, int y, string item)
    {
        bool isSet = false;
        foreach (var line in lines)
        {
            if (name == line.name)
            {
                line.items[y] = item;
                isSet = true;
            }
        }

        if (isSet == false) {
            Debug.Log("カラム「"+name+"」が見つかりませんでした");
        }
    }

    //要素を取得する
    public string Get(int x, int y)
    {
        if (IsRange(x, y))
        {
            return lines[x].items[y];
        }
        Debug.Log("範囲外を指定しています");
        return "";
    }

    //要素を取得する
    public string Get(string name, int y)
    {
        foreach (var line in lines)
        {
            if (name == line.name)
            {
                return line.items[y];
            }
        }
        Debug.Log("要素が見つかりませんでした");
        return "";
    }

    public DataFrame GetRow(int y) {
        DataFrame copy = new DataFrame();
        //カラムを作成
        for (int x = 0; x < ColumnSize(); x++)
        {
            CsvDataLine line = new CsvDataLine();
            line.name = this.Name(x);
            copy.lines.Add(line);
        }
        //データを代入
        for (int x = 0; x < ColumnSize(); x++)
        {
            copy.lines[x].items.Add(this.Get(x, y));
        }
        return copy;
    }

    //削除
    private void Delete(int x, int y) {
        if (IsRange(x, y))
        {
            lines[x].items.RemoveAt(y);
        }
        else {
            Debug.Log("範囲外を指定しています");
        }
    }

    //一列を削除
    public void DeleteLine(int y)
    {
        for (int x = 0; x < ColumnSize(); x++)
        {
            Delete(x, y);
        }
    }

    public int ColumnSize()
    {
        return lines.Count;
    }

    public int LineSize()
    {
        return lines[0].items.Count;
    }

    public DataFrame Copy() {
        DataFrame copy = new DataFrame();
        //カラムを作成
        for (int x = 0; x < ColumnSize(); x++)
        {
            CsvDataLine line = new CsvDataLine();
            line.name = this.Name(x);
            copy.lines.Add(line);
        }
        //データを代入
        for (int y = 0; y < LineSize(); y++)
        {
            for (int x = 0; x < ColumnSize(); x++)
            { 
                copy.lines[x].items.Add(Get(x, y));
            }
        }
        return copy;
    }
    public DataFrame Where(string name, int key)
    {
        return Where(name, key.ToString());
    }

    public DataFrame Where(string name, string key) {
        DataFrame copy = new DataFrame();
        //カラムを作成
        for (int x = 0; x < ColumnSize(); x++)
        {
            CsvDataLine line = new CsvDataLine();
            line.name = this.Name(x);
            copy.lines.Add(line);
        }

        //一致する場合は追加する
        for (int y = 0; y < LineSize(); y++) {
            if (Get(name, y) == key) {
                for (int x = 0; x < ColumnSize(); x++)
                {
                    copy.lines[x].items.Add(Get(x, y)); 
                }
            }
        }

        return copy;
    }

    public string ToJson() {
        string output = "";
        output += "[";

        for (int y = 0; y < LineSize(); y++) 
        {
            if (y > 0)
            {
                output += ",";
            }
            output += "{";
            for (int x = 0; x < ColumnSize(); x++)
            {
                if (x > 0) {
                    output += ",";
                }
                string key = Name(x);
                string value = Get(x, y);

                string word = "\"" + key + "\":\"" + value + "\"";

                output += word;
            }
            output += "}";
        }

        output += "]";
        return output;
    }

    public void SaveJsonWithPath(string path) {
        var jsonstr = ToJson();
        var writer = new StreamWriter(path, false);
        writer.Write(jsonstr);
        writer.Flush();
        writer.Close();
    }

    public void SaveJson(string name) {
        string path = UnityEngine.Application.persistentDataPath + "/" + name + ".json";
        //string path = Application.dataPath + "/Resources/" + name + ".json";
        SaveJsonWithPath(path);
        
    }

}

public class CsvDataLine
{
    public string name;
    public List<string> items = new List<string>();
}
