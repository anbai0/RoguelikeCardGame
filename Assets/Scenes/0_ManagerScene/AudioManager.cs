using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioSetting audioSetting;


    [Header("SE�֌W")]
    [SerializeField] private AudioClip[] seAudioClips;
    [SerializeField] private float[] seAudioVolumes;

    [Header("BGM�֌W")]
    [SerializeField] private AudioClip[] bgmAudioClips;
    [SerializeField] private float[] bgmAudioVolumes;


    void Start()
    {
        // �R���|�[�l���g�擾
        audioSource = GetComponent<AudioSource>();
        audioSetting = GetComponent<AudioSetting>();
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
            audioSource.PlayOneShot(seAudioClips[seIndex], seAudioVolumes[seIndex] * audioSetting.overallVolume * audioSetting.seVolume);
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
        if (bgmIndex >= 0)
        {
            audioSource.PlayOneShot(bgmAudioClips[bgmIndex], bgmAudioVolumes[bgmIndex] * audioSetting.overallVolume * audioSetting.bgmVolume);
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
}
