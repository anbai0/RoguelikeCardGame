using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultAnimation : MonoBehaviour
{
    [Header("���s���o�p�I�u�W�F�N�g")]
    [SerializeField]
    GameObject resultPrefab;
    [Header("���s���o�̕\���ꏊ")]
    [SerializeField]
    Transform resultPlace;

    GameObject resultObj = null;
    TextMeshProUGUI resultText; //���s�̕\�����s���e�L�X�g
    public float enlargeScale = 1.5f;
    public float animationDuration = 1.0f;

    private Vector3 originalScale;
    private bool isAnimating = false;
    private float animationStartTime;

    private void Start()
    {
        originalScale = resultPrefab.transform.localScale;
    }

    private void Update()
    {
        if (isAnimating)
        {
            float timeSinceStart = Time.time - animationStartTime;
            float t = Mathf.Clamp01(timeSinceStart / animationDuration);

            // �A�j���[�V�������I��
            if (t >= 1.0f)
            {
                isAnimating = false;
            }
            else
            {
                // �g��\�����猳�̑傫���֏��X�ɕω�������
                float currentScale = Mathf.Lerp(1.0f, enlargeScale, t);
                resultObj.transform.localScale = originalScale * currentScale;
            }
        }
    }

    /// <summary>
    /// ���s�e�L�X�g�̕\��
    /// </summary>
    /// <param name="vicdef">�����Ȃ�Victory,�����Ȃ�Defeated�̕�����������ɑ��</param>
    public void StartAnimation(string vicdef)
    {
        if (!isAnimating)
        {
            resultObj = Instantiate(resultPrefab, resultPlace);
            resultObj.transform.SetParent(resultPlace);
            resultText = resultObj.GetComponent<TextMeshProUGUI>();
            resultText.text = vicdef;
            animationStartTime = Time.time;
            isAnimating = true;
        }
    }

    /// <summary>
    /// ���s�e�L�X�g����������
    /// </summary>
    public void DisappearResult()
    {
        Destroy(resultObj);
    }
}
