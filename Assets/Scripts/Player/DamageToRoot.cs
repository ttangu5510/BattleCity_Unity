using UnityEngine;

public class DamageToParent : MonoBehaviour, IDamagable
{
    public void TakeDamage()
    {
        if (transform.parent.gameObject.activeSelf == true)
            transform.parent.GetComponentInParent<IDamagable>().TakeDamage();
    }
}
