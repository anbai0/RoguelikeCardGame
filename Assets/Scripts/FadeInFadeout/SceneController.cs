using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;//�ǉ�
using UnityEngine.SceneManagement;//�ǉ�
using System.Security.Cryptography.X509Certificates;

//��{��Main Camera�ɃA�^�b�`���Ă�������
public class SceneController : MonoBehaviour
{
    public GameObject fade;//�C���X�y�N�^����Prefab������Fade������
    private GameObject fadeCanvas;//���삷��Canvas�A�^�O�ŒT��

    private int fadeDelay;

    [SerializeField]
    private FadeManager fadeManager;

    void Start()
    {
        float tmp = fadeManager.FadeSpeed * 10; //int�ɕϊ����邽�߂�10�{�ɂ��Ă܂�
        fadeDelay = (int)tmp;
        if (!FadeManager.isFadeInstance)
        {
            Instantiate(fade);
        }

        Invoke("findFadeObject", fadeDelay / 100f);//�N�����p��Canvas�̏�����������Ƒ҂�
    }

    void findFadeObject()
    {
        fadeCanvas = GameObject.FindGameObjectWithTag("Fade");//Canvas���݂���
        fadeCanvas.GetComponent<FadeManager>().fadeIn();//�t�F�[�h�C���t���O�𗧂Ă�
    }


    /// <summary>
    /// �V�[���̃��[�h�ƃA�����[�h���s���܂��B
    /// <para>�V�[���̃��[�h�A�A�����[�h�𓯎��ɍs���ꍇ�́A</para>
    /// <code>SceneChange("���[�h�������V�[���̖��O", "�A�����[�h�������V�[���̖��O");</code>
    /// <para>�V�[���̃��[�h�����������ꍇ�́A</para>
    /// <code>SceneChange("���[�h�������V�[���̖��O");</code>
    /// <para>�V�[���̃A�����[�h�����������ꍇ�́A</para>
    /// <code>SceneChange(unLoadSceneName: "�A�����[�h�������V�[���̖��O");</code>
    /// </summary>
    /// <param name="loadSceneName"></param>
    /// <param name="unLoadSceneName"></param>
    public async void SceneChange(string loadSceneName = "None", string unLoadSceneName = "None")//�{�^������ȂǂŌĂяo��
    {
        fadeCanvas.SetActive(true);     //�p�l�����ז��ŏ����Ă����̂����ŕ\�������Ă��܂�
        fadeCanvas.GetComponent<FadeManager>().fadeOut();//�t�F�[�h�A�E�g�t���O�𗧂Ă�
        await Task.Delay(fadeDelay * 100);//�Ó]����܂ő҂�
        if (loadSceneName != "None") SceneManager.LoadSceneAsync(loadSceneName, LoadSceneMode.Additive);    //�V�[�����[�h
        if (unLoadSceneName != "None") StartCoroutine(CoUnload(unLoadSceneName));                           //�A�����[�h
    }

    IEnumerator CoUnload(string unLoadSceneName)
    {
        //�w�肵���V�[�����A�����[�h
        var op = SceneManager.UnloadSceneAsync(unLoadSceneName);
        yield return op;

        //�A�����[�h��̏���������

        //�K�v�ɉ����ĕs�g�p�A�Z�b�g���A�����[�h���ă��������������
        //���������d�������Ȃ̂ŁA�ʂɊǗ�����̂���
        yield return Resources.UnloadUnusedAssets();
    }
}