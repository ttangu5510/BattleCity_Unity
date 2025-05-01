using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushTrigger : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        Debug.Log($"Trigger ����: {other.name}");

        if (other.transform.root.CompareTag("Player"))
        {
            Debug.Log("�÷��̾� ���� (��Ʈ Ȯ��)");
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