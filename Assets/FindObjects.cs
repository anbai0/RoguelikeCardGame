using System.Collections.Generic;
using UnityEngine;

public class FindObjects : MonoBehaviour
{

    [Header("�^�O��ݒ肵��1��������Object���m�F�ł��܂�")]
    public string targetTag; // �m�F�������^�O
    public List<GameObject> objectsWithTag; // �^�O���t���Ă���I�u�W�F�N�g��ێ����郊�X�g

    [Header("�R���|�[�l���g��X�N���v�g��'���s����'�A�^�b�`����2��������Object���m�F�ł��܂��B")]
    public MonoBehaviour targetComponent; // �Ώۂ̃R���|�[�l���g��X�N���v�g
    public List<GameObject> objectsWithComponent; // �R���|�[�l���g���A�^�b�`����Ă���I�u�W�F�N�g��ێ����郊�X�g




    private void Awake()
    {
        objectsWithTag = new List<GameObject>();


        objectsWithComponent = new List<GameObject>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameObject[] objectsArray = GameObject.FindGameObjectsWithTag(targetTag);

            objectsWithTag.AddRange(objectsArray);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Component[] components = null;
            components = FindObjectsOfType(targetComponent.GetType()) as Component[];

            foreach (Component component in components)
            {
                objectsWithComponent.Add(component.gameObject);
            }
        }

    }
}