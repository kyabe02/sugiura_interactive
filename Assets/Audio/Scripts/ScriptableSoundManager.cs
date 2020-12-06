using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManagerSet
{
    public class ScriptableSoundManager : MonoBehaviour
    {
        public SoundManager data;
        private AudioSource BGM;
        private AudioSource SE;
        private AudioSource VOICE;

        //クリック音を鳴らす
        private void Awake()
        {
            //BGM用のゲームオブジェクトを作成
            var bgm = new GameObject();
            bgm.name = "BGM";
            bgm.AddComponent<AudioSource>();
            BGM = bgm.GetComponent<AudioSource>();
            bgm.transform.parent = gameObject.transform;
            BGM.outputAudioMixerGroup = data.mixerBGM;

            //SE用のゲームオブジェクトを作成
            var se = new GameObject();
            se.name = "SE";
            se.AddComponent<AudioSource>();
            SE = se.GetComponent<AudioSource>();
            se.transform.parent = gameObject.transform;
            SE.outputAudioMixerGroup = data.mixerSE;

            //VOICE用のゲームオブジェクトを作成
            var voice = new GameObject();
            voice.name = "SE";
            voice.AddComponent<AudioSource>();
            VOICE = voice.GetComponent<AudioSource>();
            voice.transform.parent = gameObject.transform;
            VOICE.outputAudioMixerGroup = data.mixerVoice;

        }

        public void SoundSE(AudioClip clip)
        {
            SE.PlayOneShot(clip);
        }

        public void PlayMusic(AudioClip clip)
        {
            BGM.clip = clip;
            BGM.Play();
        }

        public void StopMusic()
        {
            BGM.Stop();
        }
    }
}
