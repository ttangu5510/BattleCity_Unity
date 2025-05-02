using UnityEngine;

public class BrickActionCustom : MonoBehaviour
{
    Rigidbody rb;

    public void BrickDestroy(Vector3 point)
    {
        rb = gameObject.AddComponent<Rigidbody>();
        Vector3 dir = (transform.position - point).normalized;
        rb.AddForce(dir * 20, ForceMode.Impulse);
        Destroy(gameObject, 3f);
    }
}
