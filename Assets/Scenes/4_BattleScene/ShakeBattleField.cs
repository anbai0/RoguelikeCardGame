using System.Collections;
using UnityEngine;

/// <summary>
/// ダメージを受けた際に画面を揺らすスクリプト
/// </summary>
public class ShakeBattleField : MonoBehaviour
{
    /// <summary>
    /// 戦闘画面を縦に揺らす処理
    /// </summary>
    /// <param name="duration">継続時間</param>
    /// <param name="magnitude">揺れの大きさ</param>
    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(DoShake(duration, magnitude));
    }

    private IEnumerator DoShake(float duration, float magnitude)
    {
        var pos = GetComponent<RectTransform>().anchoredPosition;

        var elapsed = 0f;

        while (elapsed < duration)
        {
            var y = pos.y + Random.Range(-1f, 1f) * magnitude;

            GetComponent<RectTransform>().anchoredPosition = new Vector2(pos.x, y);

            elapsed += Time.deltaTime;

            yield return null;
        }

        GetComponent<RectTransform>().anchoredPosition = pos;
    }
}
