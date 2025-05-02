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

        if (other.CompareTag("Player")) //�ӽ÷� ���.
        {
                    if (isSliding)
                        return;
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb == null)
                return;

            switch (tileType)
            {
                case TileType.Ice:
                    Debug.Log("���� ����!");
                    
                    isSliding = true;
                    slidingTargetRb = rb;
                    slidingTargetTransform = other.transform;

                    slidingTargetRb.isKinematic = true;

                    // �̵� ���� ���� ����
                    fixedSlideDirection = rb.velocity.magnitude > 0.1f ? rb.velocity.normalized : other.transform.forward;

                    // ���⼭ �ƿ� ���� �̵�
                    if (rotateCoroutine != null)
                        //TODO : StopCoroutine(rotateCoroutine);
                    rotateCoroutine = StartCoroutine(RotateWhileSliding(other.transform));
                    break;

                case TileType.Sand:
                    if (rb != null )
                    {
                        Debug.Log("���� �� �̼�����~~");
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
                        Debug.Log("���� Ż��");

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
                        Debug.Log("ĳ���� ���� �̵��ӵ� ���� ��");

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
            slidingTargetTransform.Translate(fixedSlideDirection * slideForce * Time.deltaTime, Space.World); //�����̵�


            yield return null;
        }
    }
}