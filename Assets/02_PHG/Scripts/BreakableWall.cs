using System.Collections;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    [SerializeField] private float blastRadius = 1f;
    [SerializeField] private float blastForce = 10f;
    [SerializeField] private LayerMask fragmentLayer;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject); // ÃÑ¾Ë¿¡ ¸ÂÀ¸¸é ¹Ù·Î ÆÄ±«
        }
    }

    private void OnDestroy()
    {
        Vector3 explosionOrigin = transform.position;
        Vector3 halfExtents = new Vector3(2f, 4f, 0.5f);

        Collider[] hits = Physics.OverlapBox(explosionOrigin, halfExtents, Quaternion.identity, fragmentLayer);

        foreach (Collider hit in hits)
        {
            Rigidbody rb = hit.attachedRigidbody;
            Collider col = hit.GetComponent<Collider>();

            if (rb != null)
            {
                if (hit.TryGetComponent(out BreakableWall otherWall))
                    otherWall.enabled = false; // ¿¬¼â Æø¹ß ¹æÁö

                rb.isKinematic = false;
                if(col != null) StartCoroutine(SetTriggerAndDestroy(col.gameObject, col, 0.5f, 2f));
     
                Vector3 direction = Vector3.up + Random.insideUnitSphere * 0.3f;
                float distance = Vector3.Distance(hit.transform.position, explosionOrigin);
                float attenuation = Mathf.Clamp01(1f - (distance / blastRadius));

                rb.AddForce(direction.normalized * blastForce * attenuation, ForceMode.Impulse);


                Destroy(gameObject, 2f);
            }
        }
    }

    private IEnumerator SetTriggerAndDestroy(GameObject obj, Collider col, float triggerDelay, float destroyDelay)
    {
        yield return new WaitForSeconds(triggerDelay);
        if(col!=null)
        {
            col.isTrigger = true;

            float remaining = destroyDelay - triggerDelay;
            if (remaining > 0f)
                yield return new WaitForSeconds(remaining);

            if (obj != null)
                Destroy(obj);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 halfExtents = new Vector3(2f, 4f, 0.5f);
        Gizmos.DrawWireCube(transform.position, halfExtents * 2);
    }
}