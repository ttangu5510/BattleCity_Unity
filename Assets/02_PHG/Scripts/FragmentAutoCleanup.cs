using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentAutoCleanup : MonoBehaviour
{
    private Rigidbody rb;
    private bool isFalling = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //if (rb == null || !rb.isKinematic) return;
        //if (!isFalling)
        //{
        //    if (!Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 0.5f))
        //    {
        //        Debug.Log(name + ":지지대 실종 중력 활성화");
        //        rb.isKinematic = false;
        //        isFalling = true;
        //    }
        //}
        //else
        //{
        //    fallTimer += Time.deltaTime;
        //    if(fallTimer> 3f && !isScheduledForDestroy)
        //    {
        //        isScheduledForDestroy = true;
        //        Destroy(gameObject);
        //    }
        //}
    }
    private void OnCollisionEnter(Collision collision)
    {
            
        if(!isFalling && collision.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("플레이어 충돌로 중력 적용");
            if (rb != null)
                rb.isKinematic = false;
            isFalling = true;
        }

        Destroy(gameObject);

     
    }
}
