using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerSceneLoader : MonoBehaviour
{
    private static bool Loaded { get; set; }

    UIManager uiManager;

    void Awake()
    {
        if (Loaded) return;

        Loaded = true;
        SceneManager.LoadScene("ManagerScene", LoadSceneMode.Additive);

        switch (gameObject.name)
        {
            case "TitleSceneManager":
                GetUIManager("Title");
                break;
            case "CharacterSceneManager":
                GetUIManager("Chara");
                break;
            default:
                GetUIManager("OverlayOnly");
                break;
        }

    }

    private void GetUIManager(string uiType)
    {
        //���[�h�ς݂̃V�[���ł���΁A���O�ŕʃV�[�����擾�ł���
        Scene scene = SceneManager.GetSceneByName("ManagerScene");

        //GetRootGameObjects�ŁA���̃V�[���̃��[�gGameObjects
        //�܂�A�q�G�����L�[�̍ŏ�ʂ̃I�u�W�F�N�g���擾�ł���
        foreach (var rootGameObject in scene.GetRootGameObjects())
        {
            //Debug.Log(rootGameObject.name);
            UIManager uiManager = rootGameObject.GetComponent<UIManager>();
            if (uiManager != null)
            {
                uiManager.ChangeUI(uiType);
                break;
            }
        }
    }
}