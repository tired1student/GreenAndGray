using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool BagIsOpen = false;

    [SerializeField] private GameObject bag;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //���Ʊ����Ĵ���ر�
        if (Input.GetKeyDown(KeyCode.B))
        {
            BagIsOpen = !BagIsOpen;
            bag.SetActive(BagIsOpen);
        }
    }
}
