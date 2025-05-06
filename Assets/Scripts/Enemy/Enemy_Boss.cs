using CJM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss : MonoBehaviour
{
    [SerializeField] private Vector3 dir;
    [SerializeField] private float seedMin_DirChangeCycle;
    [SerializeField] private float seedMax_DirChangeCycle;
    [SerializeField] private float moveSpeed;
    public EnemyState state;


    // �������� �Ŵ����� ������ �� �ִ� ���� �����. �Ͻ�������. �ͷ��鵵 ����

    [SerializeField] GameObject onTriggerObj;
    Coroutine moveCycle;
    private void Start()
    {
        SwitchingDir();
        moveCycle = StartCoroutine(MovePattern());
    }


    public void SwitchingDir()
    {
        if (dir == Vector3.right)
            dir = Vector3.left;
        else if (dir == Vector3.left)
            dir = Vector3.right;
        else dir = Vector3.right;
    }
    public void Move(Vector3 dir)
    {
        transform.Translate(Time.deltaTime * moveSpeed * dir);
    }

    IEnumerator MovePattern()
    {
        bool flag = false;
        while (true)
        {
            if (state == EnemyState.Stop)
            {
                yield return new WaitUntil(() => state == EnemyState.General);
            }

            // ���� �ִ� ���
            if (onTriggerObj != null && !flag)
            {
                flag = true;
                SwitchingDir();
                Move(dir);
                yield return null;
                continue;
            }
            Move(dir);
            flag = false;
            yield return null;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            onTriggerObj = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            onTriggerObj = null;
        }
    }

    private void OnDisable()
    {
        if (moveCycle != null)
            StopCoroutine(moveCycle);
    }
}
