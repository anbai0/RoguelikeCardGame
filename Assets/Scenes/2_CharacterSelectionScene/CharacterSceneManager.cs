using UnityEngine;

public class CharacterSceneManager : MonoBehaviour
{
    string playerInput = "";
    bool isDebug = false;
    [SerializeField] UIManagerCharaSelect uiManager;

    void Update()
    {
        //�^�C�g����ʂ֑J��
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // �L�����I���V�[�����A�����[�h���A�^�C�g���V�[�������[�h
            SceneFader.Instance.SceneChange("TitleScene", "CharacterSelectionScene");
        }


        // �B���R�}���h
        if (Input.inputString.Length > 0 && !uiManager.isClick)
        {
            playerInput += Input.inputString;

            if (playerInput.Length > 5) playerInput = "";

            // "001"�����͂��ꂽ���ǂ������`�F�b�N
            if (playerInput.Contains("001") && !isDebug)
            {
                isDebug = true;
                Debug.Log("001�����͂���܂����I");

                GameManager.Instance.ReadPlayer("DebugChan");

                uiManager.ShowLottery();
                uiManager.UIEventsReload();
                uiManager.lotteryScreen.SetActive(true);

                // ���͂����Z�b�g
                playerInput = "";
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.Instance.PlaySE("guard1");         
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            AudioManager.Instance.PlaySE("guard2");
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            AudioManager.Instance.PlaySE("guard3");
        }
    }

    public void LoadFieldScene()
    {
        // �L�����I���V�[�����A�����[�h���A�t�B�[���h�V�[�������[�h
        SceneFader.Instance.SceneChange("FieldScene", "CharacterSelectionScene",true);
    }
}
