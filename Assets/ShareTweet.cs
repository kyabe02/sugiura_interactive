using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SocialConnector;  
using UnityEngine.UI;  

public class ShareTweet : MonoBehaviour
{
    
    string shopName = "";//お店の名前
    int money = 0;      //引いた金額
    List<string> menusName = new List<string>(); //ドローしたメニュー
    int kcal = 0;

    //データベースからデータを読み込む
    public void dataFetch()
    {
        shopName = Database.Instance.GetShopName();
        money = Database.Instance.drawMoney;
        menusName = Database.Instance.currentMenus;
        kcal = Database.Instance.currentKcal;
    }

    public void Share()
    {
        print("ツイッター連携を呼び出し！");
        StartCoroutine(ShareScreenShot());
    }

    public IEnumerator ShareScreenShot()
    {

        string imgPath = Application.persistentDataPath + "/screenshot.png";
        File.Delete(imgPath);
        ScreenCapture.CaptureScreenshot("screenshot.png");

        // スクショ待機
        while (true)
        {
            if (File.Exists(imgPath)) break;
            yield return null;
        }

        //Twitterテキスト用のデータを読み込む
        dataFetch();


        // 投稿
        string tweetText = shopName + "で" + money.ToString() + "円ガチャ\n";
        for (int i = 0; i < menusName.Count; i++)
        {
            string text;
            if (menusName[i].Length > 15)
            {
                text = menusName[i].Substring(0, 15);
            }
            else
            {
                text = menusName[i];
            }
            tweetText += "・" + text + "\n";

            if (i >= 4)
            {
                tweetText += "など";
                break;
            }
        }

        tweetText += "の" + menusName.Count.ToString() + "品(" + kcal + "kcal)";

        string tweetURL = "https://bit.ly/3nFBi7I";
        SocialConnector.SocialConnector.Share(tweetText, tweetURL, imgPath);
    }
}

/*
松屋で980円ガチャ

牛丼
ビール
ビール
ビール

など５品(800kcal)
*/
