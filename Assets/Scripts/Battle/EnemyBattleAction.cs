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
    public Dictionary<int, int> hasEnemyRelics = new Dictionary< int, int>(); //�G�l�~�[�̏������Ă��郌���b�N
    const int maxRelics = 12;
    private EnemyAI enemyAI;
    RelicEffectList relicEffect; //�����b�N�̌���
    bool roundEnabled; //���E���h���Ɉ�x���������݂���
    public bool GetSetRoundEnabled { get => roundEnabled; set => roundEnabled = value; }
    private string debugMoveName = "����";
    void Awake()
    {
        InitializeRelics();
    }

    private void InitializeRelics()
    {
        for (int RelicID = 1; RelicID <= maxRelics; RelicID++)
        {
            hasEnemyRelics.Add(RelicID, 0);
        }
    }
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
    /// <summary>
    /// �G�l�~�[�̍s������
    /// </summary>
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
    /// <summary>
    /// �e�X�e�[�^�X���Z�b�g���鏈��
    /// </summary>
    /// <param name="floor">���݂̊K�w</param>
    /// <param name="enemyData">�I�����ꂽ�G�l�~�[�f�[�^</param>
    public void SetStatus(int floor, EnemyDataManager enemyData)
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
        enemyAI.SetEnemyState(floor, enemyData._enemyName);
    }
    /// <summary>
    /// �퓬�J�n���ɔ������郌���b�N����
    /// </summary>
    /// <param name="playerBattleAction">�v���C���[�̃X�e�[�^�X</param>
    /// <returns>�ύX���������v���C���[�̃X�e�[�^�X</returns>
    public PlayerBattleAction StartRelicEffect(PlayerBattleAction playerBattleAction)
    {
        relicEffect = GetComponent<RelicEffectList>();
        var ps = playerBattleAction;
        var relicEffectID2 = relicEffect.RelicID2(hasEnemyRelics[2], ps.GetSetCondition.upStrength, GetSetCondition.upStrength);
        GetSetCondition.upStrength = relicEffectID2.enemyUpStrength;
        ps.GetSetCondition.upStrength = relicEffectID2.playerUpStrength;
        GetSetConstAP = relicEffect.RelicID3(hasEnemyRelics[3], GetSetConstAP, GetSetChargeAP).constAP;
        ps.GetSetCondition.burn = relicEffect.RelicID6(hasEnemyRelics[6], ps.GetSetCondition.burn);
        GetSetGP = relicEffect.RelicID8(hasEnemyRelics[8], GetSetGP);
        Debug.Log("�X�^�[�g���̃����b�N���Ăяo����܂���: " + GetSetConstAP + " to " + GetSetChargeAP);
        return ps;
    }
    /// <summary>
    /// ���E���h�I�����Ɉ�x�����������郌���b�N����
    /// </summary>
    public void OnceEndRoundRelicEffect()
    {
        GetSetChargeAP = relicEffect.RelicID3(hasEnemyRelics[3], GetSetConstAP, GetSetChargeAP).chargeAP;
    }
    /// <summary>
    /// ���E���h�I�����ɔ������郌���b�N����
    /// </summary>
    public void EndRoundRelicEffect()
    {
        GetSetCondition = relicEffect.RelicID11(hasEnemyRelics[11], GetSetCondition);
    }
}
