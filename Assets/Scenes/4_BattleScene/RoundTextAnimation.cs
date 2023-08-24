using UnityEngine;
using TMPro;

/// <summary>
/// ���E���h����\������ۂɉ��o���s���X�N���v�g
/// </summary>
public class RoundTextAnimation : MonoBehaviour
{
    [SerializeField, Header("���E���h���\���p�I�u�W�F�N�g")] GameObject roundPrefab;
    [SerializeField, Header("���E���h���̕\���ꏊ")] Transform roundPlace;

    GameObject roundObj = null;
    TextMeshProUGUI roundText; //���E���h�����e�L�X�g
    public float enlargeScale = 1.5f;
    public float animationDuration = 1.0f;
    private Vector3 originalScale;
    private bool isAnimating = false;
    private float animationStartTime;

    void Start()
    {
        originalScale = roundPrefab.transform.localScale;
    }

    void Update()
    {
        if (isAnimating)
        {
            float timeSinceStart = Time.time - animationStartTime;
            float t = Mathf.Clamp01(timeSinceStart / animationDuration);

            // �A�j���[�V�������I��
            if (t >= 1.0f)
            {
                isAnimating = false;
                Invoke("DisappearRoundText", 0.5f);
            }
            else
            {
                // �g��\�����猳�̑傫���֏��X�ɕω�������
                float currentScale = Mathf.Lerp(1.0f, enlargeScale, t);
                roundObj.transform.localScale = originalScale * currentScale;
            }
        }
    }

    /// <summary>
    /// ���E���h�e�L�X�g�̕\��
    /// </summary>
    /// <param name="roundCount">�\�����������E���h���̃e�L�X�g</param>
    public void StartAnimation(string roundCount)
    {
        if (!isAnimating)
        {
            roundObj = Instantiate(roundPrefab, roundPlace);
            roundObj.transform.SetParent(roundPlace);
            roundText = roundObj.GetComponent<TextMeshProUGUI>();
            roundText.text = roundCount;
            animationStartTime = Time.time;
            isAnimating = true;
        }
    }

    /// <summary>
    /// ���E���h�e�L�X�g����������
    /// </summary>
    public void DisappearRoundText()
    {
        Destroy(roundObj);
    }
}
