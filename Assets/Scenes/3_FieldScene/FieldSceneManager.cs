using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class FieldSceneManager : MonoBehaviour
{
    [SerializeField]
    private SceneFader sceneFader;

    private void Start()
    {
        // BGM�𗬂��܂�
        if (Random.Range(0, 2) == 0)
        {
            AudioManager.Instance.PlayBGM("Field1");
        }
        else
        {
            AudioManager.Instance.PlayBGM("Field2");
        }
    }

    public void LoadBattleScene()
    {
        // �o�g���V�[�������[�h
        sceneFader.SceneChange("BattleScene");
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
