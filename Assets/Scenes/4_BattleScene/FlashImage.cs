using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Image���I���W�i���̃V�F�[�_�[�}�e���A���Ńt���b�V��������X�N���v�g
/// </summary>
public class FlashImage : MonoBehaviour
{
    public Image flashImage; //�t���b�V��������I�u�W�F�N�g
    float flashDuration = 0.1f; // �t���b�V���̎���
    private Color32 originColor = new Color(0.7f, 0.7f, 0.7f); //�t���b�V���I�����̐F
    bool isFlashing = false;

    private void Update()
    {
        if (isFlashing)
        {
            // �t���b�V���I��
            Invoke("EndFlash", flashDuration);
        }
    }

    /// <summary>
    /// �C���[�W�̃t���b�V�����J�n
    /// </summary>
    /// <param name="startColor">�t���b�V�����̐F</param>
    /// <param name="duration">�t���b�V���̌p������</param>
    public void StartFlash(Color startColor, float duration)
    {
        flashDuration = duration;
        if (!isFlashing)
        {
            // �t���b�V���J�n
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
