using System;
using UnityEngine;
using UnityEngine.UI;

namespace UniBlock
{
    //専用のシングルトンクラスを作成する
    //要素は外部にオープンにする
    public class Block : MonoBehaviour
    {
        //共通要素
        //UI関連
        public PanelManager chatPanel;
        public Text chatText;
        public Text nameText;

        //UIエフェクト関連
        //public Fade fade;

        //サウンド関連
        public AudioSource shotLine;
        public AudioSource bgmLine;
        public AudioClip chatSound;

        //staticなので、共有である
        private static Block instance;
        public static Block Instance
        {
            get
            {
                //初回だけ呼ばれる
                if (instance == null)
                {
                    Type t = typeof(Block);

                    instance = (Block)FindObjectOfType(t);
                    if (instance == null)
                    {
                        Debug.LogError(t + " をアタッチしているGameObjectはありません");
                    }
                }

                return instance;
            }
        }


        //入力関連
        /*
        InputBlockSet input;
        public static InputBlockSet Input
        {
            get
            {
                return Instance.input;
            }
        }
        */

        //UI
        UiBlockSet ui;
        public static UiBlockSet UI
        {
            get
            {
                return Instance.ui;
            }
        }

        //サウンド
        SoundBlockSet sound;
        public static SoundBlockSet Audio
        {
            get
            {
                return Instance.sound;
            }
        }

        //初期化
        private void Awake()
        {
            ui = new UiBlockSet();
            sound = new SoundBlockSet();
        }

        //基本機能の関数
        public static InstantBlock Function(Action action) {
            return new Function(action);
        }

        //
        public static ProcessBlock ParallelOne(params ProcessBlock[] blocks)
        {
            return new ParallelOne(blocks);
        }

        public static ProcessBlock ParallelAll(params ProcessBlock[] blocks)
        {
            return new ParallelAll(blocks);
        }

        public static ProcessBlock Sequence(params ProcessBlock[] blocks)
        {
            return new Sequence(blocks);
        }

        public static FiniteBlock Wait(float second)
        {
            return new Wait(second);
        }
    }

    //UI
    public class UiBlockSet
    {
        /*
        public TimerBlock FadeToBlack(float interval)
        {
            return new FadeOn(Block.Instance.fade, interval);
        }

        public TimerBlock FadeToClear(float interval)
        {
            return new FadeOff(Block.Instance.fade, interval);
        }
        */

        public ProcessBlock SerifDetail(string str, float second, AudioClip sound, SerifType type) {
            return new SerifBlock(Block.Instance.chatText, str, second, Block.Instance.shotLine, sound, type);
        }

        
        public ProcessBlock Serif(string name, string str) {
            return Block.ParallelAll(
                Block.UI.SerifDetail(str, 0.05f, Block.Instance.chatSound, SerifType.Wait),
                new SetTextBlock(Block.Instance.nameText, name)
                );
        }

        /*
        public ProcessBlock ChatOpen()
        {
            return Block.Sequence(
                Block.Function(Config.OnChat),
                new SetTextBlock(Block.Instance.chatText, ""),
                new SetTextBlock(Block.Instance.nameText, ""),
                Block.Function(Block.Instance.chatPanel.Open)
            );
        }

        public ProcessBlock ChatClose()
        {
            return Block.Sequence(
                Block.Function(Block.Instance.chatPanel.Close),
                Block.Function(Config.OffChat)
            );
        }
        */
    }

    //Sound
    public class SoundBlockSet
    {
        AudioSource shotLine;
        AudioSource bgmLine;

        public SoundBlockSet()
        {
            this.shotLine = Block.Instance.shotLine;
            this.bgmLine = Block.Instance.bgmLine;
        }

        public FiniteBlock PlayBGM(AudioClip sound)
        {
            return new PlayBGM(bgmLine, sound);
        }

        public FiniteBlock StopBGM()
        {
            return new StopBGM(bgmLine);
        }

        public FiniteBlock PlayOneShot(AudioClip sound)
        {
            return new PlayOneShot(shotLine, sound);
        }
    }

    //Input
    /*
    public class InputBlockSet {
        public ProcessBlock Click(MyKey key, float time) {
            return new SwitchBlock(
                new KeyDownBlock(key),
                new KeyUpBlock(key),
                new Wait(time)
                );
        }

        public ProcessBlock ClickAction(MyKey key, float time, Action action)
        {
            return Block.Sequence(
                Block.Input.Click(key, time),
                Block.Function(action)
                );
        }

        public ProcessBlock Hold(MyKey key, float time)
        {
            return new SwitchBlock(
                new KeyDownBlock(key),
                new Wait(time),
                new KeyUpBlock(key)
                );
        }

        public ProcessBlock HoldAction(MyKey key, float time, Action action)
        {
            return Block.Sequence(
                Block.Input.Hold(key, time),
                Block.Function(action)
                );
        }
    }
    */

    public enum SerifType
    {
        Wait,
        Nonstop
    }
}