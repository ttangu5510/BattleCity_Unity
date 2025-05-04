using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TileController : MonoBehaviour
{
    [SerializeField] private TileType tileType;
    [SerializeField] private float slideForce = 10f;
    [SerializeField] private float slowMultiplier = 0.5f;
    private Rigidbody slidingTargetRb;
    private Transform slidingTargetTransform;
    private Vector3 fixedSlideDirection;
    private bool isSliding = false;
    private Coroutine rotateCoroutine;


    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player")) //임시로 등록.
        {
                    if (isSliding)
                        return;
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb == null)
                return;

            switch (tileType)
            {
                case TileType.Ice:
                    Debug.Log("빙판 진입!");
                    
                    isSliding = true;
                    slidingTargetRb = rb;
                    slidingTargetTransform = other.transform;

                    slidingTargetRb.isKinematic = true;

                    // 이동 방향 강제 세팅
                    fixedSlideDirection = rb.velocity.magnitude > 0.1f ? rb.velocity.normalized : other.transform.forward;

                    // 여기서 아예 강제 이동
                    if (rotateCoroutine != null)
                        //TODO : StopCoroutine(rotateCoroutine);
                    rotateCoroutine = StartCoroutine(RotateWhileSliding(other.transform));
                    break;

                case TileType.Sand:
                    if (rb != null )
                    {
                        Debug.Log("모래진 입 이속저하~~");
                        rb.velocity *= slowMultiplier;
                    }
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isSliding)
            return;
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();

            switch (tileType)
            {
                case TileType.Ice:
                    if (tileType != TileType.Ice)
                        return;
                    if (slidingTargetRb != null && other.GetComponent<Rigidbody>() == slidingTargetRb)
                    {
                        Debug.Log("빙판 탈출");

                        isSliding = false;
                    
                    if (rotateCoroutine != null)
                        {
                            StopCoroutine(rotateCoroutine);
                            slidingTargetRb.isKinematic = false;
                            slidingTargetRb = null;
                            rotateCoroutine = null;
                        }
                    }
                    break;

                case TileType.Sand:
                    if (other.CompareTag("Player") && rb != null)
                    {
                        Debug.Log("캐릭터 감지 이동속도 저하 중");

                    }
                        break;
            }

        }
    }
    private void Update()
    {
        if(isSliding && slidingTargetRb !=null)
        {
          

                slidingTargetRb.velocity = fixedSlideDirection * slideForce;
        }
    }
    private IEnumerator RotateWhileSliding(Transform target)
    {
        while(isSliding&&slidingTargetTransform !=null)
        {
            slidingTargetTransform.Translate(fixedSlideDirection * slideForce * Time.deltaTime, Space.World); //슬라이딩


            yield return null;
        }
    }
}