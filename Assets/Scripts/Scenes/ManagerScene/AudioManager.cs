using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Overall Volume")]
    [SerializeField, Range(0f, 1f)] public float overallVolume = 1f;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip[] SEaudioClips;
    [SerializeField] private float[] SEaudioVolumes;
    [SerializeField, Range(0f, 1f)] public float SEVolume = 0.5f;

    [Header("Background Music")]
    [SerializeField] private AudioClip[] BGMaudioClips;
    [SerializeField] private float[] BGMaudioVolumes;
    [SerializeField, Range(0f, 1f)] public float BGMVolume = 0.5f;


    [SerializeField] int maxOverallVolume = 1;
    [SerializeField] int maxSEVolume = 2;
    [SerializeField] int maxBGMVolume = 2;

    void Start()
    {
        // AudioSourceのコンポーネント取得
        audioSource = GetComponent<AudioSource>();
    }

    // 効果音を再生する
    public void SEPlay(string seName)
    {
        int seIndex = GetSEIndex(seName);
        if (seIndex >= 0)
        {
            audioSource.PlayOneShot(SEaudioClips[seIndex], SEaudioVolumes[seIndex] * overallVolume * SEVolume);
        }
        else
        {
            Debug.Log(seName + "という名前のaudioClipは存在しません。");
        }
    }

    // BGMを再生する
    public void BGMPlay(string bgmName)
    {
        int bgmIndex = GetBGMIndex(bgmName);
        if (bgmIndex >= 0)
        {
            audioSource.PlayOneShot(BGMaudioClips[bgmIndex], BGMaudioVolumes[bgmIndex] * overallVolume * BGMVolume);
        }
        else
        {
            Debug.Log(bgmName + "という名前のaudioClipは存在しません。");
        }
    }

    // 効果音のインデックスを取得する
    private int GetSEIndex(string seName)
    {
        for (int i = 0; i < SEaudioClips.Length; i++)
        {
            if (SEaudioClips[i].name == seName)
            {
                return i;
            }
        }
        return -1;
    }

    // BGMのインデックスを取得する
    private int GetBGMIndex(string bgmName)
    {
        for (int i = 0; i < BGMaudioClips.Length; i++)
        {
            if (BGMaudioClips[i].name == bgmName)
            {
                return i;
            }
        }
        return -1;
    }
}
