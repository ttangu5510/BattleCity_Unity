using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushTrigger : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        Debug.Log($"Trigger 들어옴: {other.name}");

        if (other.transform.root.CompareTag("Player"))
        {
            Debug.Log("플레이어 감지 (루트 확인)");
            foreach (Outline outline in other.transform.root.GetComponentsInChildren<Outline>())
            {
                outline.enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            foreach (Outline outline in other.transform.root.GetComponentsInChildren<Outline>())
            {
                outline.enabled = false;
            }
        }
    }
}