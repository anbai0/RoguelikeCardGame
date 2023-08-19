using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FieldSceneManager : MonoBehaviour
{
    [SerializeField]
    private SceneFader sceneFader;

    string sceneNameToCheck = "ShopScene";

    public void LoadBattleScene(string enemyType)
    {
        if(enemyType == "StrongEnemy")
        {
            StartCoroutine(WaitUnLoadFieldScene());
        }
        else
        {
            // �퓬�V�[�������[�h
            sceneFader.SceneChange("BattleScene");
        }
    }

    IEnumerator WaitUnLoadFieldScene()
    {
        // �o�g���V�[�������[�h
        sceneFader.SceneChange("BattleScene");

        yield return new WaitForSeconds(1.0f);

        //�t�B�[���h�V�[�����A�����[�h
        sceneFader.SceneChange(unLoadSceneName: "FieldScene");

        if (SceneUtility.GetBuildIndexByScenePath("ShopScene") != -1) //�V���b�v�V�[�������[�h����Ă����ꍇ
        {
            //�V���b�v�V�[�����A�����[�h
            sceneFader.SceneChange(unLoadSceneName: "ShopScene");
        }
    }

    public void LoadBonfireScene()
    {
        // ���΃V�[�������[�h
        sceneFader.SceneChange("BonfireScene");
    }

    public void LoadTreasureBoxScene()
    {
        // �󔠃V�[�������[�h
        sceneFader.SceneChange("TreasureBoxScene");
    }

    public void LoadShopScene()
    {
        // �V���b�v�V�[�������[�h
        sceneFader.SceneChange("ShopScene");
    }

    public void ActivateShopScene()
    {
        // �V���b�v�V�[���̃I�u�W�F�N�g��\��
        sceneFader.ToggleSceneWithFade("ShopScene", true);
    }
}
