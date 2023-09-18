using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleGenerator : MonoBehaviour
{
    [SerializeField] GameObject tapEffect;                  // �^�b�v�G�t�F�N�g

    private void Update()
    {
        Tap();
    }

    public void Tap()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // �N���b�N�����X�N���[�����W
            var screenPoint = Input.mousePosition;
            Debug.Log(screenPoint);
            
            // �N���b�N�ʒu�ɑΉ�����RectTransform��localPosition���v�Z����
            var rect = GetComponent<RectTransform>();
            var localPoint = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screenPoint, null, out localPoint);

            //���[�J�����W�Ƀp�[�e�B�N���𐶐�
            var effect = Instantiate(tapEffect, transform);
            effect.GetComponent<RectTransform>().localPosition = localPoint;
            effect.SetActive(true);
        }
    }
}
