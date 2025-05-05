using System.Collections;
using UnityEngine;

public class BrickAction : MonoBehaviour
{
    Rigidbody rb;
    Collider col;

    public void BrickDestroy(Vector3 point)
    {
        rb = gameObject.AddComponent<Rigidbody>();
        col = GetComponent<Collider>();
        if (col == null) col = gameObject.AddComponent<BoxCollider>();
        gameObject.layer = LayerMask.NameToLayer("BrokenBrick");

        Vector3 dir = (transform.position - point).normalized;
        rb.AddForce(dir * 12, ForceMode.Impulse);

        // Y�� ���� ����: �������� �����ϵ� �Ʒ��� �� ������ �Ϸ��� �ɼ� ���� ����
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        StartCoroutine(SwitchToTriggerAfterDelay(0.2f));
        if (gameObject.transform.position.y < 1f)
        {
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject, 2f);
        }
    }

    private IEnumerator SwitchToTriggerAfterDelay(float delay)
    {
        gameObject.transform.parent = null;
        yield return new WaitForSeconds(delay);

        // �߰� ���: ����� ������ ������ ���
        while (rb != null && rb.velocity.magnitude > 0.2f)
        {
            yield return null; // �� �����Ӿ� ��ٸ�
        }

        if (col != null)
        {
            col.isTrigger = true;
        }

        if (rb != null)
        {
            Destroy(rb); // ���� ������ ����
        }
    }
}