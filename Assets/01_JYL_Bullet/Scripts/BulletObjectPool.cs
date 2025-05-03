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
            Debug.Log("풀 오브젝트 모두 소진!");
            return null;
        }
        else
        {
            PooledObject bulletOut = bulletStack.Pop();
            bulletOut.returnPool = this;
            // bulletOut.gameObject.SetActive(true); 플레이어에서 활성화

            return bulletOut;
        }
    }

    public void ReturnToPool(PooledObject bullet)
    {
        bullet.transform.rotation = Quaternion.identity;
        // bullet.gameObject.SetActive(false); 객체 자체 충돌 시에 비활성화
        bulletStack.Push(bullet);
    }
    public int PoolCount()
    {
        return bulletStack.Count;
    }
}
