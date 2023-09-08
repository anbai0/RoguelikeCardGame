using System.Collections;
using UnityEngine;

/// <summary>
/// �퓬��V��ʂ̊g��\���E�k���\�����s���X�N���v�g
/// </summary>
public class DisplayAnimation : MonoBehaviour
{
    [SerializeField, Header("�\���p�I�u�W�F�N�g")] GameObject displayObject;
    [SerializeField, Header("�\���̔{��")] float enlargeScale = 1.5f;
    [SerializeField, Header("�؂�ւ��܂ł̎���")] float animationDuration = 1.0f;

    private Vector3 originalScale; //�����X�P�[��
    private float elapsedTime = 0f; // �o�ߎ���

    private void Start()
    {
        // UI�e�L�X�g�̏����X�P�[����ۑ�
        originalScale = displayObject.transform.localScale;
        // UI�e�L�X�g���\���ɂ���
        displayObject.SetActive(false);
    }

    /// <summary>
    /// �\������I�u�W�F�N�g���g��\�����鏈��
    /// </summary>
    public void StartPopUPAnimation()
    {
        // UI�e�L�X�g���A�N�e�B�u�ɂ���
        displayObject.SetActive(true);
        // �A�j���[�V�������J�n
        elapsedTime = 0f;
        StartCoroutine(ScaleUp());
    }

    /// <summary>
    /// �\������I�u�W�F�N�g���k���\�����鏈��
    /// </summary>
    public void StartDisappearAnimation()
    {
        // ��\���ɂ���
        displayObject.SetActive(false);
    }

    // �g��A�j���[�V����
    private IEnumerator ScaleUp()
    {
        while (elapsedTime < animationDuration)
        {
            float scale = Mathf.Lerp(originalScale.x, enlargeScale, elapsedTime / animationDuration);
            displayObject.transform.localScale = new Vector3(scale, scale, 1f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // �ڕW�X�P�[���ɒB������A�j���[�V�����I��
        displayObject.transform.localScale = new Vector3(enlargeScale, enlargeScale, 1f);
    }
}
