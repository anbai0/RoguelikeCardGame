using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("全体音量")]
    [SerializeField, Range(0f, 1f)] public float overallVolume = 1f;

    [Header("SE関係")]
    [SerializeField] private AudioClip[] seAudioClips;
    [SerializeField] private float[] seAudioVolumes;
    [SerializeField, Range(0f, 1f)] public float seVolume = 0.5f;

    [Header("BGM関係")]
    [SerializeField] private AudioClip[] bgmAudioClips;
    [SerializeField] private float[] bgmAudioVolumes;
    [SerializeField, Range(0f, 1f)] public float bgmVolume = 0.5f;


    void Start()
    {
        // AudioSourceのコンポーネント取得
        audioSource = GetComponent<AudioSource>();
    }

    // 効果音を再生する
    public void PlaySE(string seName)
    {
        int seIndex = GetSEIndex(seName);
        if (seIndex >= 0)
        {
            audioSource.PlayOneShot(seAudioClips[seIndex], seAudioVolumes[seIndex] * overallVolume * seVolume);
        }
        else
        {
            Debug.Log(seName + "という名前のaudioClipは存在しません。");
        }
    }

    // BGMを再生する
    public void PlayBGM(string bgmName)
    {
        int bgmIndex = GetBGMIndex(bgmName);
        if (bgmIndex >= 0)
        {
            audioSource.PlayOneShot(bgmAudioClips[bgmIndex], bgmAudioVolumes[bgmIndex] * overallVolume * bgmVolume);
        }
        else
        {
            Debug.Log(bgmName + "という名前のaudioClipは存在しません。");
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
