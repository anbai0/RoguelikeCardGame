using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using SelfMadeNamespace;
using System;
using Unity.Mathematics;

// ��{��Main Camera�ɃA�^�b�`���Ă�������
public class SceneFader : MonoBehaviour
{
    [SerializeField]
    [Header("��������FadePrefab")]
    private GameObject fadePrefab;

    [SerializeField]
    [Header("FadePrefab��Script")]
    private FadeController fadeManager;

    private GameObject fade;        // DontDestroy���ꂽFadePrefab���i�[

    private int fadeTime = 1500;       // �~���b

    public static SceneFader Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        if (!FadeController.isFadeInstance)        // FadePrefab����������Ă��Ȃ��ꍇ����
        {
            Instantiate(fadePrefab);
        }
        Invoke("findFadeObject", 0.08f);      // �N�����p��Canvas�̐�����������Ƒ҂�
    }

    async void findFadeObject()
    {
        fade = GameObject.FindGameObjectWithTag("Fade");            // Canvas���݂���
        await fade.GetComponent<FadeController>().fadeIn();         // �t�F�[�h�C���t���O�𗧂Ă�
    }


    /// <summary>
    /// �V�[���̃��[�h�ƃA�����[�h���s���A�t�F�[�h�C���A�A�E�g�����܂��B
    /// <para>�V�[���̃��[�h�A�A�����[�h�𓯎��ɍs���ꍇ�́A</para>
    /// <code>SceneChange("���[�h�������V�[���̖��O", "�A�����[�h�������V�[���̖��O");</code>
    /// <para>�V�[���̃��[�h�����������ꍇ�́A</para>
    /// <code>SceneChange("���[�h�������V�[���̖��O");</code>
    /// <para>�V�[���̃A�����[�h�����������ꍇ�́A</para>
    /// <code>SceneChange(unLoadSceneName: "�A�����[�h�������V�[���̖��O");</code>
    /// <para>�t�F�[�h�C�����I��������Ƀv���C���[�𓮂���悤�ɂ������ꍇ</para>
    /// <code>allowPlayerMove: true</code>
    /// </summary>
    /// <param name="loadSceneName"></param>
    /// <param name="unLoadSceneName"></param>
    public async void SceneChange(string loadSceneName = "None", string unLoadSceneName = "None", bool allowPlayerMove = false)
    {
        fade.SetActive(true);   // �p�l�����ז��ŏ����Ă����̂����ŕ\�������Ă��܂�

        FadeController fadeController = fade.GetComponent<FadeController>();

        // �t�F�[�h�A�E�g���t�F�[�h�A�E�g���I���܂őҋ@
        await fadeController.fadeOut();

        await Task.Delay((int)(fadeTime));       // ���̒����Ó]������(�~���b�P��)

        // ���[�h�V�[�����w�肳��Ă����ꍇ�A�V�[�������[�h
        if (loadSceneName != "None") await LoadSceneAsyncTask(loadSceneName);

        // �A�����[�h�V�[�����w�肳��Ă����ꍇ�A�V�[�����A�����[�h
        if (unLoadSceneName != "None") await UnLoadSceneAsyncTask(unLoadSceneName);    

        // �t�F�[�h�C�����t�F�[�h�C�����I���܂őҋ@
        await fadeController.fadeIn();

        // �t�F�[�h�C����Ƀv���C���[�𓮂���悤�ɂ���
        if (allowPlayerMove)
            PlayerController.Instance.isEvents = !allowPlayerMove;
    }


    /// <summary>
    /// �w�肵���V�[���̕\����؂�ւ��A�t�F�[�h�C���A�A�E�g�����܂��B
    /// <para>�t�F�[�h�C�����I��������Ƀv���C���[�𓮂���悤�ɂ������ꍇ</para>
    /// <code>allowPlayerMove: true</code>
    /// </summary>
    /// <param name="toggleSceneName"></param>
    /// <param name="isSceneActive"></param>
    public async void ToggleSceneWithFade(string toggleSceneName, bool isSceneActive, bool allowPlayerMove = false)
    {
        fade.SetActive(true);     // �p�l�����ז��ŏ����Ă����̂����ŕ\�������Ă��܂�

        FadeController fadeController = fade.GetComponent<FadeController>();

        // �t�F�[�h�A�E�g���t�F�[�h�A�E�g���I���܂őҋ@
        await fadeController.fadeOut();

        await Task.Delay(fadeTime);    // ���̒����Ó]������(�~���b�P��)

        toggleSceneName.ToggleSceneDisplay(isSceneActive);      // �V�[���̕\���ؑ�

        // �t�F�[�h�C�����t�F�[�h�C�����I���܂őҋ@
        await fadeController.fadeIn();

        // �t�F�[�h�C����Ƀv���C���[�𓮂���悤�ɂ���
        if (allowPlayerMove)
            PlayerController.Instance.isEvents = !allowPlayerMove;
    }

    /// <summary>
    /// ���ׂẴV�[�����A�����[�h����Ƃ��Ɏg���܂��B
    /// ���\�b�h���󂯎��t�F�[�h�C���A�A�E�g�����܂��B
    /// </summary>
    public async void FadeOutInWrapper(Func<Task> method, bool allowPlayerMove = false)
    {
        fade.SetActive(true);     // �p�l�����ז��ŏ����Ă����̂����ŕ\�������Ă��܂�

        FadeController fadeController = fade.GetComponent<FadeController>();

        // �t�F�[�h�A�E�g���t�F�[�h�A�E�g���I���܂őҋ@
        await fadeController.fadeOut();

        await Task.Delay(fadeTime);    // ���̒����Ó]������(�~���b�P��)

        await method?.Invoke();

        // �t�F�[�h�C�����t�F�[�h�C�����I���܂őҋ@
        await fadeController.fadeIn();

        // �t�F�[�h�C����Ƀv���C���[�𓮂���悤�ɂ���
        if (allowPlayerMove)
            PlayerController.Instance.isEvents = !allowPlayerMove;
    }

    private async Task LoadSceneAsyncTask(string loadSceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(loadSceneName, LoadSceneMode.Additive);
        while (!asyncOperation.isDone) await Task.Yield();
    }

    private async Task UnLoadSceneAsyncTask(string unLoadSceneName)
    {
        AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(unLoadSceneName);
        while (!asyncOperation.isDone) await Task.Yield();

        Resources.UnloadUnusedAssets();
    }



}