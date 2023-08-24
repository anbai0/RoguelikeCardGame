using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Imageをオリジナルのシェーダーマテリアルでフラッシュさせるスクリプト
/// </summary>
public class FlashImage : MonoBehaviour
{
    public Image flashImage; //フラッシュさせるオブジェクト
    float flashDuration = 0.1f; // フラッシュの時間
    private Color32 originColor = new Color(0.7f, 0.7f, 0.7f); //フラッシュ終了時の色
    bool isFlashing = false;

    private void Update()
    {
        if (isFlashing)
        {
            // フラッシュ終了
            Invoke("EndFlash", flashDuration);
        }
    }

    /// <summary>
    /// イメージのフラッシュを開始
    /// </summary>
    /// <param name="startColor">フラッシュ時の色</param>
    /// <param name="duration">フラッシュの継続時間</param>
    public void StartFlash(Color startColor, float duration)
    {
        flashDuration = duration;
        if (!isFlashing)
        {
            // フラッシュ開始
            flashImage.material.SetColor("_Color", startColor);
            isFlashing = true;
        }
    }
    private void EndFlash()
    {
        flashImage.material.SetColor("_Color", originColor);
        isFlashing = false;
    }
}
