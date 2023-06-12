using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Security.Cryptography.X509Certificates;

public class PressSpaceButton : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI text;
    [SerializeField][Range(0.1f, 10.0f)]
    float duration = 1.0f;  //テキストを点滅させる間隔

    private Color32 startColor = new Color32(0, 0, 0, 255);
    private Color32 endColor = new Color32(0, 0, 0, 60);

    void Update()
    {
        text.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time / duration, 1.0f));  //テキストを点滅させる
    }
}
