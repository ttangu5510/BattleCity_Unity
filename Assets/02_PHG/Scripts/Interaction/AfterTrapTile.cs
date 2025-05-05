using UnityEngine;
using System.Collections;

public class DangerTile : MonoBehaviour
{
    private Collider tileCollider;
    private Rigidbody rb;
    private bool hasSolidified = false;
    private bool playerInside = false;

    private void Start()
    {
        tileCollider = GetComponent<Collider>();
        if (tileCollider == null)
        {
            Debug.LogError("[DangerTile] Collider�� �����ϴ�!");
            return;
        }

        tileCollider.isTrigger = true;

        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.isKinematic = true;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasSolidified || !other.CompareTag("Player")) return;

        playerInside = true;
        StartCoroutine(WaitAndSolidify());
    }

    private void OnTriggerExit(Collider other)
    {
        if (hasSolidified || !other.CompareTag("Player")) return;

        playerInside = false;
    }

    private IEnumerator WaitAndSolidify()
    {
        // ����� �� ���·� ������ �ð� Ȯ��
        yield return new WaitForSeconds(0.2f);

        // ���� ������ ���
        while (playerInside)
        {
            yield return null;
        }

        Solidify();
    }

    private void Solidify()
    {
        if (hasSolidified) return;

        tileCollider.isTrigger = false;

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.isKinematic = false;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

        hasSolidified = true;

        Debug.Log("[DangerTile] �÷��̾� ��� �� ���� �Ұ� ��ȯ �Ϸ�");
    }
}