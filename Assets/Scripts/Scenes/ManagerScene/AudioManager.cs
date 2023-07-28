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
        // AudioSource�̃R���|�[�l���g�擾
        audioSource = GetComponent<AudioSource>();
    }

    // ���ʉ����Đ�����
    public void SEPlay(string seName)
    {
        int seIndex = GetSEIndex(seName);
        if (seIndex >= 0)
        {
            audioSource.PlayOneShot(SEaudioClips[seIndex], SEaudioVolumes[seIndex] * overallVolume * SEVolume);
        }
        else
        {
            Debug.Log(seName + "�Ƃ������O��audioClip�͑��݂��܂���B");
        }
    }

    // BGM���Đ�����
    public void BGMPlay(string bgmName)
    {
        int bgmIndex = GetBGMIndex(bgmName);
        if (bgmIndex >= 0)
        {
            audioSource.PlayOneShot(BGMaudioClips[bgmIndex], BGMaudioVolumes[bgmIndex] * overallVolume * BGMVolume);
        }
        else
        {
            Debug.Log(bgmName + "�Ƃ������O��audioClip�͑��݂��܂���B");
        }
    }

    // ���ʉ��̃C���f�b�N�X���擾����
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

    // BGM�̃C���f�b�N�X���擾����
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
