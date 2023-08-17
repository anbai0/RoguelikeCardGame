using System.Collections;
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
    private FadeEffectController fadeManager;

    private GameObject fade;        // DontDestroy���ꂽFadePrefab���i�[

    private float fadeDelay;
    private float fadeDuration;

    void Start()
    {
        fadeDelay = fadeManager.fadeSpeed;
        fadeDuration = fadeManager.fadeDurationMultiplier;

        if (!FadeEffectController.isFadeInstance && gameObject.scene.name == "ManagerScene")        // FadePrefab����������Ă��Ȃ��ꍇ����
        {
            Instantiate(fadePrefab);
        }

        Invoke("findFadeObject", fadeManager.fadeSpeed / 10f);      // �N�����p��Canvas�̐�����������Ƒ҂�
    }

    void findFadeObject()
    {
        fade = GameObject.FindGameObjectWithTag("Fade");            // Canvas���݂���
        fade.GetComponent<FadeEffectController>().fadeIn();         // �t�F�[�h�C���t���O�𗧂Ă�
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
        fade.SetActive(true);     // �p�l�����ז��ŏ����Ă����̂����ŕ\�������Ă��܂�

        fade.GetComponent<FadeEffectController>().fadeOut();         // �t�F�[�h�A�E�g
        await Task.Delay((int)(fadeDelay * fadeDuration * 1000));    // �Ó]����܂ő҂�(�~���b�P��)

        //if (loadSceneName != "None") SceneManager.LoadSceneAsync(loadSceneName, LoadSceneMode.Additive);    // �V�[�����[�h
        //if (unLoadSceneName != "None") StartCoroutine(CoUnload(unLoadSceneName));                           // �A�����[�h

        if (loadSceneName != "None")
            await LoadSceneAsync(loadSceneName);

        if (unLoadSceneName != "None")
            StartCoroutine(CoUnload(unLoadSceneName));

        // �A�����[�h�����s�����ꍇ�A�܂��̓V�[�������w�肳��Ȃ������ꍇ
        if (loadSceneName == "None" && (unLoadSceneName != "None" || unLoadSceneName == "None"))
            fade.GetComponent<FadeEffectController>().fadeIn();      // �t�F�[�h�C��
    }

    /// <summary>
    /// �w�肵���V�[���̕\����؂�ւ��A�t�F�[�h�C���A�A�E�g�����܂��B
    /// </summary>
    /// <param name="toggleSceneName"></param>
    /// <param name="isSceneActive"></param>
    public async void ToggleSceneWithFade(string toggleSceneName, bool isSceneActive)
    {
        fade.SetActive(true);     // �p�l�����ז��ŏ����Ă����̂����ŕ\�������Ă��܂�

        fade.GetComponent<FadeEffectController>().fadeOut();         // �t�F�[�h�A�E�g
        await Task.Delay((int)(fadeDelay * fadeDuration * 1000));    // �Ó]����܂ő҂�(�~���b�P��)

        toggleSceneName.ToggleSceneDisplay(isSceneActive);      // �V�[���̕\���ؑ�

        fade.GetComponent<FadeEffectController>().fadeIn();      // �t�F�[�h�C��
    }


    private async Task LoadSceneAsync(string sceneName)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!loadOperation.isDone)
        {
            await Task.Yield();
        }
    }

    /// <summary>
    /// �t�F�[�h�͍s�킸�A
    /// string�Ŏw�肳�ꂽ�V�[�����A�����[�h���܂��B
    /// </summary>
    public void UnLoadScene(string unLoadSceneName)
    {
        StartCoroutine(CoUnload(unLoadSceneName));
    }

    IEnumerator CoUnload(string unLoadSceneName)
    {
        // �w�肵���V�[�����A�����[�h
        var op = SceneManager.UnloadSceneAsync(unLoadSceneName);
        yield return op;

        // �A�����[�h��̏���������

        // �K�v�ɉ����ĕs�g�p�A�Z�b�g���A�����[�h���ă��������������
        yield return Resources.UnloadUnusedAssets();
    }
}