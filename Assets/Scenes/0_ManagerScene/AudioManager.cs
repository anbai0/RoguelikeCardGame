using DG.Tweening;
using UnityEngine;
using System.Collections;

/// <summary>
/// SE,BGMを管理します。
/// <para>音を流すときは、</para>
/// <code>AudioManager.Instance.PlaySE("SEの名前");</code>
/// <code>AudioManager.Instance.PlayBGM("BGMの名前");</code>
/// <para>音を消すときは、</para>
/// <code>AudioManager.Instance.seAudioSource.Stop();</code>
/// <code>AudioManager.Instance.bgmAudioSource.Stop();</code>
/// <para>一時停止、停止解除は、</para>
/// <code>AudioManager.Instance.bgmAudioSource.Pause();</code>
/// <code>AudioManager.Instance.bgmAudioSource.UnPause();</code>
/// <para>フェードありの一時停止、停止解除は、</para>
/// <code>AudioManager.Instance.StartCoroutine(AudioManager.Instance.IEFadeInBGMVolume());</code>
/// <code>AudioManager.Instance.StartCoroutine(AudioManager.Instance.IEFadeOutBGMVolume());</code>
/// </summary>
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource seAudioSource;      // SE用のAudioSource
    [SerializeField] public AudioSource bgmAudioSource;      // BGM用のAudioSource
    private AudioSetting audioSetting;

    [Header("SE関係")]
    [SerializeField] private AudioClip[] seAudioClips;
    [SerializeField] private float[] seAudioVolumes;

    [Header("BGM関係")]
    [SerializeField] private AudioClip[] bgmAudioClips;
    [SerializeField] private float[] bgmAudioVolumes;

    private int currentBGMIndex;        // 設定で音量を変えるときに使います
    private float currentBGMVolume;     // 音をフェードする時に使います
    private float fadeVolumeSpeed = 1.5f;
    private Tween fadeInTween = null;
    private Tween fadeOutTween = null;

    public static AudioManager Instance;     // シングルトン
    private void Awake()
    {
        // シングルトンインスタンスをセットアップ
        if (Instance == null)
        {
            Instance = this;
        }
        // コンポーネント取得
        audioSetting = GetComponent<AudioSetting>();

        DOTween.SetTweensCapacity(1000, 50);    // Tween用のメモリ確保
    }


    private void Update()
    {
        //// SEチェック
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //    PlaySE("タイトル画面");
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //    PlaySE("選択音");
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //    PlaySE("買い物");
        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //    PlaySE("マップ切り替え");
        //if (Input.GetKeyDown(KeyCode.Alpha5))
        //    PlaySE("攻撃1");
        //if (Input.GetKeyDown(KeyCode.Alpha6))
        //    PlaySE("回復");
        //if (Input.GetKeyDown(KeyCode.Alpha7))
        //    PlaySE("バフ");
        //if (Input.GetKeyDown(KeyCode.Alpha8))
        //    PlaySE("デバフ");
        //if (Input.GetKeyDown(KeyCode.Alpha9))
        //    PlaySE("");
        //if (Input.GetKeyDown(KeyCode.Alpha0))
        //    PlaySE("");

        //// BGMチェック
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
        //    PlayBGM("ファニーエイリアン");
        //if (Input.GetKeyDown(KeyCode.U))
        //    PlayBGM("深淵を覗く者");
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
        currentBGMIndex = bgmIndex;             // 設定で音量を変えるときに使います
        if (bgmIndex >= 0)
        {
            bgmAudioSource.Stop();
            bgmAudioSource.clip = bgmAudioClips[bgmIndex];      // 使用する音声ファイルを設定
            bgmAudioSource.volume = bgmAudioVolumes[bgmIndex] * audioSetting.overallVolume * audioSetting.bgmVolume;    // 音量を変更
            currentBGMVolume = bgmAudioSource.volume;       // 現在の音量を格納。フェードに使います。
            bgmAudioSource.Play();      // BGM再生開始
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

    /// <summary>
    /// Option画面で設定を適応ボタンを押したときに、今再生しているBGMの音量を変更します。
    /// </summary>
    public void UpdateBGMVolume()
    {
        bgmAudioSource.volume = bgmAudioVolumes[currentBGMIndex] * audioSetting.overallVolume * audioSetting.bgmVolume;
        currentBGMVolume = bgmAudioSource.volume;       // 現在の音量を格納。フェードに使います。
    }

    /// <summary>
    /// フェードインしながらBGM音量を変えます。
    /// </summary>
    public IEnumerator IEFadeInBGMVolume()
    {
        bgmAudioSource.UnPause();

        DOTween.Kill(fadeInTween);

        // 数値の変更
        fadeInTween = DOTween.To(
            () => bgmAudioSource.volume,                // 何を対象にするのか
            volume => bgmAudioSource.volume = volume,   // 値の更新
            currentBGMVolume,                           // 最終的な値
            fadeVolumeSpeed                             // アニメーション時間
        );

        while (bgmAudioSource.volume != currentBGMVolume) yield return null;
    }

    /// <summary>
    /// フェードアウトしながらBGM音量を変えます。
    /// </summary>
    public IEnumerator IEFadeOutBGMVolume()
    {
        DOTween.Kill(fadeOutTween);

        // 数値の変更
        fadeOutTween = DOTween.To(
            () => bgmAudioSource.volume,                // 何を対象にするのか
            volume => bgmAudioSource.volume = volume,   // 値の更新
            0f,                                         // 最終的な値
            fadeVolumeSpeed                             // アニメーション時間
        );

        while (bgmAudioSource.volume != 0) yield return null;

        bgmAudioSource.Pause();
    }
}
