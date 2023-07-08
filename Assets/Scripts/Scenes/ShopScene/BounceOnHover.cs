using DG.Tweening;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BounceOnHover : MonoBehaviour
{
    public RectTransform targetObject;

    private void Start()
    {



    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            targetObject.DOAnchorPos(new Vector3(0f, 50f, 0f), 0.6f).SetEase(Ease.OutBack);





        }
    }
}
