using UnityEngine;

public class BrickAction : MonoBehaviour
{
    Rigidbody rb;

    public void BrickDestroy(Vector3 point)
    {
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        Vector3 dir = (transform.position - point).normalized;
        rb.AddForce(dir * 20, ForceMode.Impulse);
        Destroy(gameObject, 3f);
    }
}
