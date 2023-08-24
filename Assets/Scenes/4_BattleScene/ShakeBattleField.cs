using System.Collections;
using UnityEngine;

/// <summary>
/// �_���[�W���󂯂��ۂɉ�ʂ�h�炷�X�N���v�g
/// </summary>
public class ShakeBattleField : MonoBehaviour
{
    /// <summary>
    /// �퓬��ʂ��c�ɗh�炷����
    /// </summary>
    /// <param name="duration">�p������</param>
    /// <param name="magnitude">�h��̑傫��</param>
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
