using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniBlock;
using UnityEngine.UI;

namespace ManagerSet
{
    public class ScriptableUiManager : MonoBehaviour
    {
        public UiManager data;

        //状態遷移
        private UiComponent current;
        private UiComponent next;

        //描画先のキャンバス
        private Canvas canvas;

        //エフェクト用
        private Image fade;

        private void Awake()
        {
            //キャンバスを作成
            var obj = new GameObject();
            obj.name = "Canvas";
            obj.layer = 5;//UIレイヤー
            obj.AddComponent<RectTransform>();
            obj.AddComponent<Canvas>();
            obj.AddComponent<CanvasScaler>();
            obj.AddComponent<GraphicRaycaster>();
            //レンダーモードを変更
            canvas = obj.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 10;//一番前に来るようにする
            //ヒエラルキーに追加
            obj.transform.parent = gameObject.transform;

        }

        //指定の要素に遷移する
        public void Transition() {
            //次を設定する
            //next = component;

            //フェードを準備する
            if (fade == null) {
                fade = CreateFade(canvas).GetComponent<Image>();
            }

            //フローチャート作成
            ProcessBlock block = new Sequence(
                new Function(() => {
                    fade.raycastTarget = true;
                }),
                new LinerVector4(() => fade.color, (x) => fade.color = x, new Vector4(0, 0, 0, 1), 0.2f),//0.2秒かけて黒にする
                //new Function(ChangePanel),    //パネルを変更する
                new Wait(0.2f),                 //0.2秒待機する
                new LinerVector4(() => fade.color, (x) => fade.color = x, new Vector4(0, 0, 0, 0), 0.2f),//0.2秒かけて透明にする
                new Function(() => {
                    fade.raycastTarget = false;
                })
            );

            //フローチャート実行
            block.Activate();
        }

        public float testFloat = 100f;
        public void Test() {
            //フローチャート作成
            ProcessBlock block = new Sequence(
                //10秒かけてtestFloatを200fに変更する
                new Liner(() => testFloat, (x) => testFloat = x, 200f, 10f)
                );

            //フローチャート実行
            block.Activate();
        }

        private void ChangePanel()
        {
            if (current != null) {
                Destroy(current.gameObject);
            }
            current = next;
            Instantiate(next);
        }

        public GameObject CreateFade(Canvas canvas) {
            var parent = canvas.gameObject.GetComponent<RectTransform>();
            //UIを作成
            var obj = new GameObject();
            obj.name = "Fade";
            obj.layer = 5;//UIレイヤー
            //フェード用のコンポーネント追加
            obj.AddComponent<RectTransform>();
            obj.AddComponent<Canvas>();
            obj.AddComponent<CanvasRenderer>();
            obj.AddComponent<Image>();
            //Imageの初期化
            var image = obj.GetComponent<Image>();
            image.color = new Color(0, 0, 0, 0);
            image.raycastTarget = false;
            //RectTransform
            obj.transform.parent = parent.transform;
            var rectTransform = obj.GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(0, 0, 0);
            rectTransform.sizeDelta = parent.rect.size;

            return obj;
        }
    }

}