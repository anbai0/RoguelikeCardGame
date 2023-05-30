//CharacterSelection
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    private Image image;
    private Color32 originalColor;
    private Color32 targetColor;

    Vector3 Scale = new Vector3(0.01f, 0.01f, 0.01f);

    bool isHighlight = false;

    private float duration = 0.25f;      //�F���ς��܂ł̕b��
    private float elapsedTime = 0f;
    private float late;

    private void Start()
    {
        // �摜��Image�R���|�[�l���g���擾
        image = this.gameObject.GetComponent<Image>();

        // �����̐F��ۑ�
        originalColor = image.color;

        // �n�C���C�g���̐F
        targetColor = new Color32(255, 255, 255, 255);
    }

    private void Update()
    {
        if (isHighlight)    //�L�����N�^�[�����n�C���C�g����
        {
            elapsedTime += Time.deltaTime;
            late = Mathf.Clamp01(elapsedTime / duration);
            image.color = Color32.Lerp(originalColor, targetColor, late);
        }
        else    //�L�����N�^�[�����[���C�g����i�Â�����j
        {
            elapsedTime = 0;
            late = 0;
            image.color = originalColor;    //�F��߂�
        }
    }

    private void OnMouseEnter()
    {
        //�L�����N�^�[�����n�C���C�g����
        isHighlight = true;
        //�摜��傫������
        this.transform.localScale += Scale;
    }

    private void OnMouseExit()
    {
        //�L�����N�^�[�����[���C�g����i�Â�����j
        isHighlight = false;
        //�摜�̑傫����߂�
        this.transform.localScale -= Scale;
    }


}