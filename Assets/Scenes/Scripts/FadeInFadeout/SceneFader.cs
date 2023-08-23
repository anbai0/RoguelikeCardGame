using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using SelfMadeNamespace;
using System;

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

    private float fadeDelay;
    private float fadeDuration;

    void Start()
    {
        fadeDelay = fadeManager.fadeSpeed;
        fadeDuration = fadeManager.fadeDurationMultiplier;

        if (!FadeController.isFadeInstance && gameObject.scene.name == "ManagerScene")        // FadePrefab����������Ă��Ȃ��ꍇ����
        {
            Instantiate(fadePrefab);
        }
        Invoke("findFadeObject", fadeDelay * fadeDuration / 10f);      // �N�����p��Canvas�̐�����������Ƒ҂�
    }

    void findFadeObject()
    {
        fade = GameObject.FindGameObjectWithTag("Fade");            // Canvas���݂���
        fade.GetComponent<FadeController>().fadeIn();         // �t�F�[�h�C���t���O�𗧂Ă�
    }


    /// <summary>
    /// �V�[���̃��[�h�ƃA�����[�h���s���A�t�F�[�h�C���A�A�E�g�����܂��B
    /// <para>�V�[���̃��[�h�A�A�����[�h�𓯎��ɍs���ꍇ�́A</para>
    /// <code>SceneChange("���[�h�������V�[���̖��O", "�A�����[�h�������V�[���̖��O");</code>
    /// <para>�V�[���̃��[�h�����������ꍇ�́A</para>
    /// <code>SceneChange("���[�h�������V�[���̖��O");</code>
    /// <para>�V�[���̃A�����[�h�����������ꍇ�́A</para>
    /// <code>SceneChange(unLoadSceneName: "�A�����[�h�������V�[���̖��O");</code>
    /// </summary>
    /// <param name="loadSceneName"></param>
    /// <param name="unLoadSceneName"></param>
    public async void SceneChange(string loadSceneName = "None", string unLoadSceneName = "None")
    {
        fade.SetActive(true);   // �p�l�����ז��ŏ����Ă����̂����ŕ\�������Ă��܂�

        FadeController fadeController = fade.GetComponent<FadeController>();

        // �t�F�[�h�A�E�g���t�F�[�h�A�E�g���I���܂őҋ@
        await fadeController.fadeOut();

        await Task.Delay((int)(fadeDelay * fadeDuration * 1000));       // ���̒����Ó]������(�~���b�P��)

        // ���[�h�V�[�����w�肳��Ă����ꍇ�A�V�[�������[�h
        if (loadSceneName != "None") await LoadSceneAsyncTask(loadSceneName);

        // �A�����[�h�V�[�����w�肳��Ă����ꍇ�A�V�[�����A�����[�h
        if (unLoadSceneName != "None") await UnLoadSceneAsyncTask(unLoadSceneName);

        // �A�����[�h�����s���ꍇ�����Ńt�F�[�h�C��������
        if (loadSceneName == "None")
        {
            // �t�F�[�h�C�����t�F�[�h�C�����I���܂őҋ@
            await fadeController.fadeIn();
        }

    }


    /// <summary>
    /// �w�肵���V�[���̕\����؂�ւ��A�t�F�[�h�C���A�A�E�g�����܂��B
    /// </summary>
    /// <param name="toggleSceneName"></param>
    /// <param name="isSceneActive"></param>
    public async void ToggleSceneWithFade(string toggleSceneName, bool isSceneActive)
    {
        fade.SetActive(true);     // �p�l�����ז��ŏ����Ă����̂����ŕ\�������Ă��܂�

        FadeController fadeController = fade.GetComponent<FadeController>();

        // �t�F�[�h�A�E�g���t�F�[�h�A�E�g���I���܂őҋ@
        await fadeController.fadeOut();

        await Task.Delay((int)(fadeDelay * fadeDuration * 1000));    // ���̒����Ó]������(�~���b�P��)

        toggleSceneName.ToggleSceneDisplay(isSceneActive);      // �V�[���̕\���ؑ�

        // �t�F�[�h�C�����t�F�[�h�C�����I���܂őҋ@
        await fadeController.fadeIn();

    }

    /// <summary>
    /// ���ׂẴV�[�����A�����[�h����Ƃ��Ɏg���܂��B
    /// ���\�b�h���󂯎��t�F�[�h�C���A�A�E�g�����܂��B
    /// </summary>
    public async void FadeOutInWrapper(Func<Task> method)
    {
        fade.SetActive(true);     // �p�l�����ז��ŏ����Ă����̂����ŕ\�������Ă��܂�

        FadeController fadeController = fade.GetComponent<FadeController>();

        // �t�F�[�h�A�E�g���t�F�[�h�A�E�g���I���܂őҋ@
        await fadeController.fadeOut();

        await Task.Delay((int)(fadeDelay * fadeDuration * 1000));    // ���̒����Ó]������(�~���b�P��)

        await method?.Invoke();

        await Task.Delay((int)(fadeDelay * fadeDuration * 1000));    // ���̒����Ó]������(�~���b�P��)

        // �t�F�[�h�C�����t�F�[�h�C�����I���܂őҋ@
        await fadeController.fadeIn();
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