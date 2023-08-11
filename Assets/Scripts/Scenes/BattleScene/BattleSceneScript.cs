using SelfMadeNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BattleSceneScript : MonoBehaviour
{
    [SerializeField]
    private SceneFader sceneFader;

    public void SwitchSceneAfterVictory()
    {
        // �o�g���V�[�����A�����[�h
        sceneFader.SceneChange(unLoadSceneName: "BattleScene");

        PlayerController.isPlayerActive = true;     // �v���C���[�𓮂���悤�ɂ���
    }

    public void SwitchSceneAfterLose()
    {
        // �t�B�[���h�V�[�����A�����[�h
        sceneFader.UnLoadScene("FieldScene");

        // �V���b�v�V�[�����A�����[�h
        sceneFader.UnLoadScene("ShopScene");

        // �o�g���V�[�����A�����[�h�A���U���g�V�[�������[�h
        sceneFader.SceneChange("ResultScene", "BattleScene");

        PlayerController.isPlayerActive = true;
    }
}
