using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBattleAction : MonoBehaviour
{
    [Header("�G�l�~�[���e�L�X�g")]
    [SerializeField] Text enemyNameText;
    [Header("�G�l�~�[�摜")]
    [SerializeField] Image enemyImage;
    [Header("�G�l�~�[HP�e�L�X�g")]
    [SerializeField] Text enemyHPText;
    [Header("�G�l�~�[HP�X���C�_�[")]
    [SerializeField] Slider enemyHPSlider;
    [SerializeField] Text moveText;
    private int enemyHP;//�ő��HP
    private int enemyCurrentHP;//���݂�HP
    private int enemyConstAP;//�Q�[���J�n���̍ő�AP
    private int roundStartAP;//���E���h�J�n���̍ő�AP
    private int enemyAP;//�ő�AP
    private int enemyCurrentAP;//���݂�AP
    private int enemyGP;//�K�[�h�|�C���g
    private int enemyChargeAP;//���E���h�o�߂ŏ㏸���Ă���AP�̒l
    private ConditionStatus enemyCondition;//�G�l�~�[�̏�Ԉُ�X�e�[�^�X
    private InflictCondition inflictCondition;//��Ԉُ�̌��ʃX�N���v�g
    private EnemyAI enemyAI;
    bool roundEnabled;
    private string debugMoveName = "����";
    public int GetSetEnemyHP { get => enemyHP; set => enemyHP = value; }
    public int GetSetEnemyCurrentHP { get => enemyCurrentHP; set => enemyCurrentHP = value; }
    public int GetSetEnemyConstAP { get => enemyConstAP; set => enemyConstAP = value; }
    public int GetSetEnemyAP { get => enemyAP; set => enemyAP = value; }
    public int GetSetEnemyCurrentAP { get => enemyCurrentAP; set => enemyCurrentAP = value; }
    public int GetSetEnemyChargeAP { get => enemyChargeAP; set => enemyChargeAP = value; }
    public int GetSetEnemyGP { get => enemyGP; set => enemyGP = value; }
    public bool GetSetRoundEnabled { get => roundEnabled; set => roundEnabled = value; }
    public ConditionStatus GetSetEnemyCondition { get => enemyCondition; set => enemyCondition = value; }
    // Start is called before the first frame update
    void Start()
    {
        enemyChargeAP = 0;
        roundEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText();
    }
    public void SetUpAP()//���E���h�J�n����AP�v�Z
    {
        var curse = inflictCondition.Curse(enemyConstAP, enemyChargeAP, enemyCondition.curse, enemyCondition.invalidBadStatus);
        enemyAP = curse.nextRoundAP;
        enemyCurrentAP = enemyAP;
        enemyCondition.invalidBadStatus = curse.invalidBadStatus;
        debugMoveName = "����";
        roundEnabled = false;
    }
    public void Move()//�G�l�~�[�̌��ʏ���
    {
        var selectMove = enemyAI.SelectMove(enemyCurrentAP);
        string moveName = selectMove.moveName;
        debugMoveName = moveName;
        int moveCost = selectMove.moveCost;
        enemyCurrentAP -= moveCost;
        enemyAI.ActionMove(moveName);
        Debug.Log("�G�l�~�[�̌��݂�AP:" + enemyCurrentAP);
    }
    public void TurnEnd()//�G�l�~�[���s���s�\�ɂ���
    {
        enemyCurrentAP = 0;
    }
    public void SetStatus(EnemyDataManager enemyData)
    {
        enemyNameText.text = enemyData._enemyName;
        enemyImage.sprite = enemyData._enemyImage;
        enemyHPSlider.value = 1;
        enemyHP = enemyData._enemyHP;
        enemyCurrentHP = enemyHP;
        enemyConstAP = enemyData._enemyAP;
        enemyAP = enemyData._enemyAP;
        enemyCurrentAP = enemyAP;
        enemyGP = enemyData._enemyGP;
        enemyCondition = new ConditionStatus();
        inflictCondition = GetComponent<InflictCondition>();
        enemyAI = GetComponent<EnemyAI>();
        enemyAI.SetEnemyState(enemyData._enemyName);
    }
    public void UpdateText() //�e�e�L�X�g�̍X�V
    {
        if (enemyCurrentHP > enemyHP)
        {
            enemyCurrentHP = enemyHP;
        }
        if (enemyCurrentHP < 0)
        {
            enemyCurrentHP = 0;
        }
        enemyHPText.text = enemyCurrentHP + "/" + enemyHP;
        enemyHPSlider.value = enemyCurrentHP / (float)enemyHP;
        if (enemyCurrentAP > enemyAP)
        {
            enemyCurrentAP = enemyAP;
        }
        if (enemyCurrentAP < 0)
        {
            enemyCurrentAP = 0;
        }
        if (enemyGP < 0)
        {
            enemyGP = 0;
        }
        moveText.text = "���݂̋Z:" + debugMoveName;
    }
    public void ChargeAP() //���E���h�I������AP����
    {
        enemyChargeAP++;
    }
    public bool CheckHP() //�G�l�~�[������ł��Ȃ����`�F�b�N����
    {
        if (enemyCurrentHP <= 0)
        {
            return true;
        }
        return false;
    }
    public void TakeDamage(int damage) //�_���[�W���󂯂��Ƃ��̏���
    {
        int deductedDamage = 0;
        if (enemyGP > 0) //�G�l�~�[�ɃK�[�h�|�C���g����������
        {
            //�K�[�h�|�C���g�̕������_���[�W���y������
            deductedDamage = damage - enemyGP;
            if (deductedDamage < 0)
            {
                deductedDamage = 0;
            }
            enemyGP -= damage;
        }
        else
        {
            deductedDamage = damage;
        }
        enemyCurrentHP -= deductedDamage;
    }
    public void SaveAP() //���E���h�J�n����AP��ۑ����Ă���
    {
        roundStartAP = enemyAP;
    }
    public bool IsCurse() //���E���h�J�n���̍ő�AP�������݂̍ő�AP�����Ȃ��ꍇ�͎������
    {
        //�^�[���I�����̍ő�AP
        int currentMaxAP = inflictCondition.Curse(enemyConstAP, enemyChargeAP, enemyCondition.curse, enemyCondition.invalidBadStatus).nextRoundAP;
        return roundStartAP > currentMaxAP ? true : false;
    }
    //��Ԉُ�
    public int EnemyUpStrengh(int attackPower)
    {
        attackPower = inflictCondition.UpStrength(attackPower, enemyCondition.upStrength);
        return attackPower;
    }
    public void EnemyAutoHealing()
    {
        enemyCurrentHP = inflictCondition.AutoHealing(enemyCurrentHP, enemyCondition.autoHealing);
    }
    public void EnemyImpatience()
    {
        var impatience = inflictCondition.Impatience(enemyCurrentAP, enemyCondition.impatience, enemyCondition.invalidBadStatus);
        enemyCurrentAP = impatience.currentAP;
        enemyCondition.invalidBadStatus = impatience.invalidBadStatus;
    }
    public int EnemyWeakness(int attackPower)
    {
        var weakness = inflictCondition.Weakness(attackPower, enemyCondition.weakness, enemyCondition.invalidBadStatus);
        attackPower = weakness.attackPower;
        enemyCondition.invalidBadStatus = weakness.invalidBadStatus;
        return attackPower;
    }
    public void EnemyBurn()
    {
        var burn = inflictCondition.Burn(enemyCurrentHP, enemyCondition.burn, enemyCondition.invalidBadStatus);
        enemyCurrentHP = burn.currentHP;
        enemyCondition.invalidBadStatus = burn.invalidBadStatus;
    }
    public void EnemyPoison(int moveCount)
    {
        var poison = inflictCondition.Poison(enemyCurrentHP, enemyCondition.poison, enemyCondition.invalidBadStatus, moveCount);
        enemyCurrentHP = poison.currentHP;
        enemyCondition.invalidBadStatus = poison.invalidBadStatus;
    }
    //�J�[�h����
    public void HealingHP(int healingHPPower) //HP�̉�
    {
        enemyCurrentHP += healingHPPower;
    }
    public void HealingAP(int healingAPPower) //AP�̉�
    {
        enemyCurrentAP += healingAPPower;
    }
    public void AddGP(int addGP) //GP�̒ǉ�
    {
        enemyGP += addGP;
    }
    public void ReleaseBadStatus() //�o�b�h�X�e�[�^�X����������
    {
        enemyCondition.curse = 0;
        enemyCondition.impatience = 0;
        enemyCondition.weakness = 0;
        enemyCondition.burn = 0;
        enemyCondition.poison = 0;
    }
    public int CheckBadStatus() //�o�b�h�X�e�[�^�X�����邩�`�F�b�N����
    {
        var pc = enemyCondition;
        int badStatus = pc.curse + pc.impatience + pc.weakness + pc.burn + pc.poison;
        return badStatus;
    }
    public void AddConditionStatus(string status, int count) //��Ԉُ��ǉ�����
    {
        if (status == "UpStrength")
        {
            enemyCondition.upStrength += count;
        }
        else if (status == "AutoHealing")
        {
            enemyCondition.autoHealing += count;
        }
        else if (status == "InvalidBadStatus")
        {
            enemyCondition.invalidBadStatus += count;
        }
        else if (status == "Curse")
        {
            enemyCondition.curse += count;
        }
        else if (status == "Impatience")
        {
            enemyCondition.impatience += count;
        }
        else if (status == "Weakness")
        {
            enemyCondition.weakness += count;
        }
        else if (status == "Burn")
        {
            enemyCondition.burn += count;
        }
        else if (status == "Poison")
        {
            enemyCondition.poison += count;
        }
    }
}
