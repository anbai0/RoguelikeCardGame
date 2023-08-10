using UnityEngine;
using UnityEngine.UI;

// Fade�Ƃ������O��Prefab�ɃA�^�b�`���Ă�������
public class FadeEffectController : MonoBehaviour
{
    [Header("�t�F�[�h�Ɋ|���鎞��")]
    public float fadeSpeed = 0.7f;
    [Header("fadeSpeed�̉��{���Ó]���������邩")]
    public float fadeDurationMultiplier = 1.2f;

    public static bool isFadeInstance = false;      // �V���O���g���Ő������ꂽ��
    private bool isFadeIn = false;                  // �t�F�[�h�C������t���O
    private bool isFadeOut = false;                 // �t�F�[�h�A�E�g����t���O

    private float alpha = 0.0f;     // ���ߗ�
    private Image fadeImage;

    void Start()
    {
        // �������ɃV���O���g�����g���ĕ�������Ȃ��悤�ɂ���
        if (!isFadeInstance)
        {
            DontDestroyOnLoad(this);
            isFadeInstance = true;
        }
        else
        {
            Destroy(this);
        }

        fadeImage = GetComponentInChildren<Image>();
    }

    void Update()
    {
        if (isFadeIn)
        {
            alpha -= Time.deltaTime / fadeSpeed;
            if (alpha <= 0.0f)      // �����ɂȂ�����A�t�F�[�h�C�����I��
            {
                isFadeIn = false;
                alpha = 0.0f;
                gameObject.SetActive(false);    //�p�l�����ז��ɂȂ�̂ňꎞ�I�ɏ����Ă��܂�
            }
            fadeImage.color = new Color(0.0f, 0.0f, 0.0f, alpha);
        }
        else if (isFadeOut)
        {
            alpha += Time.deltaTime / fadeSpeed;
            if (alpha >= 1.0f)      // �^�����ɂȂ�����A�t�F�[�h�A�E�g���I��
            {
                isFadeOut = false;
                alpha = 1.0f;
            }
            fadeImage.color = new Color(0.0f, 0.0f, 0.0f, alpha);
        }
    }

    public void fadeIn()
    {
        isFadeIn = true;
        isFadeOut = false;
    }

    public void fadeOut()
    {
        isFadeOut = true;
        isFadeIn = false;
    }
}