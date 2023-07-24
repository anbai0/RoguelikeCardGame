using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBattleAction : CharacterBattleAction
{
    [Header("�G�l�~�[���e�L�X�g")]
    [SerializeField] Text enemyNameText;
    [Header("�G�l�~�[�摜")]
    [SerializeField] Image enemyImage;
    [Header("�G�l�~�[HP�e�L�X�g")]
    [SerializeField] Text enemyHPText;
    [Header("�G�l�~�[HP�X���C�_�[")]
    [SerializeField] Slider enemyHPSlider;
    [Header("�G�l�~�[AP�e�L�X�g")]
    [SerializeField] Text enemyAPText;
    [Header("�G�l�~�[GP�e�L�X�g")]
    [SerializeField] Text enemyGPText;
    [Header("�Z�̖��O")]
    [SerializeField] Text moveText;
    private EnemyAI enemyAI;
    bool roundEnabled; //���E���h���Ɉ�x���������݂���
    public bool GetSetRoundEnabled { get => roundEnabled; set => roundEnabled = value; }
    private string debugMoveName = "����";
    // Start is called before the first frame update
    void Start()
    {
        roundEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText(enemyHPText, enemyAPText, enemyGPText, enemyHPSlider);
        moveText.text = "���݂̋Z:" + debugMoveName;
    }
    public void Move()//�G�l�~�[�̌��ʏ���
    {
        var selectMove = enemyAI.SelectMove(GetSetCurrentAP);
        string moveName = selectMove.moveName;
        debugMoveName = moveName;
        int moveCost = selectMove.moveCost;
        GetSetCurrentAP -= moveCost;
        enemyAI.ActionMove(moveName);
        Debug.Log("�G�l�~�[�̌��݂�AP:" + GetSetCurrentAP);
    }
    public void SetStatus(EnemyDataManager enemyData)
    {
        enemyNameText.text = enemyData._enemyName;
        enemyImage.sprite = enemyData._enemyImage;
        enemyHPSlider.value = 1;
        GetSetHP = enemyData._enemyHP;
        GetSetCurrentHP = GetSetHP;
        GetSetConstAP = enemyData._enemyAP;
        GetSetAP = enemyData._enemyAP;
        GetSetCurrentAP = GetSetAP;
        GetSetGP = enemyData._enemyGP;
        GetSetCondition = new ConditionStatus();
        GetSetInflictCondition = GetComponent<InflictCondition>();
        enemyAI = GetComponent<EnemyAI>();
        enemyAI.SetEnemyState(enemyData._enemyName);
    }
}
