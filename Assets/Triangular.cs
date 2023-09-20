using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangular : MonoBehaviour
{
    [SerializeField]
    RectTransform triangularArrow;
    [SerializeField]
    Tween a;

    void Start()
    {
        triangularArrow.DOAnchorPosX(10f, 0.35f).SetLoops(-1, LoopType.Yoyo);
    }


}
