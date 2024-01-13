using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// �G�l�~�[�̍s�����܂Ƃ߂��X�N���v�g
/// </summary>
public class EnemyBattleAction : CharacterBattleAction
{
    [SerializeField, Header("�G�l�~�[�摜")] Image enemyImage;
    [SerializeField, Header("�G�l�~�[HP�e�L�X�g")] Text enemyHPText;
    [SerializeField, Header("�G�l�~�[HP�X���C�_�[")] Slider enemyHPSlider;
    [SerializeField, Header("�G�l�~�[AP�e�L�X�g")] Text enemyAPText;
    [SerializeField, Header("�G�l�~�[GP�e�L�X�g")] Text enemyGPText;
    [SerializeField, Header("�_���[�W�\���I�u�W�F�N�g")] GameObject damageUI;
    [SerializeField, Header("�񕜕\���I�u�W�F�N�g")] GameObject healingUI;
    [SerializeField, Header("�K�[�h�\���I�u�W�F�N�g")] GameObject gardUI;
    [SerializeField, Header("�_���[�W�Ɖ񕜕\���̏o���ꏊ")] GameObject damageOrHealingPos;
    [SerializeField, Header("��Ԉُ�̃A�C�R���\���X�N���v�g")] EnemyConditionDisplay enemyConditionDisplay;
    [SerializeField, Header("�R�w�ڂ̓G���������b�N")] RelicController enemyRelic;
    [SerializeField, Header("�����b�N�̕\���ʒu")] Transform enemyRelicPlace;

    [SerializeField] EnemyAI enemyAI;
    [SerializeField] FlashImage flash;

    public Dictionary<int, int> hasEnemyRelics = new Dictionary< int, int>(); //�G�l�~�[�̏������Ă��郌���b�N
    const int maxRelics = 12;
    RelicEffectList relicEffect; //�����b�N�̌���

    public Dictionary<string, int> enemyCondition = new Dictionary<string, int>(); //�G�l�~�[�ɕt�^����Ă����Ԉُ�

    int dropMoney = 0;
    public int GetSetDropMoney { get => dropMoney; set => dropMoney = value; } //�G�l�~�[�����Ƃ��R�C���̖���

    bool roundEnabled; //���E���h���Ɉ�x���������݂���
    public bool GetSetRoundEnabled { get => roundEnabled; set => roundEnabled = value; }
    
    BattleGameManager bg;

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
        var selectMove = enemyAI.SelectMove(GetSetCurrentAP);
        string moveName = selectMove.moveName;
        if (moveName == "RoundEnd") //EnemyAI�őI�����ꂽ�s�����s���I���̏ꍇ
        {
            TurnEnd();
            bg.TurnCalc();
            yield break;
        }
        yield return new WaitForSeconds(1.0f);
        flash.StartFlash(Color.white, 0.1f);
        yield return new WaitForSeconds(0.2f);
        flash.StartFlash(Color.white, 0.1f);
        yield return new WaitForSeconds(0.2f);
        int moveCost = selectMove.moveCost;
        GetSetCurrentAP -= moveCost;
        enemyAI.ActionMove(moveName);
        yield return StartCoroutine(MoveAfterCondition());
        bg.isEnemyMoving = false;
        bg.TurnCalc();
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
        enemyImage.sprite = enemyData._enemyImage;
        enemyHPSlider.value = 1;
        GetSetHP = enemyData._enemyHP;
        GetSetCurrentHP = GetSetHP;
        GetSetConstAP = enemyData._enemyAP;
        GetSetAP = enemyData._enemyAP;
        GetSetCurrentAP = GetSetAP;
        GetSetGP = enemyData._enemyGP;
        dropMoney = enemyData._dropMoney;
        InitializedCondition();
        GetSetCondition = enemyCondition;
        GetSetInflictCondition = GetComponent<InflictCondition>();
        enemyAI.SetEnemyState(floor, enemyData._enemyName);
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

    // �������Ă��郌���b�N�̕\��
    public void ViewEnemyRelic(GameManager gm)
    {
        for (int RelicID = 1; RelicID <= maxRelics; RelicID++)
        {
            //�������Ɏw�肵��RelicID�̃L�[�����݂��邩�ǂ����ƃ����b�N���P�ȏ㏊�����Ă��邩
            if (hasEnemyRelics.ContainsKey(RelicID) && hasEnemyRelics[RelicID] >= 1)
            {
                RelicController relic = Instantiate(enemyRelic, enemyRelicPlace);
                relic.Init(RelicID);                                               // �擾����RelicController��Init���\�b�h���g�������b�N�̐����ƕ\��������

                relic.transform.GetChild(4).gameObject.SetActive(true);
                relic.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = hasEnemyRelics[RelicID].ToString();      // Prefab�̎q�I�u�W�F�N�g�ł��鏊������\������e�L�X�g��ύX

                relic.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = gm.relicDataList[RelicID]._relicName.ToString();        // �����b�N�̖��O��ύX
                relic.transform.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>().text = gm.relicDataList[RelicID]._relicEffect.ToString();      // �����b�N�����ύX

            }
        }
    }

    /// <summary>
    /// �_���[�W���󂯂��Ƃ��̏���
    /// </summary>
    /// <param name="damage">�󂯂��_���[�W</param>
    public void TakeDamage(int damage)
    {
        //if (damage <= 0) return; //�_���[�W��0�ȉ��������ꍇ���̏������񂳂Ȃ�

        int deductedDamage = 0;
        if (GetSetGP > 0) //�K�[�h�|�C���g����������
        {
            //�K�[�h�|�C���g�̕������_���[�W���y������
            deductedDamage = damage - GetSetGP;
            deductedDamage = deductedDamage < 0 ? 0 : deductedDamage;
            GetSetGP -= damage;
            ViewGard();
        }
        else
        {
            deductedDamage = damage;
        }
        GetSetCurrentHP -= deductedDamage;

        if (deductedDamage >= 0)
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
    /// �K�[�h�������Ƃ�`����e�L�X�g��\�����鏈��
    /// </summary>
    void ViewGard()
    {
        // SE
        int rand = Random.Range(0,3);
        switch (rand)
        {
            case 0:
                AudioManager.Instance.PlaySE("guard1");
                break;
            case 1:
                AudioManager.Instance.PlaySE("guard2");
                break;
            case 2:
                AudioManager.Instance.PlaySE("guard3");
                break;
            default:
                break;
        }       

        var gardPos = damageOrHealingPos.transform.position + new Vector3(0f, 120f, 0f);
        GameObject gardObj = Instantiate(gardUI, gardPos, Quaternion.identity, damageOrHealingPos.transform);
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
        enemyConditionDisplay.UpdateConditionIcon(enemyCondition);
    }

    //�ȉ��ATakeDamage��HealingHP�Ɋ֌W�����Ԉُ�̌Ăяo��

    /// <summary>
    /// �����񕜂̌Ăяo��
    /// </summary>
    public void AutoHealing()
    {
        if (enemyCondition["AutoHealing"] > 0)
        {
            HealingHP(GetSetInflictCondition.AutoHealing(enemyCondition["AutoHealing"]));
        }
    }

    /// <summary>
    /// �Ώ��̌Ăяo��
    /// </summary>
    public void Burn()
    {
        if (enemyCondition["Burn"] > 0) 
        {
            TakeDamage(enemyCondition["Burn"]);
        }
    }

    /// <summary>
    /// �דł̌Ăяo��
    /// </summary>
    /// <param name="moveCount">�s����</param>
    public void Poison(int moveCount)
    {
        if (enemyCondition["Poison"] > 0)
        {
            var poison = GetSetInflictCondition.Poison(enemyCondition["Poison"], moveCount);
            TakeDamage(poison);
        }
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
        relicEffect.RelicID2(hasEnemyRelics[2]);
        GetSetConstAP = relicEffect.RelicID3(hasEnemyRelics[3], GetSetConstAP, GetSetChargeAP).constAP;
        ps.AddConditionStatus("Burn", relicEffect.RelicID6(hasEnemyRelics[6]));
        GetSetGP = relicEffect.RelicID8(hasEnemyRelics[8], GetSetGP);
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
        yield return new WaitForSeconds(1.2f);
        flash.StartFlash(Color.white, 0.5f);
        yield return new WaitForSeconds(1.2f);
        flash.StartFlash(Color.white, 1.0f);
        yield return new WaitForSeconds(1.0f);
        Destroy(enemyImage);
    }
}
