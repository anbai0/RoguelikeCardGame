using System.Collections;
using UnityEngine;

/// <summary>
/// ƒ_ƒ[ƒW‚ğó‚¯‚½Û‚É‰æ–Ê‚ğ—h‚ç‚·ƒXƒNƒŠƒvƒg
/// </summary>
public class ShakeBattleField : MonoBehaviour
{
    /// <summary>
    /// í“¬‰æ–Ê‚ğc‚É—h‚ç‚·ˆ—
    /// </summary>
    /// <param name="duration">Œp‘±ŠÔ</param>
    /// <param name="magnitude">—h‚ê‚Ì‘å‚«‚³</param>
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
