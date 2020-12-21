using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SocialConnector;  
using UnityEngine.UI;  

public class ShareTweet : MonoBehaviour
{
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

        // 投稿
        string tweetText = "sample tweet";
        string tweetURL = "";
        SocialConnector.SocialConnector.Share(tweetText, tweetURL, imgPath);
    }
}
