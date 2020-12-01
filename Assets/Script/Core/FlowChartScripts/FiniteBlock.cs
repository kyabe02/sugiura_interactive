using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UniBlock
{
    //終了が確約されているブロック
    [System.Serializable]
    public class ProcessBlock : BlockBase
    {
        public virtual void Start()
        {
            Reset();
        }

        public virtual void Update()
        {
            SetEnd();
        }

        //ブロックを実行する
        public void Activate()
        {
            GameObject obj = new GameObject();
            obj.AddComponent<BlockManager>();
            BlockManager manager = obj.GetComponent<BlockManager>();
            manager.block = (ProcessBlock)this.MemberwiseClone();

            //今後はNotDestroy属性つけるといいかも
        }

    }

    //ボタンをクリックするまで待機
    /*
    public sealed class KeyDownBlock : ProcessBlock
    {
        MyKey key;
        public KeyDownBlock(MyKey key)
        {
            this.key = key;
        }

        public sealed override void Update()
        {
            if (Config.KeyDown(key))
            {
                SetEnd();
            }
        }
    }

    //ボタンをクリックするまで待機
    public sealed class KeyUpBlock : ProcessBlock
    {
        MyKey key;
        public KeyUpBlock(MyKey key)
        {
            this.key = key;
        }

        public sealed override void Update()
        {
            if (Config.KeyUp(key))
            {
                SetEnd();
            }
        }
    }
    */

    //ボタンをクリックするまで待機
    /*
    public sealed class ButtonClickBlock : ProcessBlock
    {
        ButtonManager button;
        public ButtonClickBlock(ButtonManager button)
        {
            this.button = button;
        }

        public sealed override void Update()
        {
            if (button.IsClicked)
            {
                SetEnd();
            }
        }
    }
    */
}