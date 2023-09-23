using System.Collections;
using UnityEngine;

public class BattleEffect : MonoBehaviour
{
    [SerializeField, Header("攻撃エフェクト")] GameObject attackEffect;
    [SerializeField,Header("回復エフェクト")] GameObject healEffect;

    WaitForSeconds displayHealtime = new WaitForSeconds(2.3f); //回復エフェクトの表示時間

    /// <summary>
    /// 攻撃時のエフェクト表示
    /// </summary>
    /// <param name="dropPlace">カードのドロップ場所</param>
    public void Attack(GameObject dropPlace)
    {
        var dropPlacePos = dropPlace.transform.position;
        Debug.Log(dropPlacePos);
        // DropPlaceのlocalPositionを計算する
        var rect = dropPlace.GetComponent<RectTransform>();
        var localPoint = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, dropPlacePos, null, out localPoint);
        
        //ローカル座標にパーティクルを生成
        var effect = Instantiate(attackEffect, dropPlace.transform);
        effect.GetComponent<RectTransform>().localPosition = localPoint;
        effect.SetActive(true);
    }

    /// <summary>
    /// 回復のエフェクト表示
    /// </summary>
    /// <param name="healPlace">回復エフェクトの表示場所</param>
    public void Heal(GameObject healPlace)
    {
        var healDisplayPos = healPlace.transform.position;
        Debug.Log(healPlace);
        // HealPlaceのlocalPositionを計算する
        var rect = healPlace.GetComponent<RectTransform>();
        var localPoint = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, healDisplayPos, null, out localPoint);

        //ローカル座標にパーティクルを生成
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
