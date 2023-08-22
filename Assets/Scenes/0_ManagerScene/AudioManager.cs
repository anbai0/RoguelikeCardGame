using UnityEngine;

/// <summary>
/// SE,BGMを管理します。
/// 音を流すときは、
/// AudioManager.Instance.PlaySE("SEの名前");
/// AudioManager.Instance.PlayBGM("BGMの名前");
/// 音を消すときは、
/// AudioManager.Instance.seAudioSource.Stop();
/// AudioManager.Instance.bgmAudioSource.Stop();
/// 一時停止、停止解除は、
/// bgmAudioSource.Pause
/// bgmAudioSource.UnPause
/// </summary>
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource seAudioSource;      // SE用のAudioSource
    [SerializeField] private AudioSource bgmAudioSource;     // BGM用のAudioSource
    private AudioSetting audioSetting;


    [Header("SE関係")]
    [SerializeField] private AudioClip[] seAudioClips;
    [SerializeField] private float[] seAudioVolumes;

    [Header("BGM関係")]
    [SerializeField] private AudioClip[] bgmAudioClips;
    [SerializeField] private float[] bgmAudioVolumes;


    public static AudioManager Instance;     // シングルトン
    private void Awake()
    {
        // シングルトンインスタンスをセットアップ
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        // コンポーネント取得
        audioSetting = GetComponent<AudioSetting>();
    }

    //private void Update()
    //{
    //    // SEチェック
    //    if (Input.GetKeyDown(KeyCode.Alpha1))
    //        PlaySE("タイトル画面");
    //    if (Input.GetKeyDown(KeyCode.Alpha2))
    //        PlaySE("選択音");
    //    if (Input.GetKeyDown(KeyCode.Alpha3))
    //        PlaySE("買い物");
    //    if (Input.GetKeyDown(KeyCode.Alpha4))
    //        PlaySE("マップ切り替え");
    //    if (Input.GetKeyDown(KeyCode.Alpha5))
    //        PlaySE("攻撃1");
    //    if (Input.GetKeyDown(KeyCode.Alpha6))
    //        PlaySE("回復");
    //    if (Input.GetKeyDown(KeyCode.Alpha7))
    //        PlaySE("バフ");
    //    if (Input.GetKeyDown(KeyCode.Alpha8))
    //        PlaySE("デバフ");
    //    if (Input.GetKeyDown(KeyCode.Alpha9))
    //        PlaySE("");
    //    if (Input.GetKeyDown(KeyCode.Alpha0))
    //        PlaySE("");

    //    // BGMチェック
    //    if (Input.GetKeyDown(KeyCode.Q))
    //        PlayBGM("Field1");
    //    if (Input.GetKeyDown(KeyCode.W))
    //        PlayBGM("Field2");
    //    if (Input.GetKeyDown(KeyCode.E))
    //        PlayBGM("Result");
    //    if (Input.GetKeyDown(KeyCode.R))
    //        PlayBGM("it's my turn");
    //    if (Input.GetKeyDown(KeyCode.T))
    //        PlayBGM("Social Documentary02");
    //    if (Input.GetKeyDown(KeyCode.Y))
    //        PlayBGM("ファニーエイリアン");
    //    if (Input.GetKeyDown(KeyCode.U))
    //        PlayBGM("深淵を覗く者");
    //    if (Input.GetKeyDown(KeyCode.I))
    //        PlayBGM("");
    //    if (Input.GetKeyDown(KeyCode.O))
    //        PlayBGM("");
    //    if (Input.GetKeyDown(KeyCode.P))
    //        PlayBGM("");

    //    if (Input.GetKeyDown(KeyCode.S))
    //    {
    //        seAudioSource.Stop();
    //        bgmAudioSource.Stop();
    //    }         
    //}

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
            seAudioSource.PlayOneShot(seAudioClips[seIndex], seAudioVolumes[seIndex] * audioSetting.overallVolume * audioSetting.seVolume);
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
            bgmAudioSource.Stop();
            bgmAudioSource.clip = bgmAudioClips[bgmIndex];      // 使用する音声ファイルを設定
            bgmAudioSource.volume = bgmAudioVolumes[bgmIndex] * audioSetting.overallVolume * audioSetting.bgmVolume;    // 音量を変更
            bgmAudioSource.Play();      // BGM再生開始

            //bgmAudioSource.PlayOneShot(bgmAudioClips[bgmIndex], bgmAudioVolumes[bgmIndex] * audioSetting.overallVolume * audioSetting.bgmVolume);
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
