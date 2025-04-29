using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentAutoCleanup : MonoBehaviour
{
    private bool isScheduledForDestroy = false;
    private Rigidbody rb;
    private bool grounded = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    private void Update()
    {
        //if (rb == null || !rb.isKinematic) return;
        //
        //if (!Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 0.7f))
        //{
        //    Debug.Log(name + ":지지대 실종 중력 활성화");
        //    rb.isKinematic = false;
        //}
    }
    private void OnCollisionEnter(Collision collision)
    {
            if (isScheduledForDestroy) return;


        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("플레이어랑 충돌 확인 완료!");
            isScheduledForDestroy = true;
            if (rb != null)
            {
                rb.isKinematic = false; 
            }

            Destroy(gameObject, 3f);
        }
        
    }
}
