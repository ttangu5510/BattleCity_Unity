using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushTrigger : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerBullet"))
        {
            return;
        }

        if (other.transform.root.CompareTag("Player"))
        {
            foreach (Outline outline in other.transform.root.GetComponentsInChildren<Outline>())
            {
                outline.enabled = true;
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerBullet"))
        {
            return;
        }
        if (other.transform.root.CompareTag("Player"))
        {
            foreach (Outline outline in other.transform.root.GetComponentsInChildren<Outline>())
            {
                outline.enabled = false;
            }
        }
    }
}