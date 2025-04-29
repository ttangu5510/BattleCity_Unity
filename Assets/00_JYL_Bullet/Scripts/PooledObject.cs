using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
    public BulletObjectPool returnPool;
    
    private void OnCollisionEnter(Collision collision)
    {
        returnPool.ReturnToPool(this);
    }

}
