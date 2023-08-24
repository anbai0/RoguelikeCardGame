using UnityEngine;
using TMPro;

/// <summary>
/// ダメージと回復の表示をする際の演出を行うスクリプト
/// </summary>
public class DamageOrHealingText : MonoBehaviour
{
    [SerializeField] float DeleteTime = 1.5f;
    [SerializeField] float MoveRange = 50.0f;
    [SerializeField] float EndAlpha = 0.2f;

    private float TimeCnt;
    private TextMeshProUGUI NowText;

    void Start()
    {
        TimeCnt = 0.0f;
        Destroy(this.gameObject, DeleteTime);
        NowText = this.gameObject.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        TimeCnt += Time.deltaTime;
        this.gameObject.transform.localPosition += new Vector3(0, MoveRange / DeleteTime * Time.deltaTime, 0);
        float _alpha = 1.0f - (1.0f - EndAlpha) * (TimeCnt / DeleteTime);
        if (_alpha <= 0.0f) _alpha = 0.0f;
        NowText.color = new Color(NowText.color.r, NowText.color.g, NowText.color.b, _alpha);
    }
}
