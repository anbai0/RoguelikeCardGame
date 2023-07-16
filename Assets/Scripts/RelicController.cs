using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicController : MonoBehaviour
{
    public RelicViewManager relicViewManager;// �����b�N�̌����ڂ̏���
    public RelicDataManager relicDataManager;// �����b�N�̃f�[�^������

    private void Awake()
    {
        relicViewManager = GetComponent<RelicViewManager>();
    }

    public void Init(int relicID)// �����b�N�𐶐��������ɌĂ΂��֐�
    {
        relicDataManager = new RelicDataManager(relicID);// �����b�N�f�[�^�𐶐�
        relicViewManager.ViewRelic(relicDataManager);// �����b�N�f�[�^�̕\��
    }
}
