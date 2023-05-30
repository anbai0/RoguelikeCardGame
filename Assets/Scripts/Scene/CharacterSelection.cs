//CharacterSelection
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    private Image image;
    private Color32 originalColor;
    private Color32 targetColor;

    Vector3 Scale = new Vector3(0.01f, 0.01f, 0.01f);

    bool isHighlight = false;

    private float duration = 0.25f;      //色が変わるまでの秒数
    private float elapsedTime = 0f;
    private float late;

    private void Start()
    {
        // 画像のImageコンポーネントを取得
        image = this.gameObject.GetComponent<Image>();

        // 初期の色を保存
        originalColor = image.color;

        // ハイライト時の色
        targetColor = new Color32(255, 255, 255, 255);
    }

    private void Update()
    {
        if (isHighlight)    //キャラクターををハイライトする
        {
            elapsedTime += Time.deltaTime;
            late = Mathf.Clamp01(elapsedTime / duration);
            image.color = Color32.Lerp(originalColor, targetColor, late);
        }
        else    //キャラクターをローライトする（暗くする）
        {
            elapsedTime = 0;
            late = 0;
            image.color = originalColor;    //色を戻す
        }
    }

    private void OnMouseEnter()
    {
        //キャラクターををハイライトする
        isHighlight = true;
        //画像を大きくする
        this.transform.localScale += Scale;
    }

    private void OnMouseExit()
    {
        //キャラクターをローライトする（暗くする）
        isHighlight = false;
        //画像の大きさを戻す
        this.transform.localScale -= Scale;
    }


}