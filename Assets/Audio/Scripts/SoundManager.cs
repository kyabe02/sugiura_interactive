using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace ManagerSet
{
	[CreateAssetMenu(menuName = "CreateManager/Sound")]
	public class SoundManager : ScriptableObject
	{
		private ScriptableSoundManager manager;

		[Header("ミキサー")]
		public AudioMixerGroup mixerBGM;
		public AudioMixerGroup mixerSE;
		public AudioMixerGroup mixerVoice;

		/*
		[Header("SE")]
		public AudioClip click;
		public AudioClip cancel;

		[Header("Music")]
		public AudioClip bgm1;
		*/

		public void SE(AudioClip clip)
		{
			Activate();
			manager.SoundSE(clip);
		}

		public void PlayMusic(AudioClip clip)
		{
			Activate();
			manager.PlayMusic(clip);
		}

		public void StopMusic()
		{
			Activate();
			manager.StopMusic();
		}

		//ヒエラルキー外部からゲームオブジェクトを作成
		private void Activate()
		{
			if (manager == null)
			{
				//new GameObjectで生成。継承先では生成不可能
				var obj = new GameObject();//生成時点で存在する。目に見えないだけ。
				obj.AddComponent<ScriptableSoundManager>();
				obj.name = "SoundManager";
				manager = obj.GetComponent<ScriptableSoundManager>();
				manager.name = "SoundManager";
				manager.data = this;
				DontDestroyOnLoad(manager);
			}
		}

	}
}