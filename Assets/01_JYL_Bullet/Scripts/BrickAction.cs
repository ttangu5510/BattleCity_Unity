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

        // Y축 꺼짐 방지: 물리적은 반응하되 아래로 안 빠지게 하려면 옵션 선택 가능
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

        // 추가 대기: 충분히 떨어질 때까지 대기
        while (rb != null && rb.velocity.magnitude > 0.2f)
        {
            yield return null; // 한 프레임씩 기다림
        }

        if (col != null)
        {
            col.isTrigger = true;
        }

        if (rb != null)
        {
            Destroy(rb); // 이제 굳혀도 안전
        }
    }
}