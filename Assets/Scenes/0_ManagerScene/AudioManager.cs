using DG.Tweening;
using UnityEngine;
using System.Collections;

/// <summary>
/// SE,BGM���Ǘ����܂��B
/// <para>���𗬂��Ƃ��́A</para>
/// <code>AudioManager.Instance.PlaySE("SE�̖��O");</code>
/// <code>AudioManager.Instance.PlayBGM("BGM�̖��O");</code>
/// <para>���������Ƃ��́A</para>
/// <code>AudioManager.Instance.seAudioSource.Stop();</code>
/// <code>AudioManager.Instance.bgmAudioSource.Stop();</code>
/// <para>�ꎞ��~�A��~�����́A</para>
/// <code>AudioManager.Instance.bgmAudioSource.Pause();</code>
/// <code>AudioManager.Instance.bgmAudioSource.UnPause();</code>
/// <para>�t�F�[�h����̈ꎞ��~�A��~�����́A</para>
/// <code>AudioManager.Instance.StartCoroutine(AudioManager.Instance.IEFadeInBGMVolume());</code>
/// <code>AudioManager.Instance.StartCoroutine(AudioManager.Instance.IEFadeOutBGMVolume());</code>
/// </summary>
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource seAudioSource;      // SE�p��AudioSource
    [SerializeField] public AudioSource bgmAudioSource;      // BGM�p��AudioSource
    private AudioSetting audioSetting;

    [Header("SE�֌W")]
    [SerializeField] private AudioClip[] seAudioClips;
    [SerializeField] private float[] seAudioVolumes;

    [Header("BGM�֌W")]
    [SerializeField] private AudioClip[] bgmAudioClips;
    [SerializeField] private float[] bgmAudioVolumes;

    private int currentBGMIndex;        // �ݒ�ŉ��ʂ�ς���Ƃ��Ɏg���܂�
    private float currentBGMVolume;     // �����t�F�[�h���鎞�Ɏg���܂�
    private float fadeVolumeSpeed = 1.5f;
    private Tween fadeInTween = null;
    private Tween fadeOutTween = null;

    public static AudioManager Instance;     // �V���O���g��
    private void Awake()
    {
        // �V���O���g���C���X�^���X���Z�b�g�A�b�v
        if (Instance == null)
        {
            Instance = this;
        }
        // �R���|�[�l���g�擾
        audioSetting = GetComponent<AudioSetting>();

        DOTween.SetTweensCapacity(1000, 50);    // Tween�p�̃������m��
    }


    private void Update()
    {
        //// SE�`�F�b�N
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //    PlaySE("�^�C�g�����");
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //    PlaySE("�I����");
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //    PlaySE("������");
        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //    PlaySE("�}�b�v�؂�ւ�");
        //if (Input.GetKeyDown(KeyCode.Alpha5))
        //    PlaySE("�U��1");
        //if (Input.GetKeyDown(KeyCode.Alpha6))
        //    PlaySE("��");
        //if (Input.GetKeyDown(KeyCode.Alpha7))
        //    PlaySE("�o�t");
        //if (Input.GetKeyDown(KeyCode.Alpha8))
        //    PlaySE("�f�o�t");
        //if (Input.GetKeyDown(KeyCode.Alpha9))
        //    PlaySE("");
        //if (Input.GetKeyDown(KeyCode.Alpha0))
        //    PlaySE("");

        //// BGM�`�F�b�N
        //if (Input.GetKeyDown(KeyCode.Q))
        //    PlayBGM("Field1");
        //if (Input.GetKeyDown(KeyCode.W))
        //    PlayBGM("Field2");
        //if (Input.GetKeyDown(KeyCode.E))
        //    PlayBGM("Result");
        //if (Input.GetKeyDown(KeyCode.R))
        //    PlayBGM("it's my turn");
        //if (Input.GetKeyDown(KeyCode.T))
        //    PlayBGM("Social Documentary02");
        //if (Input.GetKeyDown(KeyCode.Y))
        //    PlayBGM("�t�@�j�[�G�C���A��");
        //if (Input.GetKeyDown(KeyCode.U))
        //    PlayBGM("�[����`����");
        //if (Input.GetKeyDown(KeyCode.I))
        //    PlayBGM("");
        //if (Input.GetKeyDown(KeyCode.O))
        //    PlayBGM("");
        //if (Input.GetKeyDown(KeyCode.P))
        //    PlayBGM("");

        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    seAudioSource.Stop();
        //    bgmAudioSource.Stop();
        //}
    }

    /// <summary>
    /// �w�肳�ꂽSE�𗬂��܂��B
    /// <code>audioManager.PlaySE("SE�̑f�ނ̖��O");</code>
    /// </summary>
    /// <param name="seName"></param>
    public void PlaySE(string seName)
    {
        int seIndex = GetSEIndex(seName);
        if (seIndex >= 0)
        {
            seAudioSource.PlayOneShot(seAudioClips[seIndex], seAudioVolumes[seIndex] * audioSetting.overallVolume * audioSetting.seVolume);
        }
        else
        {
            Debug.LogError($"\"{seName}\"�Ƃ������O��audioClip�͑��݂��܂���B");
        }
    }

    /// <summary>
    /// �w�肳�ꂽBGM�𗬂��܂��B
    /// <code>audioManager.PlayBGM("BGM�̑f�ނ̖��O");</code>
    /// </summary>
    /// <param name="bgmName"></param>
    public void PlayBGM(string bgmName)
    {
        int bgmIndex = GetBGMIndex(bgmName);
        currentBGMIndex = bgmIndex;             // �ݒ�ŉ��ʂ�ς���Ƃ��Ɏg���܂�
        if (bgmIndex >= 0)
        {
            bgmAudioSource.Stop();
            bgmAudioSource.clip = bgmAudioClips[bgmIndex];      // �g�p���鉹���t�@�C����ݒ�
            bgmAudioSource.volume = bgmAudioVolumes[bgmIndex] * audioSetting.overallVolume * audioSetting.bgmVolume;    // ���ʂ�ύX
            currentBGMVolume = bgmAudioSource.volume;       // ���݂̉��ʂ��i�[�B�t�F�[�h�Ɏg���܂��B
            bgmAudioSource.Play();      // BGM�Đ��J�n
        }
        else
        {
            Debug.LogError($"\"{bgmName}\"�Ƃ������O��audioClip�͑��݂��܂���B");
        }
    }

    // ���ʉ��̃C���f�b�N�X���擾����
    private int GetSEIndex(string seName)
    {
        for (int i = 0; i < seAudioClips.Length; i++)
        {
            if (seAudioClips[i].name == seName)
            {
                return i;
            }
        }
        return -1;
    }

    // BGM�̃C���f�b�N�X���擾����
    private int GetBGMIndex(string bgmName)
    {
        for (int i = 0; i < bgmAudioClips.Length; i++)
        {
            if (bgmAudioClips[i].name == bgmName)
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// Option��ʂŐݒ��K���{�^�����������Ƃ��ɁA���Đ����Ă���BGM�̉��ʂ�ύX���܂��B
    /// </summary>
    public void UpdateBGMVolume()
    {
        bgmAudioSource.volume = bgmAudioVolumes[currentBGMIndex] * audioSetting.overallVolume * audioSetting.bgmVolume;
        currentBGMVolume = bgmAudioSource.volume;       // ���݂̉��ʂ��i�[�B�t�F�[�h�Ɏg���܂��B
    }

    /// <summary>
    /// �t�F�[�h�C�����Ȃ���BGM���ʂ�ς��܂��B
    /// </summary>
    public IEnumerator IEFadeInBGMVolume()
    {
        bgmAudioSource.UnPause();

        DOTween.Kill(fadeInTween);

        // ���l�̕ύX
        fadeInTween = DOTween.To(
            () => bgmAudioSource.volume,                // ����Ώۂɂ���̂�
            volume => bgmAudioSource.volume = volume,   // �l�̍X�V
            currentBGMVolume,                           // �ŏI�I�Ȓl
            fadeVolumeSpeed                             // �A�j���[�V��������
        );

        while (bgmAudioSource.volume != currentBGMVolume) yield return null;
    }

    /// <summary>
    /// �t�F�[�h�A�E�g���Ȃ���BGM���ʂ�ς��܂��B
    /// </summary>
    public IEnumerator IEFadeOutBGMVolume()
    {
        DOTween.Kill(fadeOutTween);

        // ���l�̕ύX
        fadeOutTween = DOTween.To(
            () => bgmAudioSource.volume,                // ����Ώۂɂ���̂�
            volume => bgmAudioSource.volume = volume,   // �l�̍X�V
            0f,                                         // �ŏI�I�Ȓl
            fadeVolumeSpeed                             // �A�j���[�V��������
        );

        while (bgmAudioSource.volume != 0) yield return null;

        bgmAudioSource.Pause();
    }
}
