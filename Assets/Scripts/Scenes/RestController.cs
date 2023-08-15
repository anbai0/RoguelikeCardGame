using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// �x�e�Ɋւ��鏈��������X�N���v�g�ł��B
/// </summary>
public class RestController : MonoBehaviour
{
    private GameManager gm;
    [SerializeField] ManagerSceneLoader msLoader;

    private const int restPrice = 70;          // �x�e�̒l�i
    [Header("�Q�Ƃ���UI")]
    [SerializeField] GameObject restButton;
    [SerializeField] TextMeshProUGUI restText;
    [Header ("ShopScene�̎������g��")]
    [SerializeField] TextMeshProUGUI restPriceText;

    private void Start()
    {
        // GameManager�擾(�ϐ����ȗ�)
        gm = GameManager.Instance;
    }


    /// <summary>
    /// �x�e�ł��邩���肷��̂Ƃ���������Ȃ��ꍇ�A
    /// �e�L�X�g�̐F��ς�����{�^�����O���[�A�E�g�ɂ��܂�
    /// </summary>
    /// <param name="sceneType">�Ăяo�����̃V�[����</param>
    /// <returns>�x�e�ł���ł���ꍇtrue</returns>
    public bool CheckRest(string sceneType)
    {
        if (sceneType == "ShopScene")                              // ShopScene����Ă΂ꂽ�Ƃ������l�i�`�F�b�N����
        {
            if (gm.playerData._playerMoney < restPrice)          // ����������Ȃ��ꍇ
            {
                restPriceText.color = Color.red;              // �l�i��Ԃ��\��
            }

            // ����HP��Max�̏ꍇ�܂��͂���������Ȃ��ꍇ
            if (gm.playerData._playerHP == gm.playerData._playerCurrentHP || gm.playerData._playerMoney < restPrice)
            {
                restButton.GetComponent<Image>().color = Color.gray;  // �x�e�{�^�����O���[�A�E�g
                return false;
            }
        }

        if (sceneType == "BonfireScene")                                   // BonfireScene����Ă΂ꂽ�Ƃ������l�i�`�F�b�N����
        {
            // ����HP��Max�̏ꍇ�܂��͂���������Ȃ��ꍇ
            if (gm.playerData._playerHP == gm.playerData._playerCurrentHP)
            {
                restButton.GetComponent<Image>().color = Color.gray;  // �x�e�{�^�����O���[�A�E�g
                return false;
            }
        }

        return true;

    }

    /// <summary>
    /// �x�e��ʂŃe�L�X�g��\�������郁�\�b�h�ł�
    /// </summary>
    /// <param name="sceneType">�Ăяo�����̃V�[����</param>
    public void ChengeRestText(string sceneType)
    {
        if (sceneType == "ShopScene")
            restText.text = $"{restPrice}G�������\n�̗͂�{gm.playerData._playerHP - gm.playerData._playerCurrentHP}�񕜂��܂����H";

        if (sceneType == "BonfireScene")
            restText.text = $"���΂��g�p����\n�̗͂�{gm.playerData._playerHP - gm.playerData._playerCurrentHP}�񕜂��܂����H";
    }

    /// <summary>
    /// �x�e�̏���
    /// �x�e�ɕK�v�ȋ��z���x�����A
    /// �̗͂�S�񕜂����܂��B
    /// </summary>
    /// <param name="sceneType">�Ăяo�����̃V�[����</param>
    public void Rest(string sceneType)
    {
        if (sceneType == "ShopScene") { gm.playerData._playerMoney -= restPrice; }         // �V���b�v�V�[������Ă΂ꂽ�Ƃ����������𕥂�
        
        gm.playerData._playerCurrentHP = gm.playerData._playerHP;
    }
}
