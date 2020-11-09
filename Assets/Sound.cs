using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sound : MonoBehaviour
{
    [Header("音声出力")]
    [SerializeField]
    AudioSource BGM;
    [SerializeField]
    AudioSource SE;

    [Header("音声素材")]
    [SerializeField]
    AudioClip click;
    [SerializeField]
    AudioClip back;
    [SerializeField]
    AudioClip bgmA;

    //外部UI関連
    [Header("ユーザー外部入力")]
    [SerializeField]
    Slider bgmSlider;
    [SerializeField]
    Slider seSlider;

    // Start is called before the first frame update
    void Start()
    {
        PlayBGM();

        //BGM設定
        BGM.volume = 1.0f / 5.0f;
        //SE設定
        SE.volume = 1.0f / 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //ボタンクリックのSE
    public void ClickSound()
    {
        SE.PlayOneShot(click);
    }

    //戻る時のSE
    public void BackSound()
    {
        SE.PlayOneShot(back);
    }

    //曲を開始
    public void PlayBGM() {
        BGM.clip = bgmA;
        BGM.Play();
    }

    //曲を停止
    public void StopBGM() {
        BGM.Stop();
    }

    public void SetBGM() {
        BGM.volume = bgmSlider.value / 5;
    }

    public void SetSE()
    {
        SE.volume = seSlider.value / 2;
    }
}
