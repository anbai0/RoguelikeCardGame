using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    [Header("�_���[�W�\���I�u�W�F�N�g")]
    [SerializeField]
    GameObject damageUI;
    [Header("�񕜕\���I�u�W�F�N�g")]
    [SerializeField]
    GameObject healingUI;
    [Header("�_���[�W�Ɖ񕜕\���̏o���ꏊ")]
    [SerializeField]
    GameObject damageOrHealingPos;
    [Header("��Ԉُ�̃A�C�R���\���X�N���v�g")]
    [SerializeField]
    EnemyConditionDisplay enemyConditionDisplay;
    

    public Dictionary<int, int> hasEnemyRelics = new Dictionary< int, int>(); //�G�l�~�[�̏������Ă��郌���b�N
    const int maxRelics = 12;
    RelicEffectList relicEffect; //�����b�N�̌���

    public Dictionary<string, int> enemyCondition = new Dictionary<string, int>(); //�G�l�~�[�ɕt�^����Ă����Ԉُ�

    int dropMoney = 0;
    public int GetSetDropMoney { get => dropMoney; set => dropMoney = value; } //�G�l�~�[�����Ƃ��R�C���̖���

    EnemyAI enemyAI; //�G�̍s���X�N���v�g

    FlashImage flash; //�G�s�����̉��o

    BattleGameManager bg;

    bool roundEnabled; //���E���h���Ɉ�x���������݂���
    public bool GetSetRoundEnabled { get => roundEnabled; set => roundEnabled = value; }
    
    string debugMoveName = "����";

    void Awake()
    {
        InitializeRelics();
    }

    /// <summary>
    /// �G�l�~�[�̃����b�N��ID�Ə�����������������
    /// </summary>
    private void InitializeRelics()
    {
        for (int RelicID = 1; RelicID <= maxRelics; RelicID++)
        {
            hasEnemyRelics.Add(RelicID, 0);
        }
    }

    void Start()
    {
        bg = BattleGameManager.Instance;
        roundEnabled = false;
    }

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
        StartCoroutine(MoveFlash());
        
    }
    IEnumerator MoveFlash()
    {
        bg.isEnemyMoving = true;
        yield return new WaitForSeconds(0.5f);
        flash.StartFlash(Color.white, 0.1f);
        yield return new WaitForSeconds(0.2f);
        flash.StartFlash(Color.white, 0.1f);
        yield return new WaitForSeconds(0.1f);
        var selectMove = enemyAI.SelectMove(GetSetCurrentAP);
        string moveName = selectMove.moveName;
        debugMoveName = moveName;
        int moveCost = selectMove.moveCost;
        GetSetCurrentAP -= moveCost;
        enemyAI.ActionMove(moveName);
        Debug.Log("�G�l�~�[�̌��݂�AP:" + GetSetCurrentAP);
        yield return StartCoroutine(MoveAfterCondition());
        bg.isEnemyMoving = false;
        yield break;
    }

    IEnumerator MoveAfterCondition()
    {
        AutoHealing();
        Impatience();
        Burn();
        yield return null;
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
        dropMoney = enemyData._dropMoney;
        //enemyCondition = GetCondition;
        InitializedCondition();
        GetSetCondition = enemyCondition;
        GetSetInflictCondition = GetComponent<InflictCondition>();
        enemyAI = GetComponent<EnemyAI>();
        enemyAI.SetEnemyState(floor, enemyData._enemyName);
        flash = GetComponent<FlashImage>();
    }

    /// <summary>
    /// ��Ԉُ�̖��O�Ə����������������Ă�������
    /// </summary>
    void InitializedCondition()
    {
        enemyCondition.Add("UpStrength", 0);
        enemyCondition.Add("AutoHealing", 0);
        enemyCondition.Add("InvalidBadStatus", 0);
        enemyCondition.Add("Curse", 0);
        enemyCondition.Add("Impatience", 0);
        enemyCondition.Add("Weakness", 0);
        enemyCondition.Add("Burn", 0);
        enemyCondition.Add("Poison", 0);
    }

    /// <summary>
    /// �_���[�W���󂯂��Ƃ��̏���
    /// </summary>
    /// <param name="damage">�󂯂��_���[�W</param>
    public void TakeDamage(int damage)
    {
        int deductedDamage = 0;
        if (GetSetGP > 0) //�K�[�h�|�C���g����������
        {
            //�K�[�h�|�C���g�̕������_���[�W���y������
            deductedDamage = damage - GetSetGP;
            deductedDamage = deductedDamage < 0 ? 0 : deductedDamage;
            GetSetGP -= damage;
        }
        else
        {
            deductedDamage = damage;
        }
        GetSetCurrentHP -= deductedDamage;

        if (deductedDamage > 0)
        {
            AudioManager.Instance.PlaySE("�U��1");
            StartCoroutine(DamageFlash(deductedDamage));
        }
    }
    IEnumerator DamageFlash(int deductedDamage)
    {
        flash.StartFlash(Color.white, 0.1f);
        yield return new WaitForSeconds(0.1f);
        ViewDamage(deductedDamage);
        yield break;
    }

    /// <summary>
    /// �_���[�W���o��\�����鏈��
    /// </summary>
    /// <param name="damage">�󂯂��_���[�W</param>
    private void ViewDamage(int damage)
    {
        GameObject damageObj = Instantiate(damageUI, damageOrHealingPos.transform);
        //float rndX = Random.Range(-50, 50);
        //float rndY = Random.Range(-50, 50);
        //damageObj.transform.position += new Vector3(rndX, rndY, 0);
        damageObj.GetComponent<TextMeshProUGUI>().text = damage.ToString();
    }

    /// <summary>
    /// HP�񕜂̏���
    /// </summary>
    /// <param name="healingHPPower">HP�̉񕜗�</param>
    public void HealingHP(int healingHPPower) //HP�̉�
    {
        GetSetCurrentHP += healingHPPower;
        if (healingHPPower > 0)
        {
            AudioManager.Instance.PlaySE("��");
            ViewHealing(healingHPPower);
        }
    }

    /// <summary>
    /// �񕜉��o��\�����鏈��
    /// </summary>
    /// <param name="_healingHPPower">HP�̉񕜗�</param>
    void ViewHealing(int _healingHPPower)
    {
        GameObject healingObj = Instantiate(healingUI, damageOrHealingPos.transform);
        healingObj.GetComponent<TextMeshProUGUI>().text = _healingHPPower.ToString();
    }

    /// <summary>
    /// ��Ԉُ�̃A�C�R����\�����鏈��
    /// </summary>
    public void ViewConditionIcon()
    {
        enemyConditionDisplay.ViewIcon(enemyCondition);
    }

    //�ȉ��ATakeDamage��HealingHP�Ɋ֌W�����Ԉُ�̌Ăяo��

    /// <summary>
    /// �����񕜂̌Ăяo��
    /// </summary>
    public void AutoHealing()
    {
        HealingHP(GetSetInflictCondition.AutoHealing(enemyCondition["AutoHealing"]));
    }

    /// <summary>
    /// �Ώ��̌Ăяo��
    /// </summary>
    public void Burn()
    {
        var burn = GetSetInflictCondition.Burn(enemyCondition["Burn"], enemyCondition["InvalidBadStatus"]);
        TakeDamage(burn.damage);
        enemyCondition["InvalidBadStatus"] = burn.invalidBadStatus;
    }

    /// <summary>
    /// �דł̌Ăяo��
    /// </summary>
    /// <param name="moveCount">�s����</param>
    public void Poison(int moveCount)
    {
        var poison = GetSetInflictCondition.Poison(enemyCondition["Poison"], enemyCondition["InvalidBadStatus"], moveCount);
        TakeDamage(poison.damage);
        enemyCondition["InvalidBadStatus"] = poison.invalidBadStatus;
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
        var relicEffectID2 = relicEffect.RelicID2(hasEnemyRelics[2], ps.playerCondition["UpStrength"], enemyCondition["UpStrength"]);
        enemyCondition["UpStrength"] = relicEffectID2.enemyUpStrength;
        ps.playerCondition["UpStrength"] = relicEffectID2.playerUpStrength;
        GetSetConstAP = relicEffect.RelicID3(hasEnemyRelics[3], GetSetConstAP, GetSetChargeAP).constAP;
        ps.playerCondition["Burn"] = relicEffect.RelicID6(hasEnemyRelics[6], ps.playerCondition["Burn"]);
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
        enemyCondition = relicEffect.RelicID11(hasEnemyRelics[11], enemyCondition);
    }

    /// <summary>
    /// �������Ƃ��̃A�j���[�V����
    /// </summary>
    public void EnemyDefeated()
    {
        StartCoroutine(EnemyDefeatedAnimation());
    }
    IEnumerator EnemyDefeatedAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        flash.StartFlash(Color.white, 0.5f);
        yield return new WaitForSeconds(1.0f);
        flash.StartFlash(Color.white, 0.5f);
        yield return new WaitForSeconds(1.0f);
        flash.StartFlash(Color.white, 1.0f);
        yield return new WaitForSeconds(1.0f);
        Destroy(enemyImage);
    }
}
