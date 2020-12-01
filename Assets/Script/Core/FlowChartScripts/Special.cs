using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UniBlock
{
    //ブロックを拡張させる特殊なものはこちらへ

    //直列に実行する
    [System.Serializable]
    public sealed class Sequence : ProcessBlock
    {
        ProcessBlock[] blocks;

        int index = 0;


        public Sequence(params ProcessBlock[] blocks)
        {
            this.blocks = blocks;
        }

        public sealed override void Start()
        {
            Reset();
            if (blocks != null)
            {
                blocks[index].Start();
            }
            else
            {
                SetEnd();
            }
        }

        public sealed override void Update()
        {
            //例外処理
            if (blocks == null) return;

            //終了条件判定
            if (blocks[index].IsEnd())
            {
                index += 1;
                if (index >= blocks.Length)
                {
                    SetEnd();
                }
                else
                {
                    blocks[index].Start();
                    //もう一度検証する(1フレームロスを防ぐ)
                    this.Update();
                }
            }
            else
            {
                //Update
                blocks[index].Update();
            }

        }


    }

    //並行に実行し、どれか一つでも終了すると次へ進む
    [System.Serializable]
    public sealed class ParallelOne : ProcessBlock
    {
        ProcessBlock[] blocks;
        public ParallelOne(params ProcessBlock[] blocks)
        {
            this.blocks = blocks;
        }

        public sealed override void Start()
        {
            Reset();
            if (blocks != null)
            {
                foreach (var block in blocks)
                {
                    block.Start();
                }
            }
            else
            {
                SetEnd();
            }
        }

        public sealed override void Update()
        {
            //例外処理
            if (blocks == null) return;

            //終了条件判定
            foreach (var block in blocks)
            {
                if (block.IsEnd())
                {
                    SetEnd();
                    return;
                }
            }

            //Update
            foreach (var block in blocks)
            {
                block.Update();
            }
        }
    }

    //並行に実行し、全て終了すると次へ進む
    [System.Serializable]
    public sealed class ParallelAll : ProcessBlock
    {
        ProcessBlock[] blocks;
        public ParallelAll(params ProcessBlock[] blocks)
        {
            this.blocks = blocks;
        }

        public sealed override void Start()
        {
            Reset();
            if (blocks != null)
            {
                foreach (var block in blocks)
                {
                    block.Start();
                }
            }
            else
            {
                SetEnd();
            }
        }

        public sealed override void Update()
        {
            //例外処理
            if (blocks == null) return;

            //終了条件判定
            bool isContinue = false;
            foreach (var block in blocks)
            {
                //一つでも終了していないものがあれば、継続する
                if (!block.IsEnd())
                {
                    isContinue = true;
                }
            }

            if (isContinue)
            {
                //Update
                foreach (var block in blocks)
                {
                    block.Update();
                }
            }
            else
            {
                SetEnd();
            }
        }
    }

    //スイッチさせる。特殊用途
    public sealed class SwitchBlock : ProcessBlock
    {
        ProcessBlock init, ok, not;
        int index;

        public SwitchBlock(ProcessBlock init, ProcessBlock ok, ProcessBlock not)
        {
            this.init = init;
            this.ok = ok;
            this.not = not;
        }

        public sealed override void Start()
        {
            Reset();//全てのスタートにResetを入れること
            index = 0;
            init.Start();
        }

        public sealed override void Update()
        {
            if (index == 0)
            {
                if (init.IsEnd())
                {
                    ok.Start();
                    not.Start();
                    index += 1;
                }
                else
                {
                    init.Update();
                }
            }
            if (index == 1) {
                if (ok.IsEnd())
                {
                    //終了させる
                    this.SetEnd();
                }
                else if (not.IsEnd())
                {
                    //再起動させる
                    this.Start();
                }
                else {
                    ok.Update();
                    not.Update();
                }
            }

        }
    }

    //セリフ用の特殊なもの。一応置いておく
    public sealed class SerifBlock : ProcessBlock
    {
        Text panel;
        string str;
        float interval;
        SerifType type;
        //サウンド関連
        AudioClip sound;
        AudioSource audioSource;

        //動的内部変数
        int index;
        string dispStr;
        System.Diagnostics.Stopwatch timer;

        public SerifBlock(Text text, string str, float second, AudioSource audioSource, AudioClip sound, SerifType type)
        {
            this.panel = text;
            this.str = str;
            this.interval = second * 1000;
            timer = new System.Diagnostics.Stopwatch();
            this.type = type;

            //サウンド関連
            this.sound = sound;
            this.audioSource = audioSource;
        }

        public sealed override void Start()
        {
            //動的内部変数の初期化
            dispStr = panel.text + str;
            index = panel.text.Length;

            timer.Restart();
            Reset();
            //セリフ更新
            SerifTyping();
        }

        public sealed override void Update()
        {
            //タイマー判定
            if (timer.ElapsedMilliseconds > interval)
            {
                //テキストを一文字ふやす
                index += 1;
                if (index <= dispStr.Length)
                {
                    SerifTyping();
                    timer.Restart();
                }
                else {
                    timer.Reset();
                    timer.Stop();

                    if (type == SerifType.Nonstop) {
                        SetEnd();
                    }
                }
            }

            //入力判定
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (index < dispStr.Length)
                {
                    //テキストを全表示する
                    index = dispStr.Length;
                    SerifTyping();
                }
                else
                {
                    if (type == SerifType.Wait) {
                        panel.text = "";
                    }
                    //終了する
                    SetEnd();
                }
            }

        }

        void SerifTyping()
        {
            //テキストを更新
            panel.text = dispStr.Substring(0, index);
            //音をならす
            audioSource.PlayOneShot(sound);
        }
    }

}