using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIcon : MonoBehaviour
{
    [SerializeField] Image playerIcon;
    float speed = 4.0f;
    float t;

    void Update()
    {
        var tempColor = playerIcon.color;
        t += Time.deltaTime;
        tempColor.a = Mathf.Sin(speed * t) / 2 + 0.5f;
        playerIcon.color = tempColor;
    }
}
