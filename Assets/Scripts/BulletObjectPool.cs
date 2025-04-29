using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BulletObjectPool : MonoBehaviour
{
    private Stack<PooledObject> bulletStack;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] int poolSize;
    private void Awake()
    {
        bulletStack = new Stack<PooledObject>();
        for (int i = 0; i < poolSize; i++)
        {
            //PooledObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        }
    }
}
