using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ManagerSet
{
	[CreateAssetMenu(menuName = "CreateManager/UI")]
	public class UiManager : ScriptableObject
	{
		private ScriptableUiManager manager;

		[Header("UIエフェクト")]
		public Image fade;                     //フェード機能用のイメージ
		public Image hoverBlack;

		//ヒエラルキー外部からゲームオブジェクトを作成
		private void Activate()
		{
			if (manager == null)
			{
				//new GameObjectで生成。継承先では生成不可能
				var obj = new GameObject();//生成時点で存在する。目に見えないだけ。
				obj.AddComponent<ScriptableUiManager>();
				manager = obj.GetComponent<ScriptableUiManager>();
				manager.name = "UiManager";
				manager.data = this;
				DontDestroyOnLoad(manager);
			}
		}

		//指定の画面に遷移させる
		public void Transition() {
			Activate();
			manager.Transition();
		}

		public void Test() {
			Activate();
			manager.Test();
		}
	}
}