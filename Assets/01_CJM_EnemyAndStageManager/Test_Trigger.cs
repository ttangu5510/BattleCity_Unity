using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Trigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("온트리거");
    }
}
