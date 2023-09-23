using System.Collections;
using UnityEngine;

public class BattleEffect : MonoBehaviour
{
    [SerializeField, Header("�U���G�t�F�N�g")] GameObject attackEffect;
    [SerializeField,Header("�񕜃G�t�F�N�g")] GameObject healEffect;

    WaitForSeconds displayHealtime = new WaitForSeconds(2.3f); //�񕜃G�t�F�N�g�̕\������

    /// <summary>
    /// �U�����̃G�t�F�N�g�\��
    /// </summary>
    /// <param name="dropPlace">�J�[�h�̃h���b�v�ꏊ</param>
    public void Attack(GameObject dropPlace)
    {
        var dropPlacePos = dropPlace.transform.position;
        Debug.Log(dropPlacePos);
        // DropPlace��localPosition���v�Z����
        var rect = dropPlace.GetComponent<RectTransform>();
        var localPoint = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, dropPlacePos, null, out localPoint);
        
        //���[�J�����W�Ƀp�[�e�B�N���𐶐�
        var effect = Instantiate(attackEffect, dropPlace.transform);
        effect.GetComponent<RectTransform>().localPosition = localPoint;
        effect.SetActive(true);
    }

    /// <summary>
    /// �񕜂̃G�t�F�N�g�\��
    /// </summary>
    /// <param name="healPlace">�񕜃G�t�F�N�g�̕\���ꏊ</param>
    public void Heal(GameObject healPlace)
    {
        var healDisplayPos = healPlace.transform.position;
        Debug.Log(healPlace);
        // HealPlace��localPosition���v�Z����
        var rect = healPlace.GetComponent<RectTransform>();
        var localPoint = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, healDisplayPos, null, out localPoint);

        //���[�J�����W�Ƀp�[�e�B�N���𐶐�
        var effect = Instantiate(healEffect, healPlace.transform);
        effect.GetComponent<RectTransform>().localPosition = localPoint;
        effect.SetActive(true);
        StartCoroutine(CoDestroyHealObj(effect));
    }

    IEnumerator CoDestroyHealObj(GameObject healObj)
    {
        yield return displayHealtime;
        Destroy(healObj);
    }
}
