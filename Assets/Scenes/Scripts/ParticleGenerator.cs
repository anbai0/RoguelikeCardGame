using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleGenerator : MonoBehaviour
{
    [SerializeField] GameObject tapEffect;                  // タップエフェクト

    private void Update()
    {
        Tap();
    }

    public void Tap()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // クリックしたスクリーン座標
            var screenPoint = Input.mousePosition;
            Debug.Log(screenPoint);
            
            // クリック位置に対応するRectTransformのlocalPositionを計算する
            var rect = GetComponent<RectTransform>();
            var localPoint = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screenPoint, null, out localPoint);

            //ローカル座標にパーティクルを生成
            var effect = Instantiate(tapEffect, transform);
            effect.GetComponent<RectTransform>().localPosition = localPoint;
            effect.SetActive(true);
        }
    }
}
