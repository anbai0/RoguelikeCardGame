using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventTrigger a;
        a.OnPointerClick?.gameObject.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TestButton(GameObject g)
    {
        Debug.Log($"{g}　がクリックされました");
    }
}
