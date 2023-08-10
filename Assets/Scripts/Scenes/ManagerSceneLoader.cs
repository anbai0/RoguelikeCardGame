using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ManagerSceneLoader : MonoBehaviour
{
    private static bool Loaded { get; set; }

    GameManager gameManager;

    void Awake()
    {
        if (Loaded)
        {
            // ������V�[���ɉ�����UI��؂�ւ���
            GetCurrentSceneName();
            return;
        }       

        Loaded = true;
        StartCoroutine(LoadManagerScene()); // �R���[�`���̌Ăяo��
    }

    private IEnumerator LoadManagerScene()
    {
        // ManagerScene��񓯊��Ń��[�h
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("ManagerScene", LoadSceneMode.Additive);

        // ���[�h����������܂őҋ@
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // ManagerScene�����[�h���ꂽ���GetCurrentSceneName��GetUIManager���Ăяo��
        GetCurrentSceneName();
    }

    /// <summary>
    /// ������V�[������肵�A�V�[���ɉ����ď������܂��B
    /// </summary>
    void GetCurrentSceneName()
    {
        switch (gameObject.name)
        {
            case "TitleSceneManager":
                GetUIManager("Title");
                break;
            case "CharacterSceneManager":
                GetUIManager("None");
                break;
            case "ResultSceneManager":
                GetUIManager("None");
                break;
            default:
                GetUIManager("OverlayOnly");
                break;
        }
    }

    /// <summary>
    /// ManagerScene����UIManager���擾���AUIManager��ChangeUI���\�b�h���g���AUI��؂�ւ��܂��B
    /// </summary>
    /// <param name="uiType">ChangeUI�Ɏg������</param>
    private void GetUIManager(string uiType)
    {
        Scene scene = SceneManager.GetSceneByName("ManagerScene");

        foreach (var rootGameObject in scene.GetRootGameObjects())
        {
            UIManager uiManager = rootGameObject.GetComponent<UIManager>();
            if (uiManager != null)
            {
                uiManager.ChangeUI(uiType);
                break;
            }
        }
    }

    public GameManager GetGameManager()
    {
        Scene scene = SceneManager.GetSceneByName("ManagerScene");

        foreach (var rootGameObject in scene.GetRootGameObjects())
        {
            gameManager = rootGameObject.GetComponent<GameManager>();
            return gameManager;
        }
        Debug.LogError("GameManager���擾�ł��܂���ł����B");
        return null;
    }

}
