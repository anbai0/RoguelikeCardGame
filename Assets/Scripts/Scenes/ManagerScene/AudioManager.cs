using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("�S�̉���")]
    [SerializeField, Range(0f, 1f)] public float overallVolume = 1f;

    [Header("SE�֌W")]
    [SerializeField] private AudioClip[] seAudioClips;
    [SerializeField] private float[] seAudioVolumes;
    [SerializeField, Range(0f, 1f)] public float seVolume = 0.5f;

    [Header("BGM�֌W")]
    [SerializeField] private AudioClip[] bgmAudioClips;
    [SerializeField] private float[] bgmAudioVolumes;
    [SerializeField, Range(0f, 1f)] public float bgmVolume = 0.5f;


    void Start()
    {
        // AudioSource�̃R���|�[�l���g�擾
        audioSource = GetComponent<AudioSource>();
    }

    // ���ʉ����Đ�����
    public void PlaySE(string seName)
    {
        int seIndex = GetSEIndex(seName);
        if (seIndex >= 0)
        {
            audioSource.PlayOneShot(seAudioClips[seIndex], seAudioVolumes[seIndex] * overallVolume * seVolume);
        }
        else
        {
            Debug.Log(seName + "�Ƃ������O��audioClip�͑��݂��܂���B");
        }
    }

    // BGM���Đ�����
    public void PlayBGM(string bgmName)
    {
        int bgmIndex = GetBGMIndex(bgmName);
        if (bgmIndex >= 0)
        {
            audioSource.PlayOneShot(bgmAudioClips[bgmIndex], bgmAudioVolumes[bgmIndex] * overallVolume * bgmVolume);
        }
        else
        {
            Debug.Log(bgmName + "�Ƃ������O��audioClip�͑��݂��܂���B");
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
