using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class DamageToParent : MonoBehaviour, IDamagable
{
    public void TakeDamage()
    {
        transform.parent.GetComponentInParent<IDamagable>().TakeDamage();
    }
}
