using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayAnimation : MonoBehaviour
{
    [Header("�\���p�I�u�W�F�N�g")]
    [SerializeField]
    GameObject displayObject;
    [Header("�\���̔{��")]
    [SerializeField]
    float enlargeScale = 1.5f;
    [Header("�؂�ւ��܂ł̎���")]
    [SerializeField]
    float animationDuration = 1.0f;

    private bool isPopUP = true; //true:�g��\���@false:�k���\��
    private Vector3 originalScale;
    private Vector3 zoomScale;
    private bool isAnimating = false;
    private float animationStartTime;

    private void Start()
    {
        originalScale = displayObject.transform.localScale;
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
                if (isPopUP)
                {
                    isAnimating = false;
                }
                else
                {
                    isAnimating = false;
                    displayObject.SetActive(false);
                }

            }
            else
            {
                if (isPopUP)
                {
                    // ���X�Ɋg�傳����
                    float currentScale = Mathf.Lerp(1.0f, enlargeScale, t);
                    displayObject.transform.localScale = originalScale * currentScale;
                }
                else
                {
                    //���X�ɏk��������
                    float currentScale = Mathf.Lerp(1.0f, 0.1f, t);
                    displayObject.transform.localScale = zoomScale * currentScale;
                }

            }
        }
    }

    /// <summary>
    /// �\������I�u�W�F�N�g���g��\�����鏈��
    /// </summary>
    public void StartPopUPAnimation()
    {
        if (!isAnimating)
        {
            displayObject.SetActive(true);
            displayObject.transform.localScale = originalScale;
            isPopUP = true;
            animationStartTime = Time.time;
            isAnimating = true;
        }
    }

    /// <summary>
    /// �\������I�u�W�F�N�g���k���\�����鏈��
    /// </summary>
    public void StartDisappearAnimation()
    {
        if (!isAnimating)
        {
            zoomScale = displayObject.transform.localScale;
            isPopUP = false;
            animationStartTime = Time.time;
            isAnimating = true;
        }
    }
}
