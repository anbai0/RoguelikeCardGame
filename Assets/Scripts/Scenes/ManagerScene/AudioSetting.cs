using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class AudioSetting : MonoBehaviour
{
    AudioManager audioManager;

    [SerializeField] private Slider overallVolumeSlider;
    [SerializeField] private Slider seVolumeSlider;
    [SerializeField] private Slider bgmVolumeSlider;

    [SerializeField] TextMeshProUGUI overallVolumeText;
    [SerializeField] TextMeshProUGUI seVolumeText;
    [SerializeField] TextMeshProUGUI bgmVolumeText;

    private float maxVolume = 1f;

    void Start()
    {
        audioManager = GetComponent<AudioManager>();

        // �ő剹�ʐݒ�
        overallVolumeSlider.maxValue = maxVolume;
        seVolumeSlider.maxValue = maxVolume;
        bgmVolumeSlider.maxValue = maxVolume;

        // ���݂̉��ʂ�ݒ�
        overallVolumeSlider.value = audioManager.overallVolume;
        seVolumeSlider.value = audioManager.SEVolume;
        bgmVolumeSlider.value = audioManager.BGMVolume;

        // ���݂̉��ʕ\��
        overallVolumeText.text = audioManager.overallVolume.ToString();
        seVolumeText.text = audioManager.SEVolume.ToString();
        bgmVolumeText.text = audioManager.BGMVolume.ToString();

        // ���X�i�[�o�^
        overallVolumeSlider.onValueChanged.AddListener(OnOverallValueChanged);
        seVolumeSlider.onValueChanged.AddListener(OnSEValueChanged);
        bgmVolumeSlider.onValueChanged.AddListener(OnBGMValueChanged);
    }

    private void OnOverallValueChanged(float value)
    {
        float roundedValue = Mathf.Floor(value * 100) / 100; // �����_���ʂ�؂�̂Ă�
        Debug.Log("�S�̂̉��ʃX���C�_�[�̒l���ύX����܂���: " + roundedValue);
        overallVolumeText.text = roundedValue.ToString();
        audioManager.overallVolume = roundedValue;
    }

    private void OnSEValueChanged(float value)
    {
        float roundedValue = Mathf.Floor(value * 100) / 100; // �����_���ʂ�؂�̂Ă�
        Debug.Log("SE�̉��ʃX���C�_�[�̒l���ύX����܂���: " + roundedValue);
        seVolumeText.text = roundedValue.ToString();
        audioManager.SEVolume = roundedValue;
    }

    private void OnBGMValueChanged(float value)
    {
        float roundedValue = Mathf.Floor(value * 100) / 100; // �����_���ʂ�؂�̂Ă�
        Debug.Log("BGM�̉��ʃX���C�_�[�̒l���ύX����܂���: " + roundedValue);
        bgmVolumeText.text = roundedValue.ToString();
        audioManager.BGMVolume = roundedValue;
    }
}
