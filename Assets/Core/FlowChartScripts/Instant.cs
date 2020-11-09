using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UniBlock
{
    //有限時間かつ一瞬で終了するモノはこちらへ
    public class InstantBlock : FiniteBlock
    {
        public sealed override void Start()
        {
            Reset();
            Action();
            SetEnd();
        }

        public sealed override void Update() { }

        public virtual void Action() { }
    }

    public sealed class Function : InstantBlock
    {
        Action action;

        public Function(Action action)
        {
            this.action = action;
        }

        public override void Action()
        {
            action.Invoke();
        }
    }

    public sealed class SetTextBlock : InstantBlock
    {
        Text panel;
        string text;

        public SetTextBlock(Text panel, string text)
        {
            this.panel = panel;
            this.text = text;
        }

        public override void Action()
        {
            panel.text = text;
        }
    }

    public sealed class PlayOneShot : InstantBlock
    {
        AudioClip sound;
        AudioSource audioSource;

        public PlayOneShot(AudioSource audioSource, AudioClip sound)
        {
            this.sound = sound;
            this.audioSource = audioSource;
        }

        public override void Action()
        {
            audioSource.PlayOneShot(sound);
        }
    }

    public sealed class PlayBGM : InstantBlock
    {
        AudioClip sound;
        AudioSource audioSource;

        public PlayBGM(AudioSource audioSource, AudioClip sound)
        {
            this.sound = sound;
            this.audioSource = audioSource;
        }

        public override void Action()
        {
            audioSource.clip = sound;
            audioSource.Play();
        }
    }

    public sealed class StopBGM : InstantBlock
    {
        AudioSource audioSource;

        public StopBGM(AudioSource audioSource)
        {
            this.audioSource = audioSource;
        }

        public override void Action()
        {
            audioSource.Stop();
        }
    }
}