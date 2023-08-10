using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconAnimation : MonoBehaviour
{
    Image iconImage;
    public float enlargeScale = 1.5f;
    public float animationDuration = 1.0f;

    private Vector3 originalScale;
    private bool isAnimating = false;
    private float animationStartTime;

    private void Start()
    {
        iconImage = GetComponent<Image>();
        originalScale = iconImage.transform.localScale;
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
                iconImage.transform.localScale = originalScale;
                isAnimating = false;
            }
            else
            {
                // �g��\�����猳�̑傫���֏��X�ɕω�������
                float currentScale = Mathf.Lerp(enlargeScale, 1.0f, t);
                iconImage.transform.localScale = originalScale * currentScale;
            }
        }
    }

    public void StartAnimation()
    {
        if (!isAnimating)
        {
            animationStartTime = Time.time;
            isAnimating = true;
        }
    }
}
