using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class DamageToParent : MonoBehaviour, IDamagable
{
    enum ParentType { Parent, Root }
    [SerializeField] ParentType type;
    public void TakeDamage()
    {
        if (type == ParentType.Parent)
            transform.parent.GetComponent<IDamagable>().TakeDamage();
        else if (type == ParentType.Root)
            transform.root.GetComponent<IDamagable>().TakeDamage();

    }
}
