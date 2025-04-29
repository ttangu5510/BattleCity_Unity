using System.Collections.Generic;
using UnityEngine;

public class BulletObjectPool : MonoBehaviour
{
    private Stack<PooledObject> bulletStack;
    [SerializeField] PooledObject bulletPrefab;
    [SerializeField] int poolSize;

    private void Awake()
    {
        bulletStack = new Stack<PooledObject>();
        for (int i = 0; i < poolSize; i++)
        {
            PooledObject bullet = Instantiate(bulletPrefab, transform);
            // TODO : 발사자 정보 추가
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
        bullet.transform.rotation = Quaternion.identity;
        bullet.gameObject.SetActive(false);
        bulletStack.Push(bullet);
    }
    public int PoolCount()
    {
        return bulletStack.Count;
    }
}
