using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

// Fade�Ƃ������O��Prefab�ɃA�^�b�`���Ă�������
public class FadeController : MonoBehaviour
{
    [Header("�t�F�[�h�Ɋ|���鎞��")]
    public float fadeSpeed = 0.7f;
    [Header("fadeSpeed�̉��{���Ó]���������邩")]
    public float fadeDurationMultiplier = 1.2f;

    public static bool isFadeInstance = false;      // �V���O���g���Ő������ꂽ��
    public static bool isFadeIn = false;                  // �t�F�[�h�C������t���O
    public static bool isFadeOut = false;                 // �t�F�[�h�A�E�g����t���O

    private float alpha = 0.0f;     // ���ߗ�
    private Image fadeImage;

    private bool fadeInDone = false;
    private bool fadeOutDone = false;

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
                fadeInDone = true;
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
                fadeOutDone = true;
            }
            fadeImage.color = new Color(0.0f, 0.0f, 0.0f, alpha);
        }
    }

    public async Task fadeIn()
    {
        isFadeIn = true;
        isFadeOut = false;
        while (!fadeInDone) await Task.Yield(); // �������\�b�h�̎��s���ꎞ�I�ɒ��f
        fadeInDone = false;
    }

    public async Task fadeOut()
    {
        isFadeOut = true;
        isFadeIn = false;
        while (!fadeOutDone) await Task.Yield();
        fadeOutDone = false;
    }
}