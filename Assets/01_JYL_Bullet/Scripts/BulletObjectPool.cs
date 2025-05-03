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
            bullet.gameObject.SetActive(false);
            bulletStack.Push(bullet);
        }
    }

    public PooledObject BulletOut()
    {
        if (bulletStack.Count <= 0)
        {
            Debug.Log("Ǯ ������Ʈ ��� ����!");
            return null;
        }
        else
        {
            PooledObject bulletOut = bulletStack.Pop();
            bulletOut.returnPool = this;
            // bulletOut.gameObject.SetActive(true); �÷��̾�� Ȱ��ȭ

            return bulletOut;
        }
    }

    public void ReturnToPool(PooledObject bullet)
    {
        bullet.transform.rotation = Quaternion.identity;
        // bullet.gameObject.SetActive(false); ��ü ��ü �浹 �ÿ� ��Ȱ��ȭ
        bulletStack.Push(bullet);
    }
    public int PoolCount()
    {
        return bulletStack.Count;
    }
}
