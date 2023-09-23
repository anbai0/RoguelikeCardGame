using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIArrowAnimation : MonoBehaviour
{
    public float fadeSpeed = 0.01f; //�t�F�[�h�̑���
    float fadeSpeed2;
    Color tmpColor; //���̐F
    float a; //�F�̃A���t�@�l
    float init_a; //���̃A���t�@�l
    WaitForSeconds timeout = new WaitForSeconds(2.0f);

    bool isContinue = false;
    // Start is called before the first frame update
    void Start()
    {
        isContinue = false;
        gameObject.SetActive(false);
        tmpColor = GetComponent<Image>().color;
        init_a = tmpColor.a;
        a = tmpColor.a;
        fadeSpeed2 = a;
    }

    public void StartArrowMovement()
    {
        gameObject.SetActive(true);
        isContinue = true;
        StartCoroutine(FadeInArrow());
    }

    public void StopArrowMovement()
    {
        isContinue = false;
        gameObject.SetActive(false);
    }

    IEnumerator FadeInArrow()
    {
        while (true)
        {
            if (!isContinue)
            {
                yield break;
            }

            while (a > 0)
            {
                fadeSpeed2 -= fadeSpeed;
                tmpColor.a = fadeSpeed2;
                GetComponent<Image>().color = tmpColor;
                a -= fadeSpeed;
                yield return null;
            }

            while (a < init_a)
            {
                fadeSpeed2 += fadeSpeed;
                tmpColor.a = fadeSpeed2;

                GetComponent<Image>().color = tmpColor;
                a += fadeSpeed;
                yield return null;
            }

            yield return timeout;
        }
    }
}
