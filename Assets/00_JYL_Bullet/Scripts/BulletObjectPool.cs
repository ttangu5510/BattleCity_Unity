using System.Collections.Generic;
using UnityEngine;

public class BulletObjectPool : MonoBehaviour
{
    private Stack<PooledObject> bulletStack;
    [SerializeField] PooledObject bulletPrefab;
    [SerializeField] GameObject bulletOutEffect;
    [SerializeField] int poolSize;
    private void Awake()
    {
        bulletStack = new Stack<PooledObject>();
        for (int i = 0; i < poolSize; i++)
        {
            PooledObject bullet = Instantiate(bulletPrefab, transform);

            bullet.gameObject.SetActive(false);
            bulletStack.Push(bullet);
        }


    }

    public PooledObject BulletOut()
    {
        PooledObject bulletOut = bulletStack.Pop();
        bulletOut.returnPool = this;
        bulletOut.gameObject.SetActive(true);
        bulletOut.GetComponent<Rigidbody>();

        return bulletOut;
    }

    public void ReturnToPool(PooledObject bullet)
    {
        Instantiate(bulletOutEffect,transform.position, transform.rotation,transform);
        bullet.transform.rotation = Quaternion.identity;
        bullet.gameObject.SetActive(false);
        bulletStack.Push(bullet);
    }
}
