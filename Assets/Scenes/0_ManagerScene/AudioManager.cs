using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioSetting audioSetting;


    [Header("SE関係")]
    [SerializeField] private AudioClip[] seAudioClips;
    [SerializeField] private float[] seAudioVolumes;

    [Header("BGM関係")]
    [SerializeField] private AudioClip[] bgmAudioClips;
    [SerializeField] private float[] bgmAudioVolumes;


    void Start()
    {
        // コンポーネント取得
        audioSource = GetComponent<AudioSource>();
        audioSetting = GetComponent<AudioSetting>();
    }
            
    /// <summary>
    /// 指定されたSEを流します。
    /// <code>audioManager.PlaySE("SEの素材の名前");</code>
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
            Debug.LogError($"\"{seName}\"という名前のaudioClipは存在しません。");
        }
    }

    /// <summary>
    /// 指定されたBGMを流します。
    /// <code>audioManager.PlayBGM("BGMの素材の名前");</code>
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
            Debug.LogError($"\"{bgmName}\"という名前のaudioClipは存在しません。");
        }
    }

    // 効果音のインデックスを取得する
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

    // BGMのインデックスを取得する
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
